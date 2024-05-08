using System;

namespace DefaultNamespace
{
    public static class CastleEvents
    {
        public static event Action OnCastleDamageTaken;
        
        
        public static void InvokeCastleDamageTakenEvent()
        {
            OnCastleDamageTaken?.Invoke();
        }
    }
}