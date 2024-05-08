﻿using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Helpers
{
    public static class TransformExtensions
    {
        public static IEnumerable<Transform> Children(this Transform parent)
        {
            foreach (Transform child in parent)
            {
                yield return child;
            }
        }

        public static void DestroyChildren(this Transform parent)
        {
            parent.PerformActionOnChildren(child => Object.Destroy(child.gameObject));
        }
        
        public static void EnableChildren(this Transform parent)
        {
            parent.PerformActionOnChildren(child => child.gameObject.SetActive(true));
        }
        
        public static void DisableChildren(this Transform parent)
        {
            parent.PerformActionOnChildren(child => child.gameObject.SetActive(false));
        }

        static void PerformActionOnChildren(this Transform parent, System.Action<Transform> action)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                action(parent.GetChild(i));
            }
        }
    }
}