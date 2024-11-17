using FruitMerge.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game.UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        
        [Inject] private SignalBus _signalBus;
        
        public void Initialize()
        {
            UpdateScoreText(0);
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