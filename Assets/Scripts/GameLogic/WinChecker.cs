// GameLogic/WinChecker.cs
// Stateless utility that checks win/draw conditions.
// No Unity dependencies — safe to use in NUnit tests.

namespace GameLogic
{
    public class WinResult
    {
        public bool HasWinner { get; set; }
        public CellState Winner { get; set; }
        public int[] WinningLine { get; set; } // The 3 cell indices that won
    }

    public class WinChecker
    {
        // All 8 possible winning combinations (rows, columns, diagonals)
        private static readonly int[][] WinPatterns = new int[][]
        {
            new int[] { 0, 1, 2 }, // Top row
            new int[] { 3, 4, 5 }, // Middle row
            new int[] { 6, 7, 8 }, // Bottom row
            new int[] { 0, 3, 6 }, // Left column
            new int[] { 1, 4, 7 }, // Middle column
            new int[] { 2, 5, 8 }, // Right column
            new int[] { 0, 4, 8 }, // Diagonal top-left to bottom-right
            new int[] { 2, 4, 6 }  // Diagonal top-right to bottom-left
        };

        /// <summary>
        /// Checks the board for a winner.
        /// Returns a WinResult with HasWinner=false if no winner yet.
        /// </summary>
        public WinResult CheckWinner(CellState[] cells)
        {
            foreach (var pattern in WinPatterns)
            {
                CellState first = cells[pattern[0]];

                // Skip if first cell is empty
                if (first == CellState.Empty) continue;

                // Check if all 3 cells in the pattern match
                if (cells[pattern[1]] == first && cells[pattern[2]] == first)
                {
                    return new WinResult
                    {
                        HasWinner = true,
                        Winner = first,
                        WinningLine = pattern
                    };
                }
            }

            return new WinResult { HasWinner = false };
        }

        /// <summary>
        /// Returns true if the board is full with no winner (draw condition).
        /// </summary>
        public bool IsDraw(CellState[] cells, bool hasWinner)
        {
            if (hasWinner) return false;

            foreach (var cell in cells)
                if (cell == CellState.Empty) return false;

            return true;
        }
    }
}