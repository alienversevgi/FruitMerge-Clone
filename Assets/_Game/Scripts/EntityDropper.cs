using FruitMerge.Events;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class EntityDropper : MonoBehaviour
    {
        [Inject] private EntityFactory _entityFactory;
        [Inject] private SignalBus _signalBus;

        private Entity _currentEntity;
        private Vector3 _defaultPosition;

        private bool _isEntityReady => _currentEntity is not null;

        public void Initialize()
        {
            _signalBus.Subscribe<GameEvents.OnDragging>(OnDragging);
            _signalBus.Subscribe<GameEvents.OnDraggingCompleted>(OnDraggingCompleted);

            _defaultPosition = this.transform.position;
            Spawn();
        }

        private void Spawn()
        {
            var spawnPoint = this.transform.position;
            _currentEntity = _entityFactory.SpawnRandomEntity(spawnPoint);
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
        }

        private void OnDraggingCompleted(GameEvents.OnDraggingCompleted eventData)
        {
            if (!_isEntityReady)
                return;

            Release();
            Spawn();
        }

        private void OnDragging(GameEvents.OnDragging eventData)
        {
            if (!_isEntityReady)
                return;
            
            Move(eventData.Position.x);
        }
    }
}