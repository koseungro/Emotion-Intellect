/// 작성자: 백인성 
/// 작성일: 2018-09-11 
/// 수정일: 2018-09-11
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력 
/// 

using UnityEngine;
using UnityEditor;

/// <summary>
/// IS_ButtonData를 커스터마이즈 합니다.
/// </summary>
[CustomEditor(typeof(IS_ButtonData))]
[CanEditMultipleObjects]
public class IS_CustomUIButtonData : Editor
{
	private IS_ButtonData m_buttonData;

	void OnEnable()
	{
		m_buttonData = base.target as IS_ButtonData;
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical();
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
			{
				m_buttonData.IsChangeText = GUILayout.Toggle(m_buttonData.IsChangeText, "Change Name", EditorStyles.miniButton, GUILayout.Width(80), GUILayout.Height(20));

				if (m_buttonData.IsChangeText)
					m_buttonData.buttonName = EditorGUILayout.TextField(m_buttonData.buttonName);
				else
					EditorGUILayout.LabelField("Use Button In Text");

			} EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
			{
				EditorGUILayout.LabelField("Use Event", EditorStyles.whiteLargeLabel, GUILayout.Width(80));
				if (GUILayout.Button("None", EditorStyles.miniButton, GUILayout.Width(80), GUILayout.Height(20)))
					m_buttonData.useEvent = UseButtonEventType.None;

				if (GUILayout.Toggle(CheckType(m_buttonData.useEvent, UseButtonEventType.Enter), "Enter", EditorStyles.miniButtonLeft, GUILayout.Height(20)))
					m_buttonData.useEvent |= UseButtonEventType.Enter;
				else
					m_buttonData.useEvent &= ~UseButtonEventType.Enter;

				if (GUILayout.Toggle(CheckType(m_buttonData.useEvent, UseButtonEventType.Exit), "Exit", EditorStyles.miniButtonMid, GUILayout.Height(20)))
					m_buttonData.useEvent |= UseButtonEventType.Exit;
				else
					m_buttonData.useEvent &= ~UseButtonEventType.Exit;

				if (GUILayout.Toggle(CheckType(m_buttonData.useEvent, UseButtonEventType.Drag), "Drag", EditorStyles.miniButtonMid, GUILayout.Height(20)))
					m_buttonData.useEvent |= UseButtonEventType.Drag;
				else
					m_buttonData.useEvent &= ~UseButtonEventType.Drag;

				if (GUILayout.Toggle(CheckType(m_buttonData.useEvent, UseButtonEventType.Down), "Down", EditorStyles.miniButtonMid, GUILayout.Height(20)))
					m_buttonData.useEvent |= UseButtonEventType.Down;
				else
					m_buttonData.useEvent &= ~UseButtonEventType.Down;

				if (GUILayout.Toggle(CheckType(m_buttonData.useEvent, UseButtonEventType.Up), "Up", EditorStyles.miniButtonRight, GUILayout.Height(20)))
					m_buttonData.useEvent |= UseButtonEventType.Up;
				else
					m_buttonData.useEvent &= ~UseButtonEventType.Up;

			} EditorGUILayout.EndHorizontal();

		} EditorGUILayout.EndVertical();

		EditorGUILayout.Space();

		EditorGUILayout.BeginVertical();
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField("Change", EditorStyles.whiteLargeLabel, GUILayout.Width(80), GUILayout.Height(20));

					m_buttonData.IsChangeFontSize = GUILayout.Toggle(m_buttonData.IsChangeFontSize, "Font Size", EditorStyles.miniButtonLeft, GUILayout.Height(20));
					m_buttonData.IsFlexibility = GUILayout.Toggle(m_buttonData.IsFlexibility, "Flexibility", EditorStyles.miniButtonMid, GUILayout.Height(20));
					EditorGUI.BeginDisabledGroup(m_buttonData.IsFlexibility);
					m_buttonData.useImageSize = GUILayout.Toggle(m_buttonData.useImageSize, "Image Size", EditorStyles.miniButtonRight, GUILayout.Height(20));
					EditorGUI.EndDisabledGroup();

				} EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUI.BeginDisabledGroup(!m_buttonData.IsChangeFontSize);
						{ 
							EditorGUILayout.LabelField("Font Size", GUILayout.Width(100), GUILayout.Height(20));
							m_buttonData.FontSize = EditorGUILayout.IntField(m_buttonData.FontSize);

						} EditorGUI.EndDisabledGroup();

					} EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					{
						EditorGUI.BeginDisabledGroup(!m_buttonData.IsFlexibility);
						{
							EditorGUILayout.LabelField("Margin(LR)", GUILayout.Width(100), GUILayout.Height(20));
							m_buttonData.FontMargin = EditorGUILayout.IntField(m_buttonData.FontMargin);

						} EditorGUI.EndDisabledGroup();

						if (m_buttonData.IsFlexibility)
							m_buttonData.useImageSize = false;

					} EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					{
						EditorGUI.BeginDisabledGroup(m_buttonData.IsFlexibility);
						{
							EditorGUILayout.LabelField("Image Size", GUILayout.Width(100), GUILayout.Height(20));

							if (m_buttonData.useImageSize || m_buttonData.IsFlexibility)
							{
								EditorGUI.BeginDisabledGroup(true);
								EditorGUILayout.Vector2Field("", m_buttonData.GetDefaultImage == null ?
																 new Vector2() : 
																 new Vector2(m_buttonData.GetDefaultImage.texture.width, 
															 				 m_buttonData.GetDefaultImage.texture.height));
								EditorGUI.EndDisabledGroup();
							}
							else
							{
								m_buttonData.size = EditorGUILayout.Vector2Field("", m_buttonData.size);
							}

						} EditorGUI.EndDisabledGroup();

					} EditorGUILayout.EndHorizontal();
				} EditorGUILayout.EndVertical();
			} EditorGUILayout.EndVertical();

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Transition", EditorStyles.whiteLargeLabel, GUILayout.Height(20));
				m_buttonData.isObject = GUILayout.Toggle(m_buttonData.isObject, "Is Object", EditorStyles.miniButton);
			} EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginVertical();
			{
				EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
				{
					EditorGUILayout.LabelField("Base", EditorStyles.whiteLargeLabel, GUILayout.Width(60), GUILayout.Height(20));
					EditorGUILayout.BeginVertical();
					{
						m_buttonData.Base = TransitionUI(m_buttonData.Base,
														 "Use Option",
														 !m_buttonData.useTextColor,
														 !m_buttonData.useImageColor,
														 !m_buttonData.useIconColor,
														 !m_buttonData.useImage,
														 !m_buttonData.useIcon,
														 !m_buttonData.useScale);

					} EditorGUILayout.EndVertical();

				} EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField("Options", EditorStyles.whiteLargeLabel, GUILayout.Width(60), GUILayout.Height(20));

						EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(100));
						{
							EditorGUI.BeginDisabledGroup(!m_buttonData.UseTransition);
							{
								EditorGUILayout.LabelField("Time", GUILayout.Width(50));
								m_buttonData.transitionTime = EditorGUILayout.FloatField(m_buttonData.transitionTime, GUILayout.Width(50));

							} EditorGUI.EndDisabledGroup();

						} EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginVertical();
						{
							if (m_buttonData.isObject)
							{
								EditorGUILayout.BeginHorizontal();
								{
									m_buttonData.useTextColor = GUILayout.Toggle(m_buttonData.useTextColor, "Text Color", EditorStyles.miniButtonLeft);
									m_buttonData.useImageColor = GUILayout.Toggle(m_buttonData.useImageColor, "Object Color", EditorStyles.miniButtonMid);
									m_buttonData.useScale = GUILayout.Toggle(m_buttonData.useScale, "Scale", EditorStyles.miniButtonRight);
								}
							}
							else
							{
								EditorGUILayout.BeginHorizontal();
								{
									m_buttonData.useTextColor = GUILayout.Toggle(m_buttonData.useTextColor, "Text Color", EditorStyles.miniButtonLeft);
									m_buttonData.useImageColor = GUILayout.Toggle(m_buttonData.useImageColor, "Image Color", EditorStyles.miniButtonMid);
									m_buttonData.useIconColor = GUILayout.Toggle(m_buttonData.useIconColor, "Icon Color", EditorStyles.miniButtonRight);
								}
								EditorGUILayout.EndHorizontal();
								EditorGUILayout.BeginHorizontal();
								{
									m_buttonData.useImage = GUILayout.Toggle(m_buttonData.useImage, "Image", EditorStyles.miniButtonLeft);
									m_buttonData.useIcon = GUILayout.Toggle(m_buttonData.useIcon, "Icon", EditorStyles.miniButtonMid);
									m_buttonData.useScale = GUILayout.Toggle(m_buttonData.useScale, "Scale", EditorStyles.miniButtonRight);
								}
							}
							EditorGUILayout.EndHorizontal();
						} EditorGUILayout.EndVertical();

					} EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal(GUI.skin.box);
					{
						EditorGUILayout.LabelField("Default", GUILayout.Width(60));
						EditorGUILayout.BeginVertical();
						{
							m_buttonData.Default = TransitionUI(m_buttonData.Default,
															    "Use Base",
															    m_buttonData.useTextColor,
															    m_buttonData.useImageColor,
																m_buttonData.useIconColor,
															    m_buttonData.useImage,
																m_buttonData.useIcon,
																m_buttonData.useScale);

						} EditorGUILayout.EndVertical();

					} EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal(GUI.skin.box);
					{
						EditorGUILayout.LabelField("Hover", GUILayout.Width(60));
						EditorGUILayout.BeginVertical();
						{
							m_buttonData.Hover = TransitionUI(m_buttonData.Hover,
															  "Use Base",
															  m_buttonData.useTextColor,
															  m_buttonData.useImageColor,
															  m_buttonData.useIconColor,
															  m_buttonData.useImage,
															  m_buttonData.useIcon,
															  m_buttonData.useScale);

						} EditorGUILayout.EndVertical();

					} EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal(GUI.skin.box);
					{
						EditorGUILayout.LabelField("Press", GUILayout.Width(60));
						EditorGUILayout.BeginVertical();
						{
							m_buttonData.Press = TransitionUI(m_buttonData.Press,
															  "Use Base",
															  m_buttonData.useTextColor,
															  m_buttonData.useImageColor,
															  m_buttonData.useIconColor,
															  m_buttonData.useImage,
															  m_buttonData.useIcon,
															  m_buttonData.useScale);

						} EditorGUILayout.EndVertical();

					} EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal(GUI.skin.box);
					{
						EditorGUILayout.LabelField("Disable", GUILayout.Width(60));
						EditorGUILayout.BeginVertical();
						{
							m_buttonData.Disable = TransitionUI(m_buttonData.Disable, 
																"Use Base", 
																m_buttonData.useTextColor, 
																m_buttonData.useImageColor,
															    m_buttonData.useIconColor,
																m_buttonData.useImage,
															    m_buttonData.useIcon,
																m_buttonData.useScale);

						} EditorGUILayout.EndVertical();
					} EditorGUILayout.EndHorizontal();
				} EditorGUILayout.EndVertical();
			} EditorGUILayout.EndVertical();
		} EditorGUILayout.EndVertical();

		//여기까지 검사해서 필드에 변화가 있으면
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObjects(targets, "Changed Update Mode");
			//변경이 있을 시 적용된다. 이 코드가 없으면 인스펙터 창에서 변화는 있지만 적용은 되지 않는다.
			EditorUtility.SetDirty(m_buttonData);
		}
		serializedObject.ApplyModifiedProperties();
		serializedObject.Update();
	}

	private TransitionSet TransitionUI(TransitionSet target, string subTitle, params bool[] check)
	{
		TransitionSet edit = new TransitionSet();

		edit.TextColor = TextColorUI(check[0], "TextColor", subTitle, target.TextColor);
		edit.ImageColor = ImageColorUI(check[1], m_buttonData.isObject ? "ObjectColor" : "ImageColor", subTitle, target.ImageColor);
		if (m_buttonData.isObject == false)
		{
			edit.IconColor = ImageColorUI(check[2], "IconColor", subTitle, target.IconColor);
			edit.Image = ImageUI(check[3], "Image", subTitle, target.Image);
			edit.Icon = ImageUI(check[4], "Icon", subTitle, target.Icon);
		}
		edit.Scale = ScaleUI(check[5], "Scale", subTitle, target.Scale, target.type);
		edit.type = target.type;

		return edit;
	}
	private Color TextColorUI(bool check, string title, string subTitle, Color target)
	{
		if (check)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, GUILayout.Width(80));
				target = EditorGUILayout.ColorField(target);

				EditorGUILayout.EndHorizontal();
			}
		}
		else
		{
			target = Color.white;
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, GUILayout.Width(80));
				EditorGUILayout.LabelField(subTitle);

				EditorGUILayout.EndHorizontal();
			}
		}

		return target;
	}
	private Color ImageColorUI(bool check, string title, string subTitle, Color target)
	{
		if (check)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, GUILayout.Width(80));
				target = EditorGUILayout.ColorField(target);

				EditorGUILayout.EndHorizontal();
			}
		}
		else
		{
			target = Color.white;
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, GUILayout.Width(80));
				EditorGUILayout.LabelField(subTitle);

				EditorGUILayout.EndHorizontal();
			}
		}

		return target;
	}
	private Sprite ImageUI(bool check, string title, string subTitle, Sprite target)
	{
		if (check)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, GUILayout.Width(80));
				target = (Sprite)EditorGUILayout.ObjectField(target, typeof(Sprite));

				EditorGUILayout.EndHorizontal();
			}
		}
		else
		{
			target = null;
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, GUILayout.Width(80));
				EditorGUILayout.LabelField(subTitle);

				EditorGUILayout.EndHorizontal();
			}
		}

		return target;
	}
	private Vector3 ScaleUI(bool check, string title, string subTitle, Vector3 target, TransitionSet.Type type)
	{
		if (check)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, GUILayout.Width(80));
				target = EditorGUILayout.Vector3Field("", target);

				EditorGUILayout.EndHorizontal();
			}
		}
		else
		{
			switch (type)
			{
				case TransitionSet.Type.Hover:   target = Vector3.one * 1.1f; break;
				case TransitionSet.Type.Press:   target = Vector3.one * 0.9f; break;
				default:						 target = Vector3.one;		  break;
			}
			
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, GUILayout.Width(80));
				EditorGUILayout.LabelField(subTitle);

				EditorGUILayout.EndHorizontal();
			}
		}

		return target;
	}
	private bool CheckType(UseButtonEventType dataType, UseButtonEventType check)
	{
		return (dataType & check) == check;
	}
}
