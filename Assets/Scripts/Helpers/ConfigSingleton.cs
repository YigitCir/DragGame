using UnityEngine;

namespace DefaultNamespace.Helpers
{
    public abstract class ConfigSingleton<T> : ScriptableObject where T : ConfigSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] assets = Resources.LoadAll<T>("");
                    //add error handling here
                    _instance = assets[0];
                }
                return _instance;
            }
        }
        
    }
}