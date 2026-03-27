// UI/MainMenuController.cs
// Controls the Main Menu scene.
// Uses conditional compilation to show/hide platform-specific UI.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Platform-Specific UI")]
        [SerializeField] private GameObject quitButton;    // Only shown on Standalone
        [SerializeField] private GameObject mobileNotice; // Only shown on Android

        private void Start()
        {
            SetupPlatformUI();
        }

        /// <summary>
        /// Shows or hides UI elements based on the current platform.
        /// Uses Unity conditional compilation for platform-aware behaviour.
        /// </summary>
        private void SetupPlatformUI()
        {
#if UNITY_STANDALONE
            // Show Quit button only on PC — quitting doesn't apply to WebGL or Mobile
            if (quitButton != null) quitButton.SetActive(true);
            if (mobileNotice != null) mobileNotice.SetActive(false);

#elif UNITY_ANDROID
            // Show mobile notice on Android, hide quit button
            if (quitButton != null) quitButton.SetActive(false);
            if (mobileNotice != null) mobileNotice.SetActive(true);

#elif UNITY_WEBGL
            // Hide quit button on WebGL — browser tab handles closing
            if (quitButton != null) quitButton.SetActive(false);
            if (mobileNotice != null) mobileNotice.SetActive(false);
#endif
        }

        /// <summary>Called by the Play button.</summary>
        public void OnPlayButtonClicked()
        {
            SceneManager.LoadScene("Game");
        }

        /// <summary>Called by the Settings button.</summary>
        public void OnSettingsButtonClicked()
        {
            SceneManager.LoadScene("Settings");
        }

        /// <summary>
        /// Called by the Quit button (Standalone only).
        /// </summary>
        public void OnQuitButtonClicked()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
        }
    }
}