using System;
using System.Collections.Generic;
using FruitMerge.Events;
using FruitMerge.Game.UI;
using FruitMerge.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class GameManager : MonoBehaviour, IDisposable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private EntityDropController _entityDropController;
        [Inject] private MergeHandler _mergeHandler;
        [Inject] private EntitySettings _entitySettings;
        [Inject] private NextQueueHandler _nextQueueHandler;
        [Inject] private InputHandler _inputHandler;
        [Inject] private GameUI _gameUI;
        [Inject] private DataManager _dataManager;
        [Inject] private EntityFactory _entityFactory;

        private int Score { get; set; }
        public List<Entity> Entities { get; private set; }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Entities = new List<Entity>();
            _dataManager.Initialize();

            if (_dataManager.GameAreaData.Entities.Count > 0)
                LoadGameAreaData();
            else
                StartNewGame();
        }

        private void LoadGameAreaData()
        {
            var gameAreaData = _dataManager.GameAreaData;
            Score = gameAreaData.Score;
            _dataManager.PlayerData.CurrentScore = Score;

            for (int i = 0; i < gameAreaData.Entities.Count; i++)
            {
                var entity = _entityFactory.SpawnEntity(gameAreaData.Entities[i].Level, gameAreaData.Entities[i].Point,
                    gameAreaData.Entities[i].RotationZ);
                Entities.Add(entity);
            }

            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Initialize(true);
            }

            _nextQueueHandler.SetData(gameAreaData.DropperLevel,gameAreaData.NextEntities);
            _nextQueueHandler.Initialize();
            _entityDropController.Initialize();
            _gameUI.Initialize();

            SubscribeEvents();
        }

        private void StartNewGame()
        {
            _nextQueueHandler.Initialize();
            _entityDropController.Initialize();
            _gameUI.Initialize();

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _signalBus.Subscribe<GameSignals.OnMergeCompleted>(OnMergeCompleted);
            _signalBus.Subscribe<GameSignals.OnGameOver>(GameOver);
            _signalBus.Subscribe<GameSignals.OnEntityAdded>(OnEntityAdded);
            _signalBus.Subscribe<GameSignals.OnEntityRemoved>(OnEntityRemoved);
        }

        private void GameOver()
        {
            _dataManager.SaveHighScore(Score);
            _gameUI.ShowGameOverPanel();
        }

        private void OnEntityRemoved(GameSignals.OnEntityRemoved signalData)
        {
            if (Entities.Contains(signalData.Entity))
            {
                Entities.Remove(signalData.Entity);
            }
        }

        private void OnEntityAdded(GameSignals.OnEntityAdded signalData)
        {
            if (!Entities.Contains(signalData.Entity))
            {
                Entities.Add(signalData.Entity);
            }
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

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveGameAreaData();
            }
        }

        private void OnApplicationQuit()
        {
            SaveGameAreaData();
        }

        [Button]
        private void SaveGameAreaData()
        {
            _dataManager.SaveGameAreaData(Entities, _nextQueueHandler.CurrentLevel, Score, _nextQueueHandler.GetQueue());
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<GameSignals.OnMergeCompleted>(OnMergeCompleted);
            _signalBus.TryUnsubscribe<GameSignals.OnGameOver>(GameOver);
            _signalBus.TryUnsubscribe<GameSignals.OnEntityAdded>(OnEntityAdded);
            _signalBus.TryUnsubscribe<GameSignals.OnEntityRemoved>(OnEntityRemoved);
        }
    }
}