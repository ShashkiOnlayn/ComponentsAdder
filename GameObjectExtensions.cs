using System.Linq;
using UnityEngine;

namespace Shashki.ComponentsAdder
{
    public static partial class GameObjectExtensions
    {
        /// <summary>
        /// If game object contains component with type <b>type</b>, then return <b>false</b>. Otherwise <b>true</b> and component will be added to the game object and assigned to the <b>component</b>
        /// </summary>
        public static bool TryAddComponent(this GameObject go, System.Type type, out Component component)
        {
            component = null;

            foreach (var c in go.GetComponents<Component>())
            {
                if (c.GetType() == type)
                    return false;
            }

            component = go.AddComponent(type);
            return true;
        }

        /// <summary>
        /// If game object contains component with type <b>T</b>, then return <b>false</b>. Otherwise <b>true</b> and component will be added to the game object and assigned to the <b>component</b>
        /// </summary>
        public static bool TryAddComponent<T>(this GameObject go, out T component) where T : Component
        {
            component = null;

            foreach (var c in go.GetComponents<Component>())
            {
                if (c.GetType() == typeof(T))
                    return false;
            }

            component = go.AddComponent<T>();
            return true;
        }

        /// <summary>
        /// Return all components on game object
        /// </summary>
        public static Component[] GetAllComponents(this GameObject go)
        {
            return go.GetComponents<Component>().Where(c => c is not MonoBehaviour).ToArray();
        }

        /// <summary>
        /// Return all components with type <b>T</b> on game object
        /// </summary>
        public static T[] GetAllComponentsWithType<T>(this GameObject go) where T : Component
        {
            return go.GetComponents<T>().Where(c => c is not MonoBehaviour).ToArray();
        }

        /// <summary>
        /// Return all <see cref="MonoBehaviour"/> scripts on game object
        /// </summary>
        public static MonoBehaviour[] GetAllScripts(this GameObject go)
        {
            return go.GetComponents<MonoBehaviour>().Where(c => c is MonoBehaviour).ToArray();
        }

        /// <summary>
        /// Return all scripts with type <b>T</b>, that inheirt <see cref="MonoBehaviour"/> on game object
        /// </summary>
        public static T[] GetAllScriptsWithType<T>(this GameObject go) where T : MonoBehaviour
        {
            return go.GetComponents<T>().Where(c => c is T).ToArray();
        }
    }
}
