using BaseX;
using UnityEngine;

namespace FruitMerge.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject,IProtoData<ProtoPlayerData>
    {
        public int HighScore;
        public int CurrentScore;
        
        public ProtoPlayerData GetProtoData()
        {
            return new ProtoPlayerData()
            {
                HighScore = HighScore
            };
        }

        public void SetProtoData(ProtoPlayerData data)
        {
            HighScore = data.HighScore;
        }
    }
}