using UnityEngine;
using UnityEngine.UI;

namespace SimpleResponsiveUI
{
    /// <summary>
    /// Automatically detects screen orientation changes and updates all ResponsiveTransform components.
    /// Runs in both Edit Mode and Play Mode to provide real-time feedback.
    /// </summary>
    [ExecuteAlways]
    [AddComponentMenu("Simple Responsive UI/Orientation Manager")]
    public class OrientationManager : MonoBehaviour
    {
        public static OrientationManager Instance { get; private set; }

        [Header("Canvas Settings")]
        [Tooltip("The main Canvas Scaler to adjust. If empty, it will be found automatically.")]
        public CanvasScaler mainCanvasScaler;

        [Tooltip("The reference resolution for the UI (Default: 1920x1080).")]
        public Vector2 referenceResolution = new Vector2(1920, 1080);

        [Header("Match Settings")]
        [Tooltip("Match value for Landscape mode (0=Width, 1=Height). Default: 0.5")]
        [Range(0f, 1f)] public float landscapeMatch = 0.5f;

        [Tooltip("Match value for Portrait mode (0=Width, 1=Height). Default: 0 (Width) is recommended for mobile.")]
        [Range(0f, 1f)] public float portraitMatch = 0f;

        private bool _lastIsPortrait;
        private bool _firstRun = true;

        private void Awake()
        {
            // Handle Singleton pattern safely for both Editor and Play modes
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
                // In Editor mode, just assign the instance without DontDestroyOnLoad
                Instance = this;
            }
        }

        private void Start()
        {
            // Auto-find CanvasScaler if missing
            if (mainCanvasScaler == null)
                mainCanvasScaler = FindFirstObjectByType<CanvasScaler>();

            CheckOrientationAndRefresh();
        }

        private void Update()
        {
            // continuously check orientation in both Edit and Play modes
            CheckOrientationAndRefresh();
        }

        private void CheckOrientationAndRefresh()
        {
            if (mainCanvasScaler == null) return;

            // Determine orientation: True if Height > Width
            bool currentIsPortrait = Screen.height > Screen.width;

            // Update if orientation changed OR if it's the first run
            if (currentIsPortrait != _lastIsPortrait || _firstRun)
            {
                _lastIsPortrait = currentIsPortrait;
                _firstRun = false;

                UpdateCanvasSettings(currentIsPortrait);
                UpdateAllPanels(currentIsPortrait);

                // Log only in Play Mode to keep Editor Console clean
                if (Application.isPlaying)
                    Debug.Log($"[OrientationManager] Orientation Changed: {(currentIsPortrait ? "Portrait" : "Landscape")}");
            }
        }

        private void UpdateCanvasSettings(bool isPortrait)
        {
            if (mainCanvasScaler == null) return;

            mainCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            mainCanvasScaler.referenceResolution = referenceResolution;

            // Adjust 'Match' value based on orientation
            mainCanvasScaler.matchWidthOrHeight = isPortrait ? portraitMatch : landscapeMatch;
        }

        private void UpdateAllPanels(bool isPortrait)
        {
            // Find all ResponsiveTransform components efficiently (including inactive ones)
            ResponsiveTransform[] allUI = FindObjectsByType<ResponsiveTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var uiElement in allUI)
            {
                uiElement.ApplyOrientation(isPortrait);
            }
        }
    }
}