// GameLogic/GameSettings.cs
// Static container that persists settings choices across scenes.
// Populated in SettingsController, read in GameController.

namespace GameLogic
{
    public enum GameMode { HumanVsHuman, HumanVsAI }

    public static class GameSettings
    {
        public static GameMode CurrentMode { get; set; } = GameMode.HumanVsHuman;
        public static int AIDifficulty { get; set; } = 5;
        public static bool SoundEnabled { get; set; } = true;
    }
}