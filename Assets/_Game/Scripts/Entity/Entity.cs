using System;
using DG.Tweening;
using FruitMerge.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class Entity : MonoBehaviour,IDisposable
    {
        [Inject] private MergeHandler _mergeHandler;
        [Inject] private SignalBus _signalBus;

        [SerializeField] [Range(Const.MIN_ENTITY_LEVEL, Const.MAX_ENTITY_LEVEL)]
        private int level;

        private Vector3 _defaultScale;
        private Rigidbody2D _rigidBody;
        private CircleCollider2D _circleCollider;
        public int Level => level;
        public bool IsReadyToEndLine { get; private set; }
        public Vector2 ContactPoint { get; private set; }

        public void Initialize(bool isPhysicsActive)
        {
            _rigidBody = this.GetComponent<Rigidbody2D>();
            _circleCollider = this.GetComponent<CircleCollider2D>();
            SetActivePhysics(isPhysicsActive);
            IsReadyToEndLine = false;
            _defaultScale = this.transform.localScale;
        }

        public void PlayMergeAnimation()
        {
            this.transform.localScale = Vector3.zero;
            this.transform.DOScale(_defaultScale, .5f).SetEase(Ease.OutBounce);
            _rigidBody.DOJump(this.transform.position + new Vector3(0, .5f, 0), 1, 1, .2f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IsReadyToEndLine = true;
            var entity = collision.gameObject.GetComponent<Entity>();
            if (entity is null)
                return;

            ContactPoint = collision.contacts[0].point;
            _mergeHandler.CheckMerge(this, entity);
        }

        public void SetActivePhysics(bool isActive)
        {
            _circleCollider.enabled = isActive;
            _rigidBody.bodyType = isActive ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }

        [Button]
        public void Scale(float scale)
        {
            this.transform.localScale += this.transform.localScale * scale;
        }

        public void Dispose()
        {
            _rigidBody.DOKill();
            this.transform.DOKill();
            _signalBus.Fire(new GameSignals.OnEntityRemoved()
            {
                Entity = this
            });
            Destroy(this.gameObject);
        }
    }
}