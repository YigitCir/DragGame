using DefaultNamespace.Helpers;
using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(menuName = "Config/DragConfig")]
    public class DragConfig : ConfigSingleton<DragConfig>
    {
        public float DragDownMultiplier = 5f;
        public float CloseEnoughRange = 3f;
        public float BaseDragSpeed = 5f;
        public float BaseDragCooldown = 0.15f;
        public float BaseExplosionForce = 10f;
        public float BaseExplosionRadius = 5f;

        public float BaseTargetXSpeed;

        public float YSpeedToDamageMultiplier = 1f;
        public float XSpeedToDamage = 0.25f;

    }
}