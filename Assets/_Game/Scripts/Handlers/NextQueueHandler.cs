using System;
using System.Collections.Generic;
using FruitMerge.Events;
using Zenject;
using Random = UnityEngine.Random;

namespace FruitMerge.Game
{
    public class NextQueueHandler : IDisposable
    {
        public int[] GetQueue() => _queue.ToArray();
        public int CurrentLevel { get; private set; }

        [Inject] private SignalBus _signalBus;

        private Queue<int> _queue;
        private Queue<int> _tempQueue;

        public void Initialize()
        {
            _tempQueue = new Queue<int>(Const.QUEUE_COUNT);
            _queue = new Queue<int>(Const.QUEUE_COUNT);

            for (int i = 0; i < Const.QUEUE_COUNT; i++)
            {
                _queue.Enqueue(Random.Range(Const.MIN_ENTITY_LEVEL, Const.MAX_SPAWN_ENTITY_LEVEL));
            }

            for (int i = 0; i < Const.QUEUE_TEMP_COUNT; i++)
            {
                _tempQueue.Enqueue(Random.Range(Const.MIN_ENTITY_LEVEL, Const.MAX_SPAWN_ENTITY_LEVEL));
            }

            _signalBus.Subscribe<GameSignals.OnEntityReleased>(OnEntityReleased);
        }

        public int GetStarterLevel()
        {
            return Random.Range(Const.MIN_ENTITY_LEVEL, Const.MAX_SPAWN_STARTER_ENTITY_LEVEL);
        }

        private void UpdateQueue()
        {
            CurrentLevel = _queue.Dequeue();
            _tempQueue.Enqueue(CurrentLevel);
            var addedLevel = _tempQueue.Dequeue();
            _queue.Enqueue(addedLevel);

            _signalBus.Fire(new GameSignals.OnNextQueueUpdated()
            {
                AddedLevel = addedLevel
            });
        }

        private void OnEntityReleased(GameSignals.OnEntityReleased signalData)
        {
            UpdateQueue();
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<GameSignals.OnEntityReleased>(OnEntityReleased);
        }
    }
}