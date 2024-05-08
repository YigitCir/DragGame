using UnityEngine;

namespace DefaultNamespace.ServiceLocator
{
    public class ServiceLocatorGlobalBootstrapper : Bootstrapper
    {
        [SerializeField]
        private bool dontDestroyOnLoad;
        protected override void Bootstrap()
        {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
}