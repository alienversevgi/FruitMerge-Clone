using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FruitMerge.Events;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game.UI
{
    public class NextQueueIndicatorView : MonoBehaviour, IDisposable
    {
        private NextQueueCell[] _cells;

        [Inject] private NextQueueHandler _nextQueueHandler;
        [Inject] private SignalBus _signalBus;

        public void Initialize()
        {
            _cells = this.transform.GetComponentsInChildren<NextQueueCell>();
            InitializeView();
            _signalBus.Subscribe<GameSignals.OnNextQueueUpdated>(OnNextQueueUpdated);
        }

        private void InitializeView()
        {
            var queue = _nextQueueHandler.GetQueue();
            for (int i = 0; i < queue.Length; i++)
            {
                _cells[i].Initialize(queue[i]);
            }
        }

        private async UniTask UpdateItems(int addedLevel)
        {
            var item = await _cells[0].HideItem();
            
            _cells[1].Shift(_cells[0]).Forget();
            await UniTask.Delay(10);
            await _cells[2].Shift(_cells[1]);
            
            _signalBus.Fire(new GameSignals.OnQueueAnimationCompleted());
            
            _cells[2].SetItem(item);
            _cells[2].SetItemLevel(addedLevel);
            await _cells[2].ShowItem();
        }

        private void OnNextQueueUpdated(GameSignals.OnNextQueueUpdated signalData)
        {
            UpdateItems(signalData.AddedLevel).Forget();
            // signalData.AddedLevel;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<GameSignals.OnNextQueueUpdated>(OnNextQueueUpdated);
        }
    }
}