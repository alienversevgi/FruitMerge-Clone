using System;
using FruitMerge.Game;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Misc
{
    public class MergeEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem coloredParticle;

        [Inject] private EntitySettings _entitySettings;

        private bool _isStarted;
        private IMemoryPool _pool;
        private void SetPool(IMemoryPool pool) => _pool = pool;

        public void Initialize(int level, Vector3 point)
        {
            _isStarted = false;
            this.transform.position = point;
            var main = coloredParticle.main;
            main.startColor = _entitySettings.GetColor(level);
        }

        private void OnDisable()
        {
            if (!_isStarted)
                return;
            
            _isStarted = false;
            _pool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<MergeEffect>
        {
            protected override void OnCreated(MergeEffect item)
            {
                item.gameObject.name += $"_{NumTotal}";
                item.SetPool(this);
                base.OnCreated(item);
            }
        }
    }
}