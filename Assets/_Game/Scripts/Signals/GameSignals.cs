using System.Collections.Generic;
using FruitMerge.Game;
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

        public struct OnEntityReleased
        {
            public int EntityLevel { get; set; }
        }

        public struct OnNextQueueUpdated
        {
            public int AddedLevel { get; set; }
        }

        public struct OnQueueAnimationCompleted
        {
        }

        public struct OnMergeCompleted
        {
            public int Level { get; set; }
        }

        public struct OnScoreGained
        {
            public int Score { get; set; }
        }

        public struct SafeAreaChanged
        {
            public SafeAreaAnchor SafeAreaAnchor { get; set; }
        }
    }
}