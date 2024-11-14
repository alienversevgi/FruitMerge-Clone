using System;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class Entity : MonoBehaviour
    {
        [Inject] private LevelController _levelController;

        [SerializeField]
        [Range(Const.MIN_ENTITY_LEVEL, Const.MAX_ENTITY_LEVEL - 1)]
        private int level;

        private Rigidbody2D _rigidBody;

        public int Level => level;

        public Vector2 ContactPoint { get; private set; }

        public void Initialize(bool isPhysicsActive)
        {
            _rigidBody = this.GetComponent<Rigidbody2D>();
            SetActivePhysics(isPhysicsActive);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var entity = collision.gameObject.GetComponent<Entity>();
            if (entity is null)
                return;

            ContactPoint = collision.contacts[0].point;
            _levelController.CheckMerge(this, entity);
        }

        public void SetActivePhysics(bool isActive)
        {
            _rigidBody.bodyType = isActive ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }
    }
}