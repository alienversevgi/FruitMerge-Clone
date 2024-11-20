using System.Collections.Generic;
using BaseX.Scripts;
using FruitMerge.Data;
using FruitMerge.Game;
using UnityEngine;

namespace FruitMerge.Managers
{
    public class DataManager : MonoBehaviour
    {
        #region Fields

        [field: SerializeField] public PlayerData PlayerData { get; set; }
        [field: SerializeField] public GameAreaData GameAreaData { get; set; }

        #endregion

        #region Unity Methods

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveAllData();
            }
        }

        private void OnApplicationQuit()
        {
            SaveAllData();
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            LoadAllData();
        }

        public void SaveHighScore(int score)
        {
            if (score > PlayerData.HighScore)
            {
                PlayerData.HighScore = score;
            }
        }

        public void SaveGameAreaData(List<Entity> entities, int dropperLevel, int score, int[] nextEntities)
        {
            GameAreaData = ScriptableObject.CreateInstance<GameAreaData>();
            GameAreaData.Score = score;
            GameAreaData.DropperLevel = dropperLevel;
            GameAreaData.NextEntities = nextEntities;
            GameAreaData.Entities = new List<EntitySaveData>();
            for (int i = 0; i < entities.Count; i++)
            {
                var currentTransform = entities[i].transform;
                GameAreaData.Entities.Add(new EntitySaveData()
                {
                    Level = entities[i].Level,
                    Point = currentTransform.position,
                    RotationZ = currentTransform.eulerAngles.z,
                });
            }

            DataHandler.SaveData(GameAreaData, nameof(GameAreaData));
        }

        #endregion

        #region Private Methods

        private void SaveAllData()
        {
            DataHandler.SaveData(PlayerData, nameof(PlayerData));
        }

        private void LoadAllData()
        {
            PlayerData = ScriptableObject.CreateInstance<PlayerData>();
            GameAreaData = ScriptableObject.CreateInstance<GameAreaData>();
            DataHandler.LoadData(PlayerData, nameof(PlayerData), ScriptableObject.CreateInstance<PlayerData>());
            DataHandler.LoadData(GameAreaData, nameof(GameAreaData), ScriptableObject.CreateInstance<GameAreaData>());
        }

        #endregion

        public void DeleteGameAreaData()
        {
            GameAreaData = ScriptableObject.CreateInstance<GameAreaData>();
            DataHandler.SaveData(GameAreaData, nameof(GameAreaData));
        }
    }
}