using UnityEngine;

namespace SimpleResponsiveUI
{
    /// <summary>
    /// Adjusts the Camera Field of View (FOV) based on screen orientation.
    /// Attach this directly to your Main Camera.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ResponsiveCamera : MonoBehaviour
    {
        [Header("FOV Settings")]
        [Tooltip("Field of View for Landscape mode (Horizontal).")]
        public float landscapeFOV = 50f;

        [Tooltip("Field of View for Portrait mode (Vertical).")]
        public float portraitFOV = 75f;
        private Camera _cam;
        private bool _lastIsPortrait;
        private bool _firstRun = true;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void Start()
        {
            UpdateFOV(true);
        }

        private void Update()
        {
            CheckOrientation();
        }

        private void CheckOrientation()
        {

            bool currentIsPortrait = Screen.height > Screen.width;

            if (currentIsPortrait != _lastIsPortrait || _firstRun)
            {
                _lastIsPortrait = currentIsPortrait;
                _firstRun = false;

                UpdateFOV(false);
            }
        }

        private void UpdateFOV(bool forceLog)
        {
            float targetFOV = _lastIsPortrait ? portraitFOV : landscapeFOV;
            _cam.fieldOfView = targetFOV;

#if UNITY_EDITOR
            if (forceLog)
                Debug.Log($"[ResponsiveCamera] FOV Updated: {targetFOV} (Mode: {(_lastIsPortrait ? "Portrait" : "Landscape")})");
#endif
        }
    }
}