using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using FruitMerge.Events;
using FruitMerge.Game.UI;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace FruitMerge.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MergeController mergeController;

        [Inject] private SignalBus _signalBus;
        [Inject] private EntityDropController _entityDropController;
        [Inject] private EntitySettings _entitySettings;
        [Inject] private NextQueueHandler _nextQueueHandler;
        [Inject] private InputHandler _inputHandler;
        [Inject] private GameUI _gameUI;

        private int Score { get; set; }

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            StartGame();
        }

        public void StartGame()
        {
            _nextQueueHandler.Initialize();
            _entityDropController.Initialize();
            _gameUI.Initialize();
            _signalBus.Subscribe<GameSignals.OnMergeCompleted>(OnMergeCompleted);
        }

        private void OnMergeCompleted(GameSignals.OnMergeCompleted signalData)
        {
            int score = _entitySettings.GetScore(signalData.Level);
            Score += score;

            _signalBus.Fire(new GameSignals.OnScoreGained()
            {
                Score = Score
            });
        }

        public void GameOver()
        {
        }
    }
}