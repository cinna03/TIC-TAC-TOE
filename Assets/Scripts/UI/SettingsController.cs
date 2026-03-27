// UI/SettingsController.cs
// Reads all Settings UI controls and writes values into GameSettings.
// Handles navigation back to Main Menu.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GameLogic;

namespace UI
{
    public class SettingsController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Dropdown modeDropdown;
        [SerializeField] private Slider difficultySlider;
        [SerializeField] private TextMeshProUGUI difficultyValueText;
        [SerializeField] private Toggle soundToggle;

        private void Start()
        {
            // Load previously saved settings into the UI on open
            modeDropdown.value = (int)GameSettings.CurrentMode;
            difficultySlider.value = GameSettings.AIDifficulty;
            difficultyValueText.text = GameSettings.AIDifficulty.ToString();
            soundToggle.isOn = GameSettings.SoundEnabled;

            // Listen for slider changes to update the number label live
            difficultySlider.onValueChanged.AddListener(OnDifficultyChanged);
        }

        /// <summary>Updates the difficulty label as the slider moves.</summary>
        private void OnDifficultyChanged(float value)
        {
            difficultyValueText.text = ((int)value).ToString();
        }

        /// <summary>
        /// Called by the Back button.
        /// Saves all current UI values into GameSettings before leaving.
        /// </summary>
        public void OnBackButtonClicked()
        {
            GameSettings.CurrentMode = (GameMode)modeDropdown.value;
            GameSettings.AIDifficulty = (int)difficultySlider.value;
            GameSettings.SoundEnabled = soundToggle.isOn;

            SceneManager.LoadScene("MainMenu");
        }
    }
}