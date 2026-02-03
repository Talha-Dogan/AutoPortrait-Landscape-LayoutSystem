/*
 * Simple Responsive UI System
 * Copyright (c) 2026 Talha Doğan
 * * Developed by Talha Doğan to manage dynamic UI layouts for Portrait and Landscape modes.
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SimpleResponsiveUI
{
    /// <summary>
    /// Automatically detects screen orientation changes and updates all ResponsiveTransform components.
    /// persistent singleton that handles scene transitions and orientation events.
    /// </summary>
    [ExecuteAlways]
    [DefaultExecutionOrder(-100)] // Ensures initialization before standard UI scripts
    [AddComponentMenu("Simple Responsive UI/Orientation Manager")]
    public class OrientationManager : MonoBehaviour
    {
        public static OrientationManager Instance { get; private set; }

        [Header("Canvas Settings")]
        [Tooltip("The main Canvas Scaler to adjust. Automatically assigned if left empty.")]
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
            // Singleton Pattern Implementation
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
                Instance = this; // Editor Mode Singleton logic
            }
        }

        private void Start()
        {
            RefreshReferences();
            CheckOrientationAndRefresh(true); // Force initial refresh on start
        }

        private void OnEnable()
        {
            // Subscribe to scene loading events to handle UI resets
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// Triggered when a new scene is loaded. Forces a layout refresh to ensure UI is correct.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RefreshReferences();
            CheckOrientationAndRefresh(true);
        }

        private void RefreshReferences()
        {
            if (mainCanvasScaler == null)
                mainCanvasScaler = FindFirstObjectByType<CanvasScaler>();
        }

        private void Update()
        {
            CheckOrientationAndRefresh(false);
        }

        /// <summary>
        /// Checks if the screen orientation has changed and updates the UI accordingly.
        /// </summary>
        /// <param name="forceUpdate">If true, updates the UI regardless of orientation change.</param>
        private void CheckOrientationAndRefresh(bool forceUpdate)
        {
            if (mainCanvasScaler == null) return;

            // Determine orientation: True if Height > Width
            bool currentIsPortrait = Screen.height > Screen.width;

            // Update if orientation changed, it's the first run, or an update is forced
            if (currentIsPortrait != _lastIsPortrait || _firstRun || forceUpdate)
            {
                _lastIsPortrait = currentIsPortrait;
                _firstRun = false;

                UpdateCanvasSettings(currentIsPortrait);
                UpdateAllPanels(currentIsPortrait);

#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    Debug.Log($"[OrientationManager] Layout Refreshed. Mode: {(currentIsPortrait ? "Portrait" : "Landscape")}");
                }
#endif
            }
        }

        private void UpdateCanvasSettings(bool isPortrait)
        {
            if (mainCanvasScaler != null)
            {
                mainCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                mainCanvasScaler.referenceResolution = referenceResolution;
                mainCanvasScaler.matchWidthOrHeight = isPortrait ? portraitMatch : landscapeMatch;
            }
        }

        private void UpdateAllPanels(bool isPortrait)
        {
            // Efficiently find all ResponsiveTransforms (including inactive ones) to apply settings
            var allUI = FindObjectsByType<ResponsiveTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var uiElement in allUI)
            {
                uiElement.ApplyOrientation(isPortrait);
            }
        }
    }
}