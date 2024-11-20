using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FruitMerge.UI
{
    public class GameOverPanelView : BasePaneView
    {
        public void OnRetryButtonClicked()
        {
            SceneManager.LoadScene(0);
        }
    }
}