using BaseX;
using UnityEngine;

namespace FruitMerge.Data
{
    [System.Serializable]
    public class EntitySaveData : IProtoData<ProtoEntitySaveData>
    {
        public int Level;
        public Vector2 Point;
        public float RotationZ;

        public ProtoEntitySaveData GetProtoData()
        {
            return new ProtoEntitySaveData()
            {
                Level = Level,
                PosX = Point.x,
                PosY = Point.y,
                RotationZ = RotationZ
            };
        }

        public void SetProtoData(ProtoEntitySaveData data)
        {
            Level = data.Level;
            Point = new Vector2(data.PosX, data.PosY);
            RotationZ = data.RotationZ;
        }
    }
}