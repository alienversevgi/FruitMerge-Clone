using UnityEngine;

namespace FruitMerge.Events
{
    public static class GameEvents
    {
        public struct OnDragging
        {
            public Vector3 Position { get; set; }
        }
        
        public struct OnDraggingCompleted
        {
            public Vector3 Position { get; set; }
        }
    }
}