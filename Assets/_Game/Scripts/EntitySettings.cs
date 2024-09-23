using System.Collections.Generic;
using FruitMerge.Util;
using UnityEngine;

namespace FruitMerge.Game
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntitySettings", order = 1)]
    public class EntitySettings : ScriptableObject
    {
        [SerializeField] private List<Sprite> views;
        [SerializeField] private List<Entity> prefabs;

        public Sprite GetSprite(int level)
        {
            if (!GameUtil.CheckLevelRange(level))
            {
                throw new UnityException("Is not range");
            }

            return views[level];
        } 
        
        public Entity GetPrefab(int level)
        {
            if (!GameUtil.CheckLevelRange(level))
            {
                throw new UnityException("Is not range");
            }

            return prefabs[level];
        }
        
        public Entity GetRandomPrefab()
        {
            int randomIndex = Random.Range(Const.MIN_ENTITY_LEVEL, Const.MAX_ENTITY_LEVEL-5);
            
            return GetPrefab(randomIndex);
        }
    }
}