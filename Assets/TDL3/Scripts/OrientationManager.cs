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
    /// Automatically detects screen orientation changes and updates all ResponsiveTransform components.
    /// Runs in both Edit Mode and Play Mode.
    /// </summary>
    [ExecuteAlways]
    [DefaultExecutionOrder(-100)] // Ensures this initializes before other UI scripts
    [AddComponentMenu("Simple Responsive UI/Orientation Manager")]
    public class OrientationManager : MonoBehaviour
    {
        public static OrientationManager Instance { get; private set; }

        [Header("Canvas Settings")]
        [Tooltip("The main Canvas Scaler to adjust. Found automatically if empty.")]
        public CanvasScaler mainCanvasScaler;

        [Tooltip("The reference resolution for the UI (Default: 1920x1080).")]
        public Vector2 referenceResolution = new Vector2(1920, 1080);

        [Header("Match Settings")]
        [Range(0f, 1f), Tooltip("Match value for Landscape (0=Width, 1=Height).")]
        public float landscapeMatch = 0.5f;

        [Range(0f, 1f), Tooltip("Match value for Portrait (0=Width, 1=Height).")]
        public float portraitMatch = 0f;

        private bool _lastIsPortrait;
        private bool _firstRun = true;

        private void Awake()
        {
            // Singleton Logic
            if (Application.isPlaying)
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Instance = this; // Editor Mode Singleton
            }
        }

        private void Start()
        {
            if (mainCanvasScaler == null)
                mainCanvasScaler = FindFirstObjectByType<CanvasScaler>();

            CheckOrientationAndRefresh();
        }

        private void Update()
        {
            CheckOrientationAndRefresh();
        }

        private void CheckOrientationAndRefresh()
        {
            if (mainCanvasScaler == null) return;

            // Determine orientation: True if Height > Width
            bool currentIsPortrait = Screen.height > Screen.width;

            // Update only if orientation changed or it is the first run
            if (currentIsPortrait != _lastIsPortrait || _firstRun)
            {
                _lastIsPortrait = currentIsPortrait;
                _firstRun = false;

                UpdateCanvasSettings(currentIsPortrait);
                UpdateAllPanels(currentIsPortrait);

                if (Application.isPlaying)
                {
                    Debug.Log($"[OrientationManager] Orientation Changed: {(currentIsPortrait ? "Portrait" : "Landscape")}");
                }
            }
        }

        private void UpdateCanvasSettings(bool isPortrait)
        {
            mainCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            mainCanvasScaler.referenceResolution = referenceResolution;
            mainCanvasScaler.matchWidthOrHeight = isPortrait ? portraitMatch : landscapeMatch;
        }

        private void UpdateAllPanels(bool isPortrait)
        {
            // Efficiently find all ResponsiveTransforms (including inactive ones)
            var allUI = FindObjectsByType<ResponsiveTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var uiElement in allUI)
            {
                uiElement.ApplyOrientation(isPortrait);
            }
        }
    }
}