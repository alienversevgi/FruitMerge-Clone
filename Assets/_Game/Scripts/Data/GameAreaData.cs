using System.Collections.Generic;
using BaseX;
using Google.Protobuf.Collections;
using UnityEngine;

namespace FruitMerge.Data
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "Data/SaveData", order = 1)]
    public class GameAreaData : ScriptableObject, IProtoData<ProtoGameAreaData>
    {
        public int Score;
        public int DropperLevel;
        public List<EntitySaveData> Entities = new List<EntitySaveData>();
        public int[] NextEntities = new int[3];

        public ProtoGameAreaData GetProtoData()
        {
            RepeatedField<ProtoEntitySaveData> entities = new RepeatedField<ProtoEntitySaveData>();
            for (int i = 0; i < Entities.Count; i++)
            {
                entities.Add(Entities[i].GetProtoData());
            }

            return new ProtoGameAreaData()
            {
                Score = Score,
                DropperLevel = DropperLevel,
                Entities = { entities },
                NextEntities = { NextEntities }
            };
        }

        public void SetProtoData(ProtoGameAreaData data)
        {
            Score = data.Score ;
            DropperLevel = data.DropperLevel;

            for (int i = 0; i < data.Entities.Count; i++)
            {
                EntitySaveData entitySaveData = new EntitySaveData();
                entitySaveData.SetProtoData(data.Entities[i]);
                Entities.Add(entitySaveData);
            }

            for (int i = 0; i < data.NextEntities.Count; i++)
            {
                NextEntities[i] = data.NextEntities[i];
            }
        }
    }
}