using UnityEngine;

namespace DefaultNamespace
{
    public static class ObjectDragger
    {
        public static Draggable CurrentDraggedObject { get; private set; }
        public static Draggable LastDraggedObject { get; private set; }
        public static float LastDraggedTime;
        public static float baseCooldown = 1f;

        public static void SetDraggedObject(Draggable d)
        {
            if (LastDraggedObject == d && LastDraggedTime + baseCooldown > Time.time)
            {
                
                return;
            }

            if (d.State == DragState.Fall)
            {
                return;
            }
            LastDraggedTime = Time.time;
            LastDraggedObject = d;
            CurrentDraggedObject = d;
            CurrentDraggedObject.SetState(DragState.Dragging);
        }

        public static void AddForceTowards(Vector3 targetPos)
        {
            if (CurrentDraggedObject == null) return;
            CurrentDraggedObject.SetState(DragState.Dragging);
            CurrentDraggedObject.MoveTowards(targetPos);
            
        }

        public static void Free()
        {
            if (CurrentDraggedObject == null) return;
            CurrentDraggedObject.LetGo();
            CurrentDraggedObject = null;
        }
        
    }
}