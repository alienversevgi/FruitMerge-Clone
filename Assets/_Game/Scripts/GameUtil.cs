using System.Collections.Generic;
using FruitMerge.Game;
using UnityEngine;

namespace FruitMerge.Util
{
    public static class GameUtil
    {
        public static bool IsInSizeRange(int size)
        {
            return size >= Const.MIN_ENTITY_LEVEL && size <= Const.MAX_ENTITY_LEVEL;
        }

        public static bool CheckLevelRange(int size)
        {
            bool isInSizeRange = IsInSizeRange(size);
            if (!isInSizeRange)
            {
                Debug.LogError($"{size} must be {Const.MIN_ENTITY_LEVEL}-{Const.MAX_ENTITY_LEVEL}");
            }

            return isInSizeRange;
        }
    }
}