using Shashki.ComponentsAdder.Data;
using UnityEditor;
using UnityEngine;

namespace Shashki.ComponentsAdder.Editor
{
    [CustomEditor(typeof(ComponentsData))]
    public class ComponentsDataEditor : UnityEditor.Editor
    {
        private GUIStyle _labelStyle = new();
        private GUIStyle _dataNameStyle = new();

        private string _currentDataName;

        private void OnEnable()
        {
            _currentDataName = target.name;
            SetStyles();
        }

        private void OnDisable()
        {
            _currentDataName = "";
            SetStyles();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("Components Data Settings", _labelStyle);

            EditorGUILayout.LabelField(string.Empty, new GUIStyle(GUI.skin.horizontalSlider));

            float dataNameHeight = _dataNameStyle.CalcHeight(new GUIContent(_currentDataName), EditorGUIUtility.currentViewWidth);
            EditorGUILayout.LabelField(_currentDataName, new GUIStyle(_dataNameStyle) { fontSize = 18, alignment = TextAnchor.MiddleCenter, normal = new() { textColor = Color.cyan }, fixedHeight = 0 }, GUILayout.Height(dataNameHeight));

            EditorGUILayout.LabelField(string.Empty, new GUIStyle(GUI.skin.horizontalSlider));

            EditorGUILayout.Space(5);

            base.OnInspectorGUI();
        }

        private void SetStyles()
        {
            _labelStyle.wordWrap = true;
            _labelStyle.alignment = TextAnchor.MiddleCenter;
            _labelStyle.fontSize = 30;
            _labelStyle.fontStyle = FontStyle.Bold;
            _labelStyle.normal.textColor = Color.white;
        }
    }
}