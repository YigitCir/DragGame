using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MaterialProvider : MonoBehaviour
    {
        public static MaterialProvider Instance;

        public List<Material> DraggableMaterials;

        private void Awake()
        {
            if(Instance!=null) DestroyImmediate(Instance.gameObject);
            Instance = this;
        }

        public Material GetDraggableMaterial(DragState state)
        {
            return DraggableMaterials[(int) state];
        }
    }
}