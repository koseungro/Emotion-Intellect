/// 작성자: 백인성 
/// 작성일: 2018-05-01 
/// 수정일: 2018-07-25
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력 
/// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Panic2;
using TMPro;

namespace Panic2
{

    /// <summary>
    /// IS_SceneData를 커스터마이즈 합니다.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(IS_Button))]
    public class IS_Button_Editor : Editor
    {
        /// <summary>
        /// 현재 씬의 정보입니다.
        /// </summary>
        private IS_Button m_button;

        void OnEnable()
        {
            m_button = base.target as IS_Button;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("버튼 구조 만들기"))
            {
                CreateButton();
            }
        }

        private void CreateButton()
        {
            if (m_button.data)
            {
                Transform parent = CreateImage(m_button.transform, "Parent", m_button.data.GetDefaultImageColor, false, m_button.data.GetDefaultImage, m_button.data.size).transform;
                CreateText(parent, "Text");
                if (m_button.data.GetDefaultIcon)
                    CreateImage(parent, "Icon", Color.white, false, m_button.data.GetDefaultIcon);
                Image image = CreateImage(parent, "Disable", new Color(0.5f, 0.5f, 0.5f, 0.5f), true, m_button.data.GetDisableImage);
                image.enabled = false;
            }
            else
            {
                Transform parent = CreateImage(m_button.transform, "Parent", Color.white, true).transform;
                CreateText(parent, "Text");
                Image image = CreateImage(parent, "Disable", new Color(0.5f, 0.5f, 0.5f, 0.5f), true);
                image.enabled = false;
            }
        }

        private Image CreateImage(Transform parent, string createName, Color color, bool isFull = true, Sprite sprite = null, Vector2 size = new Vector2())
        {
            GameObject create = null;
            Transform find = parent.Find(createName);
            if (find)
                create = find.gameObject;
            else
            {
                create = new GameObject(createName);
                create.transform.SetParent(parent, false);
            }

            Image image = create.GetComponent<Image>();
            if (image == null)
                image = create.AddComponent<Image>();
            image.rectTransform.anchorMin = isFull ? Vector2.zero : (Vector2.one * 0.5f);
            image.rectTransform.anchorMax = isFull ? Vector2.one : (Vector2.one * 0.5f);
            image.rectTransform.anchoredPosition = Vector2.zero;
            image.rectTransform.sizeDelta = isFull ? Vector2.zero : size;

            image.type = Image.Type.Sliced;
            image.color = color;
            image.sprite = sprite;
            return image;
        }
        private TextMeshProUGUI CreateText(Transform parent, string createName)
        {
            GameObject create = null;
            Transform find = parent.Find(createName);
            if (find)
                create = find.gameObject;
            else
            {
                create = new GameObject(createName);
                create.transform.SetParent(parent, false);
            }

            TextMeshProUGUI text = create.GetComponent<TextMeshProUGUI>();
            if (text == null)
                text = create.AddComponent<TextMeshProUGUI>();
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.anchoredPosition = Vector2.zero;
            text.rectTransform.sizeDelta = Vector2.zero;

            //text.alignment = TextAnchor.MiddleCenter;
            text.alignment = TMPro.TextAlignmentOptions.Center;

            if (m_button.data)
            {
                text.text = m_button.data.IsChangeText ? m_button.data.buttonName : "Button";
                text.color = m_button.data.GetDefaultTextColor;
                text.fontSize = m_button.data.FontSize;
            }
            else
            {
                text.text = "Button";
                text.color = Color.black;
            }
            return text;
        }
    }

}
