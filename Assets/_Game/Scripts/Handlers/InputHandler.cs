using FruitMerge.Events;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        [Inject] private SignalBus _signalBus;

        private bool _isDragging;
        private Vector3 _dragPosition;
        
        private void Update()
        {
            HandleDrag();
        }

        private void HandleDrag()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
            }

            if (Input.GetMouseButton(0) && _isDragging)
            {
                Vector3 mousePosition = Input.mousePosition;

                _dragPosition =
                    camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane));
               
                _signalBus.Fire(new GameSignals.OnDragging()
                                {
                                    Position = _dragPosition
                                }
                );
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                
                _signalBus.Fire(new GameSignals.OnDraggingCompleted()
                                {
                                    Position = _dragPosition
                                }
                );
            }
        }
    }
}