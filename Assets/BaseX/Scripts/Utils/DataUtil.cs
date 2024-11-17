using System.IO;
using BaseX.Scripts;
using UnityEngine;

namespace BaseX.Utils
{
    public static class DataUtil 
    {
        public static void ClearAllData()
        {
            if (Directory.Exists(DataHandler.GeneralPath))
            {
                Directory.Delete(DataHandler.GeneralPath,true);
            }
            
            Debug.Log("Data deleted successfully!");
        }
    }
}