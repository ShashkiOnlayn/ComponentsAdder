using Shashki.ComponentsAdder.Data;
using UnityEditor;
using UnityEngine;

namespace Shashki.ComponentsAdder.Editor
{
    [CustomEditor(typeof(ComponentsData))]
    public class ComponentsDataEditor : UnityEditor.Editor
    {
        private GUIStyle _labelStyle = new();

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

            EditorGUILayout.LabelField(_currentDataName, new GUIStyle(EditorStyles.label) { fontSize = 18, alignment = TextAnchor.MiddleCenter, normal = new() { textColor = Color.cyan } });

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