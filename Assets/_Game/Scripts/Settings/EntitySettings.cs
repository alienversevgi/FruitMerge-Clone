using System.Collections.Generic;
using _Game.Scripts.Data;
using _Game.Scripts.Misc;
using UnityEngine;

namespace FruitMerge.Game
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntitySettings", order = 1)]
    public class EntitySettings : ScriptableObject
    {
        [SerializeField] private List<EntityData> entities;

        public Sprite GetSprite(int level) => entities[level].Sprite;
        public Entity GetPrefab(int level) => entities[level].Prefab;
        public int GetScore(int level) => entities[level].Score;
        public Color GetColor(int level) => entities[level].Color;
        
        public float GetValidBound(int level) => entities[level].ValidBound;
    }
}