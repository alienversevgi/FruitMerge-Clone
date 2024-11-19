using BaseX.Scripts;
using BaseX.Utils;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

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
        
        [MenuItem("Game Tool/Copy Data Path", false, 98)]
        public static void CopyDataPath()
        {
            Clipboard.Copy(DataHandler.GeneralPath);
            Debug.Log("Successfully copied data folder");
        }
    }
}