// UI/GameController.cs
// Orchestrates the Game scene.
// Connects pure logic classes (BoardState, TurnManager, WinChecker)
// with the UI layer (CellView, strikethroughs, title text).
//
// Follows the Observer-style pattern:
// Logic classes report state → GameController updates visuals.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using GameLogic;
using AI;

namespace UI
{
    public class GameController : MonoBehaviour
    {
        [Header("Cell Views")]
        [SerializeField] private CellView[] cellViews;

        [Header("Strikethrough Lines")]
        [SerializeField] private GameObject[] strikethroughLines;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private GameObject reloadButton;
        [SerializeField] private GameObject backButton; // Back to Main Menu

        // --- Logic layer objects (no Unity dependency) ---
        private BoardState _boardState;
        private TurnManager _turnManager;
        private WinChecker _winChecker;
        private IAIStrategy _aiStrategy;

        private bool _gameOver = false;

        private void Start()
        {
            // Initialise all logic classes
            _boardState = new BoardState();
            _turnManager = new TurnManager();
            _winChecker = new WinChecker();

            // Set AI strategy based on Settings
            if (GameSettings.CurrentMode == GameMode.HumanVsAI)
                _aiStrategy = new RuleBasedAIStrategy();
            else
                _aiStrategy = null;

            // Initialise all cell views with their index + click callback
            for (int i = 0; i < cellViews.Length; i++)
            {
                int index = i;
                cellViews[i].Initialise(index, OnCellClicked);
            }

            reloadButton.SetActive(false);
            UpdateTitleText();
            SetupPlatformUI();
        }

        /// <summary>
        /// Adjusts UI layout based on current platform.
        /// Uses Unity conditional compilation for platform-aware behaviour.
        /// </summary>
        private void SetupPlatformUI()
        {
#if UNITY_ANDROID
            titleText.fontSize = 36;
#elif UNITY_WEBGL
            titleText.fontSize = 42;
#elif UNITY_STANDALONE
            titleText.fontSize = 48;
#endif
        }

        /// <summary>
        /// Called when a player clicks a cell.
        /// Handles placing mark, checking win/draw, switching turns, and AI.
        /// </summary>
        private void OnCellClicked(int index)
        {
            if (_gameOver) return;

            bool placed = _boardState.PlaceMark(index, _turnManager.CurrentPlayer);
            if (!placed) return;

            string mark = _turnManager.CurrentPlayer == CellState.X ? "X" : "O";
            cellViews[index].SetMark(mark);

            if (CheckEndCondition()) return;

            _turnManager.SwitchTurn();
            UpdateTitleText();

            if (GameSettings.CurrentMode == GameMode.HumanVsAI
                && _turnManager.CurrentPlayer == CellState.O
                && !_gameOver)
            {
                StartCoroutine(AITakeTurn());
            }
        }

        /// <summary>
        /// AI takes its turn after a short delay (simulates "thinking").
        /// </summary>
        private IEnumerator AITakeTurn()
        {
            SetAllCellsInteractable(false);
            titleText.text = "AI is thinking...";

            yield return new WaitForSeconds(0.6f);

            int aiMove = _aiStrategy.GetAIMove(_boardState.GetSnapshot());

            if (aiMove != -1)
            {
                _boardState.PlaceMark(aiMove, CellState.O);
                cellViews[aiMove].SetMark("O");

                if (CheckEndCondition()) yield break;

                _turnManager.SwitchTurn();
                UpdateTitleText();
            }

            SetAllCellsInteractable(true);
        }

        /// <summary>
        /// Checks if the game has ended (win or draw).
        /// Returns true if the game is over.
        /// </summary>
        private bool CheckEndCondition()
        {
            WinResult result = _winChecker.CheckWinner(_boardState.Cells);

            if (result.HasWinner)
            {
                ShowStrikethrough(result.WinningLine);

                string winner = result.Winner == CellState.X ? "Player X" : "Player O";
                titleText.text = $"{winner} Wins! 🎉";

                EndGame();
                return true;
            }

            if (_winChecker.IsDraw(_boardState.Cells, false))
            {
                titleText.text = "It's a Draw!";
                EndGame();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Matches WinningLine indices to the correct strikethrough GameObject.
        /// </summary>
        private void ShowStrikethrough(int[] winningLine)
        {
            int[][] patterns = new int[][]
            {
                new int[] { 0, 1, 2 },
                new int[] { 3, 4, 5 },
                new int[] { 6, 7, 8 },
                new int[] { 0, 3, 6 },
                new int[] { 1, 4, 7 },
                new int[] { 2, 5, 8 },
                new int[] { 0, 4, 8 },
                new int[] { 2, 4, 6 }
            };

            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i][0] == winningLine[0] &&
                    patterns[i][1] == winningLine[1] &&
                    patterns[i][2] == winningLine[2])
                {
                    strikethroughLines[i].SetActive(true);
                    return;
                }
            }
        }

        /// <summary>Marks game as over, disables cells, shows reload button.</summary>
        private void EndGame()
        {
            _gameOver = true;
            SetAllCellsInteractable(false);
            reloadButton.SetActive(true);
        }

        /// <summary>Called by the Reload button. Fully resets the game.</summary>
        public void OnReloadButtonClicked()
        {
            _boardState.Reset();
            _turnManager.Reset();
            _gameOver = false;

            foreach (var cell in cellViews)
                cell.Reset();

            foreach (var line in strikethroughLines)
                line.SetActive(false);

            reloadButton.SetActive(false);
            UpdateTitleText();

            if (GameSettings.CurrentMode == GameMode.HumanVsAI
                && _turnManager.CurrentPlayer == CellState.O)
            {
                StartCoroutine(AITakeTurn());
            }
        }

        /// <summary>
        /// Called by the Back button.
        /// Returns the player to the Main Menu.
        /// </summary>
        public void OnBackButtonClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>Updates the title to show whose turn it is.</summary>
        private void UpdateTitleText()
        {
            titleText.text = $"{_turnManager.GetCurrentPlayerLabel()}'s Turn";
        }

        /// <summary>Enables or disables all cell buttons at once.</summary>
        private void SetAllCellsInteractable(bool interactable)
        {
            foreach (var cell in cellViews)
                cell.GetComponent<UnityEngine.UI.Button>().interactable = interactable;
        }
    }
}