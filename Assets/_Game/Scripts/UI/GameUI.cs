using UnityEngine;

namespace FruitMerge.Game.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private NextQueueIndicatorView nextQueueIndicator;
        [SerializeField] private ScoreView scoreView;

        public void Initialize()
        {
            nextQueueIndicator.Initialize();
            scoreView.Initialize();
        }
    }
}