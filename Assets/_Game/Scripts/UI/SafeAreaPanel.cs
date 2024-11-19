using System;
using FruitMerge.Events;
using FruitMerge.Game;
using UnityEngine;
using Zenject;

namespace FruitMerge.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaPanel : MonoBehaviour, IDisposable
    {
        [Inject] private SafeAreaHandler _safeAreaHandler;
        [Inject] private SignalBus _signalBus;
        
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _safeAreaHandler.ApplySafeArea(_rectTransform);
            _signalBus.Subscribe<GameSignals.OnSafeAreaChanged>(OnSafeAreaChanged);
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void OnSafeAreaChanged()
        {
            _safeAreaHandler.ApplySafeArea(_rectTransform);
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<GameSignals.OnSafeAreaChanged>(OnSafeAreaChanged);
        }
    }
}