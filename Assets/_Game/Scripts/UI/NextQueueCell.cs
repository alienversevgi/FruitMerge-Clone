using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace FruitMerge.Game.UI
{
    public class NextQueueCell : MonoBehaviour
    {
        public NextQueueItem Item => _item;
        private NextQueueItem _item;

        public void Initialize(int level)
        {
            _item = this.transform.GetChild(0).GetComponent<NextQueueItem>();
            SetItemLevel(level);
        }

        public void SetItemLevel(int level)
        {
            _item.Initialize(level);
        }

        public async UniTask Shift(NextQueueCell targetCell)
        {
            var item = _item;
            targetCell.SetItem(_item);
            ClearItem();
            await item.transform.DOLocalMoveX(0, .2f);
        }

        public async Task<NextQueueItem> HideItem()
        {
            _item.transform.localScale = Vector3.one;
            await _item.transform.DOScale(Vector3.zero, .2f);
            return _item;
        }
        
        public async UniTask ShowItem()
        {
            _item.transform.localPosition = Vector3.zero;
            _item.transform.localScale = Vector3.zero;
            await _item.transform.DOScale(Vector3.one, .2f);
        }

        public void SetItem(NextQueueItem item)
        {
            _item = item;
            _item.transform.SetParent(this.transform);
        }

        public void ClearItem()
        {
            _item = null;
        }
    }
}