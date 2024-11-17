using FruitMerge.Events;
using FruitMerge.Util;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class MergeController : MonoBehaviour
    {
        [SerializeField] private GameObject mergeEffectPrefab;

        [Inject] private EntityFactory _entityFactory;
        [Inject] private SignalBus _signalBus;

        private void Merge(Entity from, Entity to)
        {
            Debug.Log($"Merge : {from.gameObject.name} - {to.gameObject.name}");
            int currentLevel = from.Level;
            int nextLevel = currentLevel + 1;
            from.Dispose();
            to.Dispose();
 
            if (GameUtil.IsInSizeRange(nextLevel))
            {
                var entity = _entityFactory.SpawnEntity(nextLevel, from.ContactPoint);
                entity.Initialize(true);
                entity.PlayMergeAnimation();
            }

            Instantiate(mergeEffectPrefab, from.ContactPoint, Quaternion.identity);
            _signalBus.Fire(new GameSignals.OnMergeCompleted()
            {
                Level = currentLevel
            });
        }

        public void CheckMerge(Entity from, Entity to)
        {
            if (!IsMergeable(from, to) || !IsUniqueOperation(from, to))
                return;

            Merge(from, to);
        }

        private bool IsMergeable(Entity from, Entity to)
        {
            return from.Level == to.Level;
        }

        /// <summary>
        /// When you call the command to merge 2 objects, the merging process is performed twice.
        /// This method is used to prevent this.
        /// </summary>
        private bool IsUniqueOperation(Entity from, Entity to)
        {
            return from.gameObject.GetInstanceID() < to.gameObject.GetInstanceID();
        }
    }
}