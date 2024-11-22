using FruitMerge.Events;
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

        public Entity Current => _currentCurrent;

        private Entity _currentCurrent;
        private Vector3 _defaultPosition;
        private float _validBound;
        private LineRenderer _lineRenderer;

        private static readonly int LineRendererColorName = Shader.PropertyToID("_Color");

        private bool _isEntityReady => _currentCurrent is not null;

        public void Initialize()
        {
            _signalBus.Subscribe<GameSignals.OnDragging>(OnDragging);
            _signalBus.Subscribe<GameSignals.OnDraggingCompleted>(OnDraggingCompleted);
            _signalBus.Subscribe<GameSignals.OnQueueAnimationCompleted>(OnQueueUpdated);

            _defaultPosition = this.transform.position;
            _lineRenderer = this.GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
            Spawn(_nextQueueHandler.StarterLevel);
        }

        private void OnQueueUpdated(GameSignals.OnQueueAnimationCompleted signalData)
        {
            Spawn(_nextQueueHandler.CurrentLevel);
        }

        private void Spawn(in int level)
        {
            this.transform.position = _defaultPosition;
            _currentCurrent = _entityFactory.SpawnEntity(level, _defaultPosition);
            _currentCurrent.Initialize(false);
            _validBound = _entitySettings.GetValidBound(_currentCurrent.Level);
            _lineRenderer.material.SetColor(LineRendererColorName, _entitySettings.GetColor(_currentCurrent.Level));
            _lineRenderer.enabled = true;
        }

        private void Release()
        {
            _signalBus.Fire(new GameSignals.OnEntityReleased()
            {
                EntityLevel = _currentCurrent.Level
            });

            _signalBus.Fire(new GameSignals.OnEntityAdded()
            {
                Entity = _currentCurrent
            });

            _currentCurrent.SetActivePhysics(true);
            _currentCurrent = null;
            _lineRenderer.enabled = false;
        }

        private void Move(float x)
        {
            var newPosition = _defaultPosition;
            newPosition.x = Mathf.Clamp(x, _validBound * -1, _validBound);

            this.transform.position = newPosition;
            _currentCurrent.transform.position = this.transform.position;
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