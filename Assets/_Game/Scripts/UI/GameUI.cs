using FruitMerge.UI;
using UnityEngine;

namespace FruitMerge.Game.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private NextQueueIndicatorView nextQueueIndicator;
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private GameOverView gameOverView;
        
        public void Initialize()
        {
            nextQueueIndicator.Initialize();
            scoreView.Initialize();
        }

        public void ShowGameOverPanel()
        {
            gameOverView.Show();
        }
    }
}