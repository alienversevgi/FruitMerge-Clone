using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FruitMerge.Game.UI
{
    public class NextQueueItem : MonoBehaviour
    {
        [SerializeField] private Image image;

        [Inject] private EntitySettings _entitySettings;

        public void Initialize(int level)
        {
            image.sprite = _entitySettings.GetSprite(level);
            image.SetNativeSize();
            image.rectTransform.sizeDelta *= .6f;
        }

        public async UniTask Hide()
        {
            await this.transform.DOScale(Vector3.zero, 1f);
        }
        
        public async UniTask Show()
        {
            await this.transform.DOScale(Vector3.zero, 1f);
        }

        public void SetEmpty()
        {
            image.enabled = false;
        }
    }
}