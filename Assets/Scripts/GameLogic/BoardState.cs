// GameLogic/BoardState.cs
// Represents the pure data state of the Tic-Tac-Toe board.
// No Unity dependencies — safe to use in NUnit tests.

namespace GameLogic
{
    public enum CellState { Empty, X, O }

    public class BoardState
    {
        public CellState[] Cells { get; private set; } // 9 cells, index 0-8

        public BoardState()
        {
            Cells = new CellState[9];
            Reset();
        }

        /// <summary>Resets all cells to Empty.</summary>
        public void Reset()
        {
            for (int i = 0; i < Cells.Length; i++)
                Cells[i] = CellState.Empty;
        }

        /// <summary>
        /// Attempts to place a mark on the board.
        /// Returns true if successful, false if cell is already taken.
        /// </summary>
        public bool PlaceMark(int index, CellState mark)
        {
            if (index < 0 || index > 8) return false;
            if (Cells[index] != CellState.Empty) return false;

            Cells[index] = mark;
            return true;
        }

        /// <summary>Returns true if all 9 cells are filled.</summary>
        public bool IsFull()
        {
            foreach (var cell in Cells)
                if (cell == CellState.Empty) return false;
            return true;
        }

        /// <summary>Returns a copy of the current cells array.</summary>
        public CellState[] GetSnapshot()
        {
            return (CellState[])Cells.Clone();
        }
    }
}