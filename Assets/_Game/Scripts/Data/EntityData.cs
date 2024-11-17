using FruitMerge.Game;
using UnityEngine;

namespace _Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Data")]
    public class EntityData : ScriptableObject
    {
        [Range(0,10)]
        public int Level;
        public Sprite Sprite;
        public Color Color;
        public Entity Prefab;
        public int Score;
    }
}