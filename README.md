# Tic-Tac-Toe — Unity Assignment

A complete, cross-platform Tic-Tac-Toe game built in Unity with clean OOP architecture, AI opponent, NUnit tests, and platform-specific UI.

---

## How to Run

### Prerequisites
- Unity 2022 LTS or newer (URP or Built-In RP both work)
- TextMeshPro package (install via Package Manager if prompted)

### Steps
1. Clone this repository
2. Open the project in Unity Hub → **Open Project**
3. In **Build Settings** add scenes in this order:
   - `Assets/Scenes/MainMenuScene`
   - `Assets/Scenes/SettingsScene`
   - `Assets/Scenes/GameScene`
4. Press **Play** from `MainMenuScene` to start

### Running Tests
1. Open **Window → General → Test Runner**
2. Select **Edit Mode**
3. Click **Run All** — all tests should pass with green ticks

---

## Scenes

| Scene | Purpose |
|-------|---------|
| `MainMenuScene` | Entry point — Play and Settings buttons, Quit (hidden on WebGL) |
| `SettingsScene` | Dropdown (mode), Slider (AI difficulty), Toggle (sound), Haptic toggle (mobile only) |
| `GameScene` | 3×3 grid gameplay, AI opponent, strikethroughs, win/draw detection, Reload |

---

## Folder / Class Structure

```
Assets/Scripts/
├── GameLogic/              ← Pure C# — no Unity dependency
│   ├── CellState.cs        Enum: Empty | X | O
│   ├── GameResult.cs       Enum: InProgress | WinX | WinO | Draw
│   ├── WinLine.cs          Struct: three cell indices that form a win
│   ├── BoardState.cs       3×3 board — PlaceSymbol, CheckWinner, IsBoardFull, Reset
│   ├── TurnManager.cs      Tracks current player, SwitchTurn, Reset
│   └── GameSettings.cs     DontDestroyOnLoad singleton — Mode, AIDiff, SoundEnabled
│
├── AI/
│   ├── IAIStrategy.cs      Interface: int GetAIMove(CellState[] board, CellState aiSymbol)
│   └── RuleBasedAIStrategy.cs  Win → Block → Random algorithm (see below)
│
├── UI/                     ← MonoBehaviours — only touch Unity API
│   ├── GameController.cs       Owns BoardState + TurnManager; fires C# events
│   ├── GridCellUI.cs           Single cell button — SetSymbol, ResetCell, OnClicked event
│   ├── BoardCellObserver.cs    Bridges OnCellPlaced/OnBoardReset → GridCellUI[]
│   ├── StatusUI.cs             Updates title text on turn change / game over
│   ├── StrikethroughUI.cs      Activates correct strikethrough GameObject on win
│   ├── SoundManager.cs         Plays audio clips; respects SoundEnabled setting
│   ├── MainMenuController.cs   Play / Settings / Quit button callbacks
│   ├── SettingsController.cs   Reads UI controls → writes GameSettings; platform UI
│   ├── QuitButtonVisibility.cs #if UNITY_WEBGL hides Quit; #if UNITY_ANDROID adds back-button
│   └── SceneBootstrapper.cs    Creates default GameSettings if scene loaded directly
│
└── Tests/
    ├── TicTacToe.Tests.asmdef
    └── BoardStateTests.cs      20+ NUnit tests (see Test Coverage below)
```

---

## Design Patterns Used

### Strategy Pattern — AI
`IAIStrategy` defines a single method `GetAIMove(...)`. `RuleBasedAIStrategy` implements it. `GameController` holds an `IAIStrategy` reference and never knows the concrete type. Swapping to a Minimax AI requires only:
```csharp
_aiStrategy = new MinimaxAIStrategy();
```

### Observer Pattern — UI Updates
`GameController` exposes C# events:
```
OnCellPlaced(int index, CellState symbol)
OnGameOver(GameResult result, int strikethroughIndex)
OnTurnChanged(CellState currentPlayer)
OnBoardReset()
```
UI classes (`StatusUI`, `StrikethroughUI`, `BoardCellObserver`, `SoundManager`) subscribe to these events. Game logic never calls into Unity UI code — the dependency arrow points one way only.

### Singleton — GameSettings
`GameSettings` uses `DontDestroyOnLoad` so settings chosen in `SettingsScene` survive the scene transition into `GameScene`.

---

## AI Algorithm

The `RuleBasedAIStrategy` follows a strict three-step priority chain every turn:

**Step 1 — Win Check**
Iterate all 8 winning lines. For each line, count how many cells the AI already owns. If the AI has exactly 2 of 3 cells in a line and the third cell is empty → play that cell immediately and win.

**Step 2 — Block Check** *(Hard difficulty only)*
Repeat the same scan for the opponent. If the opponent has exactly 2 of 3 cells in a line and the third is empty → play that cell to prevent their win.

**Step 3 — Random Fallback**
Collect all empty cells into a list and return one at random.

**Difficulty levels:**
| Level | Win Check | Block Check | Fallback |
|-------|-----------|-------------|---------|
| Easy  | ✗ | ✗ | ✓ random |
| Medium | ✓ | ✗ | ✓ random |
| Hard  | ✓ | ✓ | ✓ random |

---

## Platform-Specific UI

| Platform | Behaviour |
|---------|-----------|
| `UNITY_WEBGL` | Quit button hidden; haptic toggle hidden |
| `UNITY_ANDROID` | Back hardware button quits; haptic toggle shown |
| `UNITY_STANDALONE` | Quit button shown; haptic toggle hidden |

Implemented in `QuitButtonVisibility.cs` and `SettingsController.cs` using `#if` directives.

---

## Test Coverage

| Test Class | What's Tested |
|-----------|--------------|
| `BoardStateTests` | Empty board not full, full board detection, all 3 rows win (X & O), all 3 columns win, both diagonals, no winner on partial board, draw on full no-winner board, InProgress on empty board, PlaceSymbol throws on occupied cell, Reset clears all cells |
| `TurnManagerTests` | Starts with X, switches to O, switches back to X, Reset restores X |
| `AIStrategyTests` | Hard AI takes win (row), Hard AI takes win (diagonal), Hard AI blocks human win (column), Hard AI blocks human win (row), Hard AI prefers win over block, Easy AI returns valid empty cell, Medium AI takes available win |

---

## Builds

| Platform | Link |
|---------|------|
| PC (Windows) | *(add your build link here)* |
| Android APK | *(add your build link here)* |
| WebGL (Unity Play) | *(add your Unity Play link here)* |
