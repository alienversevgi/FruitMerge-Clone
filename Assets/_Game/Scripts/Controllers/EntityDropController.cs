using FruitMerge.Events;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class EntityDropController : MonoBehaviour
    {
        [Inject] private EntityFactory _entityFactory;
        [Inject] private SignalBus _signalBus;
        [Inject] private NextQueueHandler _nextQueueHandler;
        [Inject] private EntitySettings _entitySettings;

        private Entity _currentEntity;
        private Vector3 _defaultPosition;
        private LineRenderer _lineRenderer;
        
        private static readonly int LineRendererColorName = Shader.PropertyToID("_Color");

        private bool _isEntityReady => _currentEntity is not null;

        public void Initialize()
        {
            _signalBus.Subscribe<GameSignals.OnDragging>(OnDragging);
            _signalBus.Subscribe<GameSignals.OnDraggingCompleted>(OnDraggingCompleted);
            _signalBus.Subscribe<GameSignals.OnQueueAnimationCompleted>(OnQueueUpdated);

            _defaultPosition = this.transform.position;
            _lineRenderer = this.GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
            Spawn(_nextQueueHandler.GetStarterLevel());
        }

        private void OnQueueUpdated(GameSignals.OnQueueAnimationCompleted signalData)
        {
            Spawn(9);
        }

        private void Spawn(in int level)
        {
            this.transform.position = _defaultPosition;
            _currentEntity = _entityFactory.SpawnEntity(level, _defaultPosition);
            _currentEntity.Initialize(false);
            _lineRenderer.material.SetColor(LineRendererColorName, _entitySettings.GetColor(_currentEntity.Level));
            _lineRenderer.enabled = true;
        }

        private void Release()
        {
            _signalBus.Fire(new GameSignals.OnEntityReleased()
            {
                EntityLevel = _currentEntity.Level
            });

            _currentEntity.SetActivePhysics(true);
            _currentEntity = null;
            _lineRenderer.enabled = false;
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
        }

        private void OnDragging(GameSignals.OnDragging eventData)
        {
            if (!_isEntityReady)
                return;

            Move(eventData.Position.x);
        }
    }
}