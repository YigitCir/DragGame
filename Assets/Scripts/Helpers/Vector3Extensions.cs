﻿using UnityEngine;

namespace DefaultNamespace.Helpers
{
    public static class Vector3Extensions
    {
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        
        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
        }
        
        public static Vector3 Multiply(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector.x * (x ?? 1), vector.y * (y ?? 1), vector.z * (z ?? 1));
        }

        public static void test()
        {
            var vec = Vector3.one;
            vec.Add(z: 1f);
        }
        
    }
}