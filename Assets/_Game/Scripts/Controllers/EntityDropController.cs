using FruitMerge.Events;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class EntityDropController : MonoBehaviour
    {
        [SerializeField] private Renderer line;

        [Inject] private EntityFactory _entityFactory;
        [Inject] private SignalBus _signalBus;
        [Inject] private NextQueueHandler _nextQueueHandler;
        [Inject] private EntitySettings _entitySettings;

        private Entity _current;
        private Vector3 _defaultPosition;
        private float _validBound;
        private ContactFilter2D _lineFilter;
        private RaycastHit2D[] _lineHit;
        private MaterialPropertyBlock _lineMPB;

        private static readonly int LineRendererColorName = Shader.PropertyToID("_Color");
        private static readonly int ShadingValue = Shader.PropertyToID("_ShadingValue");

        private bool _isEntityReady => _current is not null;

        public void Initialize()
        {
            _signalBus.Subscribe<GameSignals.OnDragging>(OnDragging);
            _signalBus.Subscribe<GameSignals.OnDraggingCompleted>(OnDraggingCompleted);
            _signalBus.Subscribe<GameSignals.OnQueueAnimationCompleted>(OnQueueUpdated);

            _defaultPosition = this.transform.position;
            _lineMPB = new MaterialPropertyBlock();
            line.GetPropertyBlock(_lineMPB);
            line.enabled = false;

            _lineHit = new RaycastHit2D[1];
            _lineFilter = new ContactFilter2D()
            {
                layerMask = LayerMask.GetMask("Ground", "Entity"),
            };

            Spawn(_nextQueueHandler.StarterLevel);
        }

        private void OnQueueUpdated(GameSignals.OnQueueAnimationCompleted signalData)
        {
            Spawn(_nextQueueHandler.CurrentLevel);
        }

        private void Spawn(in int level)
        {
            this.transform.position = _defaultPosition;
            _current = _entityFactory.SpawnEntity(level, _defaultPosition);
            _current.Initialize(false);
            _validBound = _entitySettings.GetValidBound(_current.Level);
            _lineMPB.SetColor(LineRendererColorName, _entitySettings.GetColor(_current.Level));
            SetLineEndPoint();
            line.enabled = true;
        }

        private void Release()
        {
            _signalBus.Fire(new GameSignals.OnEntityReleased()
            {
                EntityLevel = _current.Level
            });

            _signalBus.Fire(new GameSignals.OnEntityAdded()
            {
                Entity = _current
            });

            _current.SetActivePhysics(true);
            _current = null;
            line.enabled = false;
        }

        private void Move(float x)
        {
            var newPosition = _defaultPosition;
            newPosition.x = Mathf.Clamp(x, _validBound * -1, _validBound);

            this.transform.position = newPosition;
            _current.transform.position = this.transform.position;
        }

        private void SetLineEndPoint()
        {
            Physics2D.Raycast(this.transform.position, Vector2.down, _lineFilter, _lineHit);
            if (_lineHit[0].collider is not null)
            {
                float shadingValue = _lineHit[0].distance / 16.8f;
                shadingValue = 1 + (1 - shadingValue);
                _lineMPB.SetFloat(ShadingValue, shadingValue);
                line.SetPropertyBlock(_lineMPB);
            }
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
            SetLineEndPoint();
        }
    }
}