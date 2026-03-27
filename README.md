# Tic-Tac-Toe — Unity Game

A complete Tic-Tac-Toe game built in Unity with Human vs Human and Human vs AI modes.

---

## How to Run

1. Clone this repository
2. Open the project in Unity 6 (LTS)
3. Open the `MainMenu` scene from `Assets/Scenes/`
4. Press Play — or build for your target platform

### Build Links
- PC Build: [link here]
- WebGL Build: [link here]
- Android Build: [link here]

---

## Class Structure

### GameLogic/ (Pure C# — no Unity dependency)
| Class | Responsibility |
|-------|---------------|
| `BoardState` | Stores the 9-cell grid state, handles placing marks and resetting |
| `TurnManager` | Tracks whose turn it is (X or O) and switches between players |
| `WinChecker` | Checks all 8 winning patterns and detects draw conditions |
| `GameSettings` | Static container that carries settings (mode, difficulty, sound) across scenes |

### AI/
| Class | Responsibility |
|-------|---------------|
| `IAIStrategy` | Interface defining the `GetAIMove()` contract for all AI strategies |
| `RuleBasedAIStrategy` | Implements the rule-based AI algorithm (win → block → random) |

### UI/ (MonoBehaviours — Unity dependent)
| Class | Responsibility |
|-------|---------------|
| `GameController` | Orchestrates the Game scene, connects logic classes to UI |
| `CellView` | Attached to each cell button, displays marks and handles clicks |
| `MainMenuController` | Handles Main Menu navigation and platform-specific UI |
| `SettingsController` | Reads UI controls and writes values into GameSettings |

---

## AI Algorithm

The AI opponent uses a rule-based strategy implemented through the `IAIStrategy` 
interface and `RuleBasedAIStrategy` class. On each turn, the AI evaluates the 
board in three sequential steps:

1. **Win:** Scan all 8 winning patterns to check whether the AI can win 
immediately by completing a line of three. If a winning move exists, take it.

2. **Block:** If no winning move is available, perform the same scan for the 
human player's mark to detect and block any immediate threats.

3. **Random:** If neither condition applies, select a random cell from the 
remaining empty cells.

This design guarantees the AI never overlooks an obvious win or block, while 
the interface-based structure allows alternative strategies (e.g. Minimax) to 
be swapped in without modifying the game manager.

---

## Design Patterns Used

- **Strategy Pattern** — `IAIStrategy` interface allows AI behaviour to be 
swapped without changing `GameController`
- **Observer Pattern** — Logic classes report state changes, `GameController` 
listens and updates visuals accordingly

## Folder Structure
