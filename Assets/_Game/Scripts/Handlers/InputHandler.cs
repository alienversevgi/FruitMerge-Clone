using System.Collections.Generic;
using FruitMerge.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace FruitMerge.Game
{
    public class InputHandler : MonoBehaviour
    {
        [Inject(Id = "Main")] private Camera _camera;
        [Inject] private SignalBus _signalBus;
        [Inject] private GraphicRaycaster _graphicRaycaster;
        [Inject] private EventSystem _eventSystem;

        private bool _isDragging;
        private Vector3 _dragPosition;
        private bool _isActive;
        private Vector3 _mousePosition;
        private List<RaycastResult> _graphicRaycastResults;
        private LayerMask _uiMask;

        public void Initialize()
        {
            _uiMask = LayerMask.GetMask("IgnoreInput");
            _graphicRaycastResults = new List<RaycastResult>();
            SetActive(true);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetActive(!DetectUIElement());
            }

            if (!_isActive)
                return;

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
                _mousePosition = Input.mousePosition;

                _dragPosition =
                    _camera.ScreenToWorldPoint(new Vector3(_mousePosition.x, _mousePosition.y, _camera.nearClipPlane));

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

        private void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        private bool DetectUIElement()
        {
            _graphicRaycastResults.Clear();

            PointerEventData pointerEventData = new PointerEventData(_eventSystem)
            {
                position = Input.mousePosition
            };

            _graphicRaycaster.Raycast(pointerEventData, _graphicRaycastResults);

            bool hasUIElement = false;
            for (var index = 0; index < _graphicRaycastResults.Count; index++)
            {
                var result = _graphicRaycastResults[index];
                hasUIElement = (_uiMask.value & (1 << result.gameObject.layer)) != 0;
                if (hasUIElement)
                {
                    //Debug.Log("Detected UI Element: " + result.gameObject.name);
                    break;
                }
            }

            return hasUIElement;
        }
    }
}