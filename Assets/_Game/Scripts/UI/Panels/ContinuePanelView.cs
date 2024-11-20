using FruitMerge.Managers;
using UnityEngine.SceneManagement;
using Zenject;

namespace FruitMerge.UI
{
    public class ContinuePanelView : BasePaneView
    {
        [Inject] private DataManager _dataManager;
        
        public void OnRestartButtonClicked()
        {
            _dataManager.DeleteGameAreaData();
            SceneManager.LoadScene(0);
        }
        
        public void OnContinueButtonClicked()
        {
            Hide();
        }
    }
}