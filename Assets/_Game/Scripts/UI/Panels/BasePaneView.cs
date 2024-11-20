using DG.Tweening;
using UnityEngine;

namespace FruitMerge.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BasePaneView : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        public virtual void Initialize()
        {
            _canvasGroup = this.GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(1, 0.5f);
        }

        public virtual void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(0, 0.5f);
        }
    }
}