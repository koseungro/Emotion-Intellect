using System.Collections;
// 최초작성자: 조효련
// 최초작성일: 2019-11-27

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// 컬러를 가진 헤더 속성 클래스
namespace FNI
{
    public class ColorHeaderAttribute : PropertyAttribute
    {
        public Color color;
        public string header;

        public ColorHeaderAttribute(string header)
        {
            this.color = new Color32(254, 161, 0, 255);
            this.header = header;
        }

        public ColorHeaderAttribute(Color color, string header)
        {
            this.color = color;
            this.header = header;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ColorHeaderAttribute))]
    internal sealed class RangeDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight * 1.5f;
        }

        public override void OnGUI(Rect position)
        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ColorHeaderAttribute headAttribute = (ColorHeaderAttribute)attribute;

            GUIStyle HeaderStyle = new GUIStyle(EditorStyles.boldLabel);
            HeaderStyle.normal.textColor = headAttribute.color;

            position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;
            position = EditorGUI.IndentedRect(position);

            GUI.Label(position, headAttribute.header, HeaderStyle);
        }
    }
#endif
}
