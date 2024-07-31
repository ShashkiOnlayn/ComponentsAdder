using Shashki.ComponentsAdder.Data;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Shashki.ComponentsAdder.Editor
{
    public class ComponentsAdderWindow : EditorWindow
    {
        private Texture2D windowLogo { get; set; } 

        public ComponentsData currentData;
        public string customDataName = "CustomData";

        public List<MonoScript> scripts = new();
        public List<Component> components = new();

        public bool enableScripts = false;
        public bool enableComponents = false;

        private List<ComponentsData> _componentsDatas;
        private List<string> DatasName = new();
        private string _customComponentsDataPath = @"Assets/ComponentsAdder/Resources/ComponentsData/";

        private bool _scriptsAreInvalid = false;
        private int _selectedIndex;

        private Vector2 _scrollPos = new();
        private SerializedObject serializedObject { get; set; }

        private GUIStyle _deleteDataButtonStyle;

        [MenuItem("Window/Shashki assets/Components Adder")]
        public static void ShowWindow()
        {
            var window = GetWindow<ComponentsAdderWindow>("Components Adder");
            window.minSize = new Vector2(350, 470);
            window.windowLogo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ComponentsAdder/Editor/logo.png");
            window.titleContent = new(window.title, window.windowLogo);
        }

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);

            _componentsDatas = Resources.LoadAll<ComponentsData>("ComponentsData").ToList();
            try
            {
                currentData = _componentsDatas[_selectedIndex];
                scripts = currentData.scripts;
                components = currentData.components;

                enableComponents = currentData.components.Count > 0;
                enableScripts = currentData.scripts.Count > 0;
            }
            catch { }
        }

        private void OnGUI()
        {
            #region SerializeField
            _componentsDatas = Resources.LoadAll<ComponentsData>("ComponentsData").ToList();

            #region Error
            if (_componentsDatas.Count == 0)
            {
                GUIStyle errorStyle = new(GUI.skin.label) { fontSize = 15, fontStyle = FontStyle.Bold };
                errorStyle.normal.textColor = Color.red;
                errorStyle.alignment = TextAnchor.MiddleCenter;
                errorStyle.wordWrap = true;

                GUILayout.FlexibleSpace();
                GUILayout.Label($"{_customComponentsDataPath} does not contains ComponentsData", errorStyle);
                GUILayout.Space(30);

                GUILayout.BeginHorizontal();
                GUILayout.Space(30);

                if (GUILayout.Button("Create Empty data", GUILayout.Height(40)))
                {
                    scripts.Clear();
                    components.Clear();
                    customDataName = "Empty";
                    SaveCustomData();
                    customDataName = "CustomData";
                }

                GUILayout.Space(30);
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                return;
            }

            #endregion

            currentData = _componentsDatas[_selectedIndex];

            serializedObject.Update();

            SerializedProperty scriptsProperty = serializedObject.FindProperty(nameof(scripts));
            SerializedProperty componentsProperty = serializedObject.FindProperty(nameof(components));

            #region Calculate Scrollview
            var countsSum = scripts.Count + components.Count;
            var needScroll = countsSum >= 4;
            if (needScroll)
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(position.height - (countsSum - 5)));
            }
            #endregion

            #region Label
            GUILayout.Space(10);
            GUILayout.Label("Components Adder", new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = 20 });
            GUILayout.Label("by ShashkiOnlayn", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter, fontSize = 12 });
            GUILayout.Label("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);
            #endregion

            #region ComponentsData
            foreach (var item in _componentsDatas)
            {
                DatasName.Add(item.name);
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Choose components data", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 15 });
            _selectedIndex = EditorGUILayout.Popup(_selectedIndex, DatasName.ToArray());
            if (GUI.changed)
            {
                currentData = _componentsDatas[_selectedIndex];
                scripts = currentData.scripts;
                components = currentData.components;

                enableComponents = components.Count > 0;
                enableScripts = scripts.Count > 0;
            }
            DatasName.Clear();
            EditorGUI.EndChangeCheck();
            EditorGUILayout.EndHorizontal();

            SetDeleteDataButtonStyle();

            GUILayout.BeginHorizontal();
            GUILayout.Space(47.5f);

            if (GUILayout.Button("Delete chosen Data", _deleteDataButtonStyle))
            {
                ShowDeleteDataWindow();
            }

            GUILayout.Space(47.5f);
            GUILayout.EndHorizontal();

            #region CustomData
            GUILayout.Space(5);
            GUIStyle guiLabel = new(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = 15 };
            guiLabel.normal.textColor = Color.yellow;
            EditorGUILayout.LabelField("Custom ComponentsData", guiLabel);
            EditorGUILayout.BeginHorizontal();
            customDataName = EditorGUILayout.TextField("Your custom Data name", customDataName);

            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Save custom data"))
            {
                SaveCustomData();
            }
            #endregion

            EditorGUILayout.LabelField("", new GUIStyle(GUI.skin.horizontalSlider));
            GUILayout.Space(5);
            #endregion

            #region Scripts
            enableScripts = EditorGUILayout.ToggleLeft(new GUIContent("Enable scripts", "Will scripts be added to the objects?"), enableScripts);

            GUI.enabled = enableScripts;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(scriptsProperty, new GUIContent("Scripts", "Scripts, which will added to selected objects"));
            if (GUI.changed)
            {
                currentData.scripts = scripts;
                Debug.Log("Changed");
            }
            EditorGUI.EndChangeCheck();
            GUI.enabled = true;

            GUILayout.Space(5);
            #endregion

            #region Components
            enableComponents = EditorGUILayout.ToggleLeft(new GUIContent("Enable components", "Will components be added to the objects?"), enableComponents);

            GUI.enabled = enableComponents;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(componentsProperty, new GUIContent("Components", "Components, which will added to selected objects"));
            if (GUI.changed)
            {
                currentData.components = components;
            }
            EditorGUI.EndChangeCheck();
            GUI.enabled = true;
            GUILayout.Space(5);
            #endregion

            #endregion

            #region Add components button

            if (GUILayout.Button("Add components", new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Height(30)))
            {
                _scriptsAreInvalid = false;
                if (Selection.gameObjects.Length > 1)
                {
                    foreach (var obj in Selection.gameObjects)
                    {
                        if (enableScripts)
                        {
                            foreach (var script in currentData.scripts)
                            {
                                if (script != null)
                                {
                                    if (script.GetClass().IsStatic())
                                    {
                                        Debug.LogError("Scripts cannot be static! (script: " + script.GetClass() + ")");
                                        _scriptsAreInvalid = true;
                                    }
                                }
                            }

                            if (_scriptsAreInvalid == false)
                            {
                                ComponentsAdder.AddScripts(obj, currentData);
                            }
                        }

                        if (enableComponents)
                        {
                            ComponentsAdder.AddComponents(obj, currentData);
                        }

                        Undo.RecordObject(obj, "Added components and scripts");
                    }
                }
                else
                {
                    Debug.LogError("Need use with a lot of selected objects (more than 1)!");
                }
            }

            #region Calculate Scrollview
            if (needScroll)
            {
                EditorGUILayout.EndScrollView();
            }
            #endregion

            serializedObject.ApplyModifiedProperties();

            #endregion
        }

        private void SaveCustomData()
        {
            if (_componentsDatas.Count > 0)
            {
                _componentsDatas.ForEach(d => DatasName.Add(d.name));
            }

            if (DatasName.Contains(customDataName) == false)
            {
                var newPreset = CreateInstance<ComponentsData>();

                scripts.ForEach(s => newPreset.scripts.Add(s));
                components.ForEach(c => newPreset.components.Add(c));
                _componentsDatas.Add(newPreset);
                EditorUtility.SetDirty(newPreset);
                AssetDatabase.CreateAsset(newPreset, _customComponentsDataPath + customDataName + ".asset");
                AssetDatabase.SaveAssets();
                Debug.LogFormat("Asset saved at <color=cyan><b>{0}</b></color>", _customComponentsDataPath + customDataName + ".asset");
            }
            else
            {
                Debug.LogError("This preset already exists! Please, rename it!");
            }
            DatasName.Clear();
        }

        private void ShowDeleteDataWindow()
        {
            bool answer = EditorUtility.DisplayDialog("Delete data", $"Do you really want to delete {currentData.name}?", "Yes", "No");
            if (answer)
                DeleteChosenData(currentData);
        }
        private void DeleteChosenData(ComponentsData data)
        {
            _selectedIndex = 0;
            Debug.Log($"Asset <color=yellow><b>{_customComponentsDataPath + data.name}.asset</b></color> deleted");
            AssetDatabase.DeleteAsset(_customComponentsDataPath + data.name + ".asset");
            AssetDatabase.Refresh();
        }
        private void SetDeleteDataButtonStyle()
        {
            _deleteDataButtonStyle = new(GUI.skin.button) { fontStyle = FontStyle.Bold, fontSize = 13 };

            Color[] normalPix = new Color[4], hoverPix = new Color[4];
            Color normalColor = new Color(0.8f, 0, 0, 0.7f),
                hoverColor = new Color(1f, 0, 0, 1);

            for (int i = 0; i < normalPix.Length; i++)
            {
                normalPix[i] = normalColor;
            }

            for (int i = 0; i < hoverPix.Length; i++)
            {
                hoverPix[i] = hoverColor;
            }

            Texture2D normalResult = new Texture2D(2, 2), hoverResult = new(2, 2);
            normalResult.SetPixels(normalPix);
            hoverResult.SetPixels(hoverPix);
            normalResult.Apply();
            hoverResult.Apply();
            _deleteDataButtonStyle.normal.background = normalResult;
            _deleteDataButtonStyle.hover.background = hoverResult;
        }
    }

}