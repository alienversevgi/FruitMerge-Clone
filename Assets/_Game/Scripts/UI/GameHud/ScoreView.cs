using FruitMerge.Events;
using FruitMerge.Managers;
using TMPro;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game.UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        
        [Inject] private SignalBus _signalBus;
        [Inject] private DataManager _dataManager;
        
        public void Initialize()
        {
            highScoreText.text = _dataManager.PlayerData.HighScore.ToString();
            UpdateScoreText(_dataManager.PlayerData.CurrentScore);
            _signalBus.Subscribe<GameSignals.OnScoreGained>(OnScoreGained);
        }

        private void OnScoreGained(GameSignals.OnScoreGained signalData)
        {
            UpdateScoreText(signalData.Score);
        }

        private void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}