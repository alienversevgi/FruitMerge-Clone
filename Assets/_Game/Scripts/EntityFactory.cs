using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class EntityFactory : MonoBehaviour
    {
        [SerializeField] private Transform defaultContainer;

        [Inject] private EntitySettings _entitySettings;
        [Inject] private DiContainer _diContainer;

        public Entity SpawnRandomEntity(Vector3 position)
        {
            var prefab = _entitySettings.GetRandomPrefab();
            var entity = InstantiateEntity(prefab, position, defaultContainer);

            return entity;
        }

        public Entity SpawnEntity(int level, Vector3 position)
        {
            var prefab = _entitySettings.GetPrefab(level);
            var entity = InstantiateEntity(prefab, position, defaultContainer);

            return entity;
        }

        private Entity InstantiateEntity
        (
            Entity prefab,
            Vector3 position,
            Transform parent
        )
        {
            var entity = _diContainer.InstantiatePrefabForComponent<Entity>(prefab, position, quaternion.identity, parent);
            return entity;
        }
    }
}