using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class LevelController : MonoBehaviour
    {
        [Inject] private EntityFactory _entityFactory;
        private void Merge(Entity from, Entity to)
        {
            Debug.Log($"Merge : {from.gameObject.name} - {to.gameObject.name}");
            int nextLevel = from.Level +1;
            Destroy(from.gameObject); 
            Destroy(to.gameObject);
            var entity = _entityFactory.SpawnEntity(nextLevel, from.ContactPoint);
            entity.Initialize(true);
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