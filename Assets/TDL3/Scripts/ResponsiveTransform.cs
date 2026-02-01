/*
 * Simple Responsive UI System
 * Copyright (c) 2026 Talha Doğan
 * * Developed by Talha Doğan to manage dynamic UI layouts for Portrait and Landscape modes.
 */

using UnityEngine;
using UnityEngine.UI;

namespace SimpleResponsiveUI
{
    /// <summary>
    /// Stores and applies RectTransform, Grid (including alignment/axis), and Sprite settings.
    /// Right-click the component in the Inspector to save current settings.
    /// </summary>
    [AddComponentMenu("Simple Responsive UI/Responsive Transform")]
    [DisallowMultipleComponent]
    public class ResponsiveTransform : MonoBehaviour
    {
        [System.Serializable]
        public class LayoutSettings
        {
            [Header("Rect Transform Settings")]
            public Vector2 anchorMin;
            public Vector2 anchorMax;
            public Vector2 anchoredPosition;
            public Vector2 sizeDelta;
            public Vector3 localScale = Vector3.one;

            [Header("Grid Layout Settings")]
            [Tooltip("How the grid adds items (Flexible, Fixed Column, Fixed Row).")]
            public GridLayoutGroup.Constraint gridConstraint = GridLayoutGroup.Constraint.Flexible; // NEW

            [Tooltip("Column/Row count limit.")]
            public int gridConstraintCount = 2;

            [Tooltip("Size of each element (X, Y).")]
            public Vector2 gridCellSize = new Vector2(100, 100);

            [Tooltip("Spacing between elements (X, Y).")]
            public Vector2 gridSpacing;

            [Tooltip("Which corner should the grid start from?")]
            public GridLayoutGroup.Corner startCorner; // NEW

            [Tooltip("Which axis should the grid fill first?")]
            public GridLayoutGroup.Axis startAxis; // NEW

            [Tooltip("How should children be aligned within the grid?")]
            public TextAnchor childAlignment; // NEW

            [Header("Visual Settings")]
            [Tooltip("The sprite to display in this orientation (Requires Image component).")]
            public Sprite uiSprite;
        }

        [Header("Orientation Configurations")]
        [Tooltip("Configuration for Landscape Mode.")]
        public LayoutSettings landscapeSettings;

        [Tooltip("Configuration for Portrait Mode.")]
        public LayoutSettings portraitSettings;

        // Cached components
        private RectTransform _rect;
        private GridLayoutGroup _grid;
        private Image _image;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();
            if (_grid == null) _grid = GetComponent<GridLayoutGroup>();
            if (_image == null) _image = GetComponent<Image>();
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

            // Apply Full Grid Layout settings
            if (_grid != null)
            {
                _grid.constraint = target.gridConstraint;           // Type (Flexible/Fixed)
                _grid.constraintCount = target.gridConstraintCount; // Count
                _grid.cellSize = target.gridCellSize;
                _grid.spacing = target.gridSpacing;
                _grid.startCorner = target.startCorner;             // NEW
                _grid.startAxis = target.startAxis;                 // NEW
                _grid.childAlignment = target.childAlignment;       // NEW
            }

            // Apply Sprite settings
            if (_image != null && target.uiSprite != null)
            {
                _image.sprite = target.uiSprite;
            }
        }

        #region Context Menu Tools (Editor)

        [ContextMenu("Save Current As LANDSCAPE")]
        public void SaveAsLandscape()
        {
            InitializeComponents();
            landscapeSettings = GetCurrentSettings();
#if UNITY_EDITOR
            Debug.Log($"[ResponsiveTransform] Saved LANDSCAPE settings for: {gameObject.name}");
#endif
        }

        [ContextMenu("Save Current As PORTRAIT")]
        public void SaveAsPortrait()
        {
            InitializeComponents();
            portraitSettings = GetCurrentSettings();
#if UNITY_EDITOR
            Debug.Log($"[ResponsiveTransform] Saved PORTRAIT settings for: {gameObject.name}");
#endif
        }

        private LayoutSettings GetCurrentSettings()
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();
            if (_grid == null) _grid = GetComponent<GridLayoutGroup>();
            if (_image == null) _image = GetComponent<Image>();

            LayoutSettings s = new LayoutSettings
            {
                anchorMin = _rect.anchorMin,
                anchorMax = _rect.anchorMax,
                anchoredPosition = _rect.anchoredPosition,
                sizeDelta = _rect.sizeDelta,
                localScale = _rect.localScale
            };

            if (_grid != null)
            {
                s.gridConstraint = _grid.constraint;
                s.gridConstraintCount = _grid.constraintCount;
                s.gridCellSize = _grid.cellSize;
                s.gridSpacing = _grid.spacing;
                s.startCorner = _grid.startCorner;      // Save Start Corner
                s.startAxis = _grid.startAxis;          // Save Start Axis
                s.childAlignment = _grid.childAlignment;// Save Alignment
            }

            if (_image != null)
            {
                s.uiSprite = _image.sprite;
            }

            return s;
        }

        #endregion
    }
}