using UnityEngine;

namespace DefaultNamespace.Helpers
{
    public class SaveableSingleton<T> : ScriptableObject where T: SaveableSingleton<T>
    {
        
    }
}