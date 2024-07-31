using Shashki.ComponentsAdder.Data;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Shashki.ComponentsAdder
{
    public class ComponentsAdder
    {
        public static void AddScripts(GameObject gameObject, ComponentsData componentsData)
        {
            foreach (var script in componentsData.scripts)
            {
                if (script != null)
                {
                    if (gameObject.GetComponent(script.GetClass()) == null)
                    {
                        MonoImporter.SetExecutionOrder(script, 100);
                        var addedScript = gameObject.AddComponent(script.GetClass());
                        EditorUtility.CopySerialized(script, addedScript);
                    }
                }
                else
                {
                    Debug.LogWarning("Script at index <color=red><b>" + componentsData.scripts.IndexOf(script) + "</b></color> is null!");
                }
            }
        }

        public static void AddScripts(IEnumerable<GameObject> gameObjects, ComponentsData componentsData)
        {
            foreach (var obj in gameObjects)
            {
                AddScripts(obj, componentsData);
            }
        }


        public static void AddComponents(GameObject gameObject, ComponentsData componentsData)
        {
            foreach (var component in componentsData.components)
            {
                if (component != null)
                {
                    if (gameObject.GetComponent(component.GetType()) == null)
                    {
                        Component copiedComponent = gameObject.AddComponent(component.GetType());
                        EditorUtility.CopySerialized(component, copiedComponent);
                    }
                }
                else
                {
                    Debug.LogWarning("Component at index <color=red><b>" + componentsData.components.IndexOf(component) + "</b></color> is null!");
                }
            }
        }

        public static void AddComponents(IEnumerable<GameObject> gameObjects, ComponentsData componentsData)
        {
            foreach (var obj in gameObjects)
            {
                AddComponents(obj, componentsData);
            }
        }

        /// <summary>
        /// Add scripts and components
        /// </summary>
        public static void AddAll(GameObject gameObject, ComponentsData componentsData)
        {
            AddScripts(gameObject, componentsData);
            AddComponents(gameObject, componentsData);
        }

        /// <summary>
        /// Add scripts and components
        /// </summary>
        public static void AddAll(IEnumerable<GameObject> gameObjects, ComponentsData componentsData)
        {
            AddScripts(gameObjects, componentsData);
            AddComponents(gameObjects, componentsData);
        }
    }
}