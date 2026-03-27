// AI/IAIStrategy.cs
// Interface for all AI strategies.
// Using the Strategy Pattern allows different AI behaviours
// to be swapped in without changing the game manager.

using GameLogic;

namespace AI
{
    public interface IAIStrategy
    {
        /// <summary>
        /// Given the current board state, returns the index (0-8)
        /// of the cell the AI wants to play.
        /// </summary>
        int GetAIMove(CellState[] boardState);
    }
}