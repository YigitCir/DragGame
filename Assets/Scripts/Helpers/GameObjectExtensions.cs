using UnityEngine;

namespace DefaultNamespace.Helpers
{
    public static class GameObjectExtensions
    {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (!component) component = gameObject.AddComponent<T>();
            return component;
        }

        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

        public static void DestroyChildren(this GameObject gameObject)
        {
            gameObject.transform.DestroyChildren();
        }
        
        public static void EnableChildren(this GameObject gameObject)
        {
            gameObject.transform.EnableChildren();
        }
        
        public static void DisableChildren(this GameObject gameObject)
        {
            gameObject.transform.DisableChildren();
        }

    }
}