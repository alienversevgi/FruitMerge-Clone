using System;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelController levelController;

        [Inject] private EntityDropController _entityDropController;
        [Inject] private InputHandler _inputHandler;

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
            _entityDropController.Initialize();
        }

        public void GameOver()
        {
        }
    }
}