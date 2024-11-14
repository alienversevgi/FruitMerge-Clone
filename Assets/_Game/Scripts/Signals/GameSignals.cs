using UnityEngine;

namespace FruitMerge.Events
{
    public static class GameSignals
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