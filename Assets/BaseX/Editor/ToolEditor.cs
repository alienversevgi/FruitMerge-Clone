using BaseX.Utils;
using UnityEditor;

namespace BaseX.Editor
{
    public static class ToolEditor
    {
        [MenuItem("Game Tool/Clear All Data", false, 99)]
        public static void ClearData()
        {
            if (EditorUtility.DisplayDialog("Clear All Data", "All data will be deleted. Are you sure?", "Yes", "No"))
            {
                DataUtil.ClearAllData();
            }
        }
    }
}