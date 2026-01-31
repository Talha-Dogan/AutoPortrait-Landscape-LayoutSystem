using UnityEngine;
using UnityEngine.UI;

namespace SimpleResponsiveUI
{
    /// <summary>
    /// Stores and applies different RectTransform/Layout settings for Landscape and Portrait modes.
    /// Use the Context Menu (Right Click on Component) to save settings.
    /// </summary>
    [AddComponentMenu("Simple Responsive UI/Responsive Transform")]
    [DisallowMultipleComponent]
    public class ResponsiveTransform : MonoBehaviour
    {
        [System.Serializable]
        public class LayoutSettings
        {
            public Vector2 anchorMin;
            public Vector2 anchorMax;
            public Vector2 anchoredPosition;
            public Vector2 sizeDelta;
            public Vector3 localScale = Vector3.one;

            [Tooltip("Column count if using a Grid Layout Group.")]
            public int gridConstraintCount = 2;
        }

        [Header("Orientation Settings")]
        [Tooltip("Configuration for Landscape Mode.")]
        public LayoutSettings landscapeSettings;

        [Tooltip("Configuration for Portrait Mode.")]
        public LayoutSettings portraitSettings;

        private RectTransform _rect;
        private GridLayoutGroup _grid;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();
            if (_grid == null) _grid = GetComponent<GridLayoutGroup>();
        }

        /// <summary>
        /// Applies the saved settings based on the target orientation.
        /// </summary>
        public void ApplyOrientation(bool isPortrait)
        {
            InitializeComponents();

            LayoutSettings target = isPortrait ? portraitSettings : landscapeSettings;

            // Apply RectTransform settings
            if (_rect != null)
            {
                _rect.anchorMin = target.anchorMin;
                _rect.anchorMax = target.anchorMax;
                _rect.anchoredPosition = target.anchoredPosition;
                _rect.sizeDelta = target.sizeDelta;
                _rect.localScale = target.localScale;
            }

            // Apply Grid Layout settings (if applicable)
            if (_grid != null)
            {
                _grid.constraintCount = target.gridConstraintCount;
            }
        }

        // --- CONTEXT MENU TOOLS (Right-Click on Component) ---

        [ContextMenu("Save Current As LANDSCAPE")]
        public void SaveAsLandscape()
        {
            InitializeComponents();
            landscapeSettings = GetCurrentSettings();
            Debug.Log($"[ResponsiveTransform] Saved LANDSCAPE settings for: {gameObject.name}");
        }

        [ContextMenu("Save Current As PORTRAIT")]
        public void SaveAsPortrait()
        {
            InitializeComponents();
            portraitSettings = GetCurrentSettings();
            Debug.Log($"[ResponsiveTransform] Saved PORTRAIT settings for: {gameObject.name}");
        }

        private LayoutSettings GetCurrentSettings()
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();

            LayoutSettings s = new LayoutSettings
            {
                anchorMin = _rect.anchorMin,
                anchorMax = _rect.anchorMax,
                anchoredPosition = _rect.anchoredPosition,
                sizeDelta = _rect.sizeDelta,
                localScale = _rect.localScale
            };

            if (_grid != null) s.gridConstraintCount = _grid.constraintCount;

            return s;
        }
    }
}