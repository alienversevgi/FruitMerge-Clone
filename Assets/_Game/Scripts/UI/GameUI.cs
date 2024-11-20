using System;
using System.Collections.Generic;
using FruitMerge.UI;
using UnityEngine;

namespace FruitMerge.Game.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private NextQueueIndicatorView nextQueueIndicator;
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private Transform panelContainer;

        private Dictionary<Type, BasePaneView> _panels;

        public void Initialize()
        {
            _panels = new Dictionary<Type, BasePaneView>();
            var panels = panelContainer.GetComponentsInChildren<BasePaneView>();
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].Initialize();
                _panels.Add(panels[i].GetType(), panels[i]);
            }

            nextQueueIndicator.Initialize();
            scoreView.Initialize();
        }

        public void ShowPanel<T>() where T : BasePaneView
        {
            _panels[typeof(T)].Show();
        }

        public void OnRestartButtonClicked()
        {
            ShowPanel<RestartPanelView>();
        }
    }
}