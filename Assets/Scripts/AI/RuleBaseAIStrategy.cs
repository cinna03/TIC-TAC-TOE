// AI/RuleBasedAIStrategy.cs
// Implements a rule-based AI using the IAIStrategy interface.
//
// Algorithm (executed in priority order):
//   1. WIN:   If the AI can win on this move, take that cell.
//   2. BLOCK: If the human can win on their next move, block that cell.
//   3. RANDOM: No immediate win or block needed — pick a random empty cell.
//
// This ensures the AI never misses a win or an obvious block,
// while remaining simple enough to understand and test.

using System.Collections.Generic;
using GameLogic;

namespace AI
{
    public class RuleBasedAIStrategy : IAIStrategy
    {
        // All 8 winning line combinations
        private static readonly int[][] WinPatterns = new int[][]
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

        private System.Random _random = new System.Random();

        /// <summary>
        /// Main entry point. Returns the index of the cell the AI will play.
        /// </summary>
        public int GetAIMove(CellState[] boardState)
        {
            // --- STEP 1: Check if AI (O) can win immediately ---
            int winMove = FindCriticalMove(boardState, CellState.O);
            if (winMove != -1) return winMove;

            // --- STEP 2: Check if Human (X) is about to win — block them ---
            int blockMove = FindCriticalMove(boardState, CellState.X);
            if (blockMove != -1) return blockMove;

            // --- STEP 3: No win or block needed — pick a random empty cell ---
            return PickRandomMove(boardState);
        }

        /// <summary>
        /// Looks for a line where the given player has 2 marks and the
        /// third cell is empty. Returns that empty cell index, or -1 if none.
        /// This is used for both WIN detection (pass AI's mark)
        /// and BLOCK detection (pass Human's mark).
        /// </summary>
        private int FindCriticalMove(CellState[] board, CellState player)
        {
            foreach (var pattern in WinPatterns)
            {
                int playerCount = 0;
                int emptyIndex = -1;

                foreach (int idx in pattern)
                {
                    if (board[idx] == player)
                        playerCount++;
                    else if (board[idx] == CellState.Empty)
                        emptyIndex = idx;
                }

                // Two of this player's marks + one empty = critical cell
                if (playerCount == 2 && emptyIndex != -1)
                    return emptyIndex;
            }

            return -1; // No critical move found
        }

        /// <summary>
        /// Collects all empty cells and returns one at random.
        /// </summary>
        private int PickRandomMove(CellState[] board)
        {
            List<int> emptyCells = new List<int>();

            for (int i = 0; i < board.Length; i++)
                if (board[i] == CellState.Empty)
                    emptyCells.Add(i);

            if (emptyCells.Count == 0) return -1; // Board is full (safety check)

            return emptyCells[_random.Next(emptyCells.Count)];
        }
    }
}