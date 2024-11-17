using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FruitMerge.Game
{
    public class EndLineController : MonoBehaviour
    {
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
            
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancelImmediately: true,
                cancellationToken: _cancellationTokenSource.Token);
            
            Debug.LogError("GameOver");
        }

        private void StopTimer()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = null;
            }
            
            _triggeredEntity = null;
        }
    }
}