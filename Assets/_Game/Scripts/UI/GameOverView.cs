using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FruitMerge.UI
{
    public class GameOverView : MonoBehaviour
    {
        public void Show()
        {
            this.transform.DOScale(Vector3.one, .5f).SetEase(Ease.Flash);
            this.gameObject.SetActive(true);
        }

        public void OnRetryButtonClicked()
        {
            SceneManager.LoadScene(0);
        }
    }
}