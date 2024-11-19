using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FruitMerge.Events;
using UnityEngine;
using UnityEngine.Animations;
using Zenject;

namespace FruitMerge.Game
{
    public class EndLineController : MonoBehaviour
    {
        [SerializeField] private ParentConstraint effectParent;

        [Inject] private SignalBus _signalBus;
        
        private Entity _triggeredEntity;
        private CancellationTokenSource _cancellationTokenSource;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_triggeredEntity != null)
                return;

            _triggeredEntity = other.gameObject.GetComponent<Entity>();
            if (!_triggeredEntity.IsReadyToEndLine)
            {
                _triggeredEntity = null;
                return;
            }

            RunTimer().Forget();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            StopTimer();
        }

        private async UniTask RunTimer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            await UniTask.Delay(TimeSpan.FromSeconds(.2f), cancelImmediately: true,
                cancellationToken: _cancellationTokenSource.Token);
            effectParent.transform.position = _triggeredEntity.transform.position;
            effectParent.SetSource(0, new ConstraintSource()
            {
                sourceTransform = _triggeredEntity.transform,
                weight = 1
            });
            effectParent.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancelImmediately: true,
                cancellationToken: _cancellationTokenSource.Token);

            _signalBus.Fire(new GameSignals.OnGameOver());
        }

        private void StopTimer()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = null;
            }
            effectParent.gameObject.SetActive(false);
            _triggeredEntity = null;
        }
    }
}