using FruitMerge.Events;
using UnityEngine;
using Zenject;

namespace FruitMerge.Game
{
    public class SafeAreaHandler : IInitializable, ITickable
    {
        #region Simulations

        public enum SimDevice
        {
            None,
            iPhoneX,
            iPhoneXsMax,
            Pixel3XL_LSL,
            Pixel3XL_LSR
        }

        public static SimDevice Sim = SimDevice.None;

        Rect[] NSA_iPhoneX = new Rect[]
        {
            new Rect(0f, 102f / 2436f, 1f, 2202f / 2436f), // Portrait
            new Rect(132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f) // Landscape
        };

        Rect[] NSA_iPhoneXsMax = new Rect[]
        {
            new Rect(0f, 102f / 2688f, 1f, 2454f / 2688f), // Portrait
            new Rect(132f / 2688f, 63f / 1242f, 2424f / 2688f, 1179f / 1242f) // Landscape
        };

        Rect[] NSA_Pixel3XL_LSL = new Rect[]
        {
            new Rect(0f, 0f, 1f, 2789f / 2960f), // Portrait
            new Rect(0f, 0f, 2789f / 2960f, 1f) // Landscape
        };

        Rect[] NSA_Pixel3XL_LSR = new Rect[]
        {
            new Rect(0f, 0f, 1f, 2789f / 2960f), // Portrait
            new Rect(171f / 2960f, 0f, 2789f / 2960f, 1f) // Landscape
        };

        #endregion

        [Inject] private SignalBus _signalBus;
        public SafeAreaAnchor SafeAreaAnchor { get; set; }

        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
        private Vector2Int _lastScreenSize = new Vector2Int(0, 0);
        private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;
        private bool _conformX;
        private bool _conformY;
        private bool _isLogging;

        public void Initialize()
        {
            _conformX = true;
            _conformY = true;
            _isLogging = false;
        }

        public void Tick()
        {
            Refresh();
        }

        private void Refresh()
        {
            Rect safeArea = GetSafeAreaRect();

            if (safeArea != _lastSafeArea
                || Screen.width != _lastScreenSize.x
                || Screen.height != _lastScreenSize.y
                || Screen.orientation != _lastOrientation)
            {
                // Fix for having auto-rotate off and manually forcing a screen orientation.
                // See https://forum.unity.com/threads/569236/#post-4473253 and https://forum.unity.com/threads/569236/page-2#post-5166467
                _lastScreenSize.x = Screen.width;
                _lastScreenSize.y = Screen.height;
                _lastOrientation = Screen.orientation;

                SafeAreaAnchor = GetSafeAreaAnchor();
                _signalBus.Fire(new GameSignals.OnSafeAreaChanged()
                {
                    SafeAreaAnchor = SafeAreaAnchor
                });
                
                if (_isLogging)
                {
                    Debug.Log($"Safe area changed : {SafeAreaAnchor}, on full extents w={Screen.width}, h={Screen.height}");
                }
            }
        }

        private Rect GetSafeAreaRect()
        {
            Rect safeArea = Screen.safeArea;

            if (Application.isEditor && Sim != SimDevice.None)
            {
                Rect nsa = new Rect(0, 0, Screen.width, Screen.height);

                switch (Sim)
                {
                    case SimDevice.iPhoneX:
                        if (Screen.height > Screen.width) // Portrait
                            nsa = NSA_iPhoneX[0];
                        else // Landscape
                            nsa = NSA_iPhoneX[1];
                        break;
                    case SimDevice.iPhoneXsMax:
                        if (Screen.height > Screen.width) // Portrait
                            nsa = NSA_iPhoneXsMax[0];
                        else // Landscape
                            nsa = NSA_iPhoneXsMax[1];
                        break;
                    case SimDevice.Pixel3XL_LSL:
                        if (Screen.height > Screen.width) // Portrait
                            nsa = NSA_Pixel3XL_LSL[0];
                        else // Landscape
                            nsa = NSA_Pixel3XL_LSL[1];
                        break;
                    case SimDevice.Pixel3XL_LSR:
                        if (Screen.height > Screen.width) // Portrait
                            nsa = NSA_Pixel3XL_LSR[0];
                        else // Landscape
                            nsa = NSA_Pixel3XL_LSR[1];
                        break;
                    default:
                        break;
                }

                safeArea = new Rect(Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width,
                    Screen.height * nsa.height);
            }

            return safeArea;
        }

        private SafeAreaAnchor GetSafeAreaAnchor()
        {
            var result = new SafeAreaAnchor(Vector2.zero, Vector2.one);
            var safeAreaRect = GetSafeAreaRect();
            _lastSafeArea = safeAreaRect;

            // Ignore x-axis?
            if (!_conformX)
            {
                safeAreaRect.x = 0;
                safeAreaRect.width = Screen.width;
            }

            // Ignore y-axis?
            if (!_conformY)
            {
                safeAreaRect.y = 0;
                safeAreaRect.height = Screen.height;
            }

            // Check for invalid screen startup state on some Samsung devices (see below)
            if (Screen.width > 0 && Screen.height > 0)
            {
                // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
                Vector2 anchorMin = safeAreaRect.position;
                Vector2 anchorMax = safeAreaRect.position + safeAreaRect.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                // Fix for some Samsung devices (e.g. Note 10+, A71, S20) where Refresh gets called twice and the first time returns NaN anchor coordinates
                // See https://forum.unity.com/threads/569236/page-2#post-6199352
                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    result.Min = anchorMin;
                    result.Max = anchorMax;
                }
            }

            return result;
        }

        public void ApplySafeArea(RectTransform rectTransform)
        {
            rectTransform.anchorMin = SafeAreaAnchor.Min;
            rectTransform.anchorMax = SafeAreaAnchor.Max;
        }
    }

    public struct SafeAreaAnchor
    {
        public Vector2 Min;
        public Vector2 Max;

        public SafeAreaAnchor(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"Min: [x:{Min.x}, y={Min.y}], Min: [x:{Max.x}, y={Max.y}]";
        }
    }
}