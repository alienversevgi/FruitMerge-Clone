using _Game.Scripts.Misc;
using UnityEngine;

namespace FruitMerge.Game
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PrefabSettings", order = 1)]
    public class PrefabSettings : ScriptableObject
    {
        public MergeEffect MergeEffect;
    }
}