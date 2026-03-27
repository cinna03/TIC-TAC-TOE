// GameLogic/TurnManager.cs
// Manages whose turn it is (X or O).
// No Unity dependencies — safe to use in NUnit tests.

namespace GameLogic
{
    public class TurnManager
    {
        public CellState CurrentPlayer { get; private set; }

        public TurnManager()
        {
            CurrentPlayer = CellState.X; // X always goes first
        }

        /// <summary>Switches the active player from X to O or vice versa.</summary>
        public void SwitchTurn()
        {
            CurrentPlayer = (CurrentPlayer == CellState.X)
                ? CellState.O
                : CellState.X;
        }

        /// <summary>Resets the turn back to X (start of new game).</summary>
        public void Reset()
        {
            CurrentPlayer = CellState.X;
        }

        /// <summary>Returns a display-friendly string for the current player.</summary>
        public string GetCurrentPlayerLabel()
        {
            return CurrentPlayer == CellState.X ? "Player X" : "Player O";
        }
    }
}