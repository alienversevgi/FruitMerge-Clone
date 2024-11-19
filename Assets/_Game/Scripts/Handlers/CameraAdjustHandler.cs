using System;
using FruitMerge.Events;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class CameraAdjustHandler : IInitializable, IDisposable
    {
        [Inject(Id = "Main")] private Camera _camera;
        [Inject] private SafeAreaHandler _safeAreaHandler;
        [Inject] private SignalBus _signalBus;
        
        public void Initialize()
        {
            ApplySafeArea(_safeAreaHandler.SafeAreaAnchor);
            _signalBus.Subscribe<GameSignals.OnSafeAreaChanged>(OnSafeAreaChanged);
        }

        private void OnSafeAreaChanged(GameSignals.OnSafeAreaChanged signalData)
        {
            ApplySafeArea(signalData.SafeAreaAnchor);
        }
        
        private void ApplySafeArea(SafeAreaAnchor anchor)
        {
            var minValue = anchor.Min.y;
            var height = anchor.Max.y - minValue;
            _camera.rect = new Rect(0, minValue, 1, height);   
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<GameSignals.OnSafeAreaChanged>(OnSafeAreaChanged);
        }
    }
}