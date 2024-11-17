namespace BaseX.Utils
{
    public static class CollectionUtil
    {
        public static bool IsInRange(this System.Collections.ICollection collection, int index)
        {
            return index >= 0 && index < collection.Count;
        }
    }
}