using FruitMerge.Managers;
using UnityEngine.SceneManagement;
using Zenject;

namespace FruitMerge.UI
{
    public class RestartPanelView : BasePaneView
    {
        [Inject] private DataManager _dataManager;
        
        public void OnYesButtonClicked()
        {
            _dataManager.DeleteGameAreaData();
            SceneManager.LoadScene(0);
        }
        
        public void OnNoButtonClicked()
        {
            Hide();
        }
    }
}