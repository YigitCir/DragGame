using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Helpers;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.ServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        static ServiceLocator global;
        static Dictionary<Scene, ServiceLocator> sceneContainers;
        private static List<GameObject> tmpSceneGameObjects;
        
        private readonly ServiceManager services = new ServiceManager();

        private const string k_globalServiceLocatorName = "ServiceLocator [Global]";
        private const string k_sceneServiceLocatorName = "ServiceLocator [Scene]";

        internal void ConfigureAsGlobal(bool dontDestroyOnload)
        {
            if (global == this)
            {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Already configured as global",this);
            }else if (global != null)
            {
                Debug.LogError("ServiceLocator.ConfigureAsGlobal: Another ServiceLocator is already configured as global", this);
            }
            else
            {
                global = this;
                if(dontDestroyOnload) DontDestroyOnLoad(gameObject);
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;
            if (sceneContainers.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator.ConfigureForScene: Another ServiceLocator is already configured for this scene", this);
                return;
            }
            
            sceneContainers.Add(scene,this);
        }

        public ServiceLocator Register<T>(T service)
        {
            services.Register(service);
            return this;
        }

        public ServiceLocator Register(Type type, object service)
        {
            services.Register(type,service);
            Debug.Log($"ServiceLocator.Register: Registered service of type {type.FullName}");
            return this;
        }

        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service)) return this;

            if (TryGetNextInHierarchy(out ServiceLocator container))
            {
                container.Get<T>(out service);
                return this;
            }

            throw new ArgumentException($"ServiceLocator.Get: Service of type {typeof(T).FullName} not registered");
        }

        bool TryGetService<T>(out T service) where T : class
        {
            return services.TryGet(out service);
        }

        bool TryGetNextInHierarchy(out ServiceLocator container)
        {
            if (this == global)
            {
                container = null;
                return false;
            }

            container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(this);
            return container != null;
        }

        public static ServiceLocator Global
        {
            get
            {
                if (global != null) return global;

                var found = FindObjectOfType<ServiceLocatorSceneBootstrapper>();
                if (found)
                {
                    found.BootstrapOnDemand();
                    return global;
                }

                var container = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorSceneBootstrapper>().BootstrapOnDemand();
                return global;
            }
        }

        
        public static ServiceLocator For(MonoBehaviour mb)
        {
            return mb.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(mb) ?? Global;
        }

        public static ServiceLocator ForSceneOf(MonoBehaviour mb)
        {
            Scene scene = mb.gameObject.scene;

            if (sceneContainers.TryGetValue(scene, out ServiceLocator container) && container != mb)
            {
                return container;
            }
            
            tmpSceneGameObjects.Clear();
            scene.GetRootGameObjects(tmpSceneGameObjects);

            foreach (GameObject go in tmpSceneGameObjects.Where(go => go.GetComponent<ServiceLocatorSceneBootstrapper>() != null))
            {
                if (go.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper) &&
                    bootstrapper.Container != mb)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            return Global;
        }

        private void OnDestroy()
        {
            if (this == global)
            {
                global = null;
            }else if (sceneContainers.ContainsValue(this))
            {
                sceneContainers.Remove(gameObject.scene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            global = null;
            sceneContainers = new Dictionary<Scene, ServiceLocator>();
            tmpSceneGameObjects = new List<GameObject>();
        }

#if UNITY_EDITOR
        
        [MenuItem("GameObject/ServiceLocator/Add Global")]
        static void AddGlobal()
        {
            var go = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocatorGlobalBootstrapper));
        }
        
        [MenuItem("GameObject/ServiceLocator/Add Scene")]
        static void AddScene()
        {
            var go = new GameObject(k_sceneServiceLocatorName, typeof(ServiceLocatorSceneBootstrapper));
        }
#endif

    }
}