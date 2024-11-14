using FruitMerge.Events;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class EntityDropController : MonoBehaviour
    {
        [Inject] private EntityFactory _entityFactory;
        [Inject] private SignalBus _signalBus;

        private Entity _currentEntity;
        private Vector3 _defaultPosition;

        private bool _isEntityReady => _currentEntity is not null;

        public void Initialize()
        {
            _signalBus.Subscribe<GameSignals.OnDragging>(OnDragging);
            _signalBus.Subscribe<GameSignals.OnDraggingCompleted>(OnDraggingCompleted);

            _defaultPosition = this.transform.position;
            Spawn();
        }

        private void Spawn()
        {
            this.transform.position = _defaultPosition;
            _currentEntity = _entityFactory.SpawnRandomEntity(_defaultPosition);
            _currentEntity.Initialize(false);
        }

        private void Release()
        {
            _currentEntity.SetActivePhysics(true);
            _currentEntity = null;
        }

        private void Move(float x)
        {
            var newPosition = _defaultPosition;
            newPosition.x = x;

            _currentEntity.transform.position = newPosition;
            this.transform.position = newPosition;
        }

        private void OnDraggingCompleted(GameSignals.OnDraggingCompleted eventData)
        {
            if (!_isEntityReady)
                return;

            Release();
            Spawn();
        }

        private void OnDragging(GameSignals.OnDragging eventData)
        {
            if (!_isEntityReady)
                return;
            
            Move(eventData.Position.x);
        }
    }
}