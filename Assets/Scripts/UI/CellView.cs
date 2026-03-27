// UI/CellView.cs
// Attached to each of the 9 grid cell buttons.
// Handles displaying X or O and disabling further clicks.

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cellText;

        private Button _button;
        private int _cellIndex;
        private System.Action<int> _onCellClicked;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        /// <summary>
        /// Called by GameController to initialise this cell.
        /// Stores its index and registers the click callback.
        /// </summary>
        public void Initialise(int index, System.Action<int> onCellClicked)
        {
            _cellIndex = index;
            _onCellClicked = onCellClicked;
            _button.onClick.AddListener(OnClicked);
        }

        /// <summary>Displays the player's mark and locks the cell.</summary>
        public void SetMark(string mark)
        {
            cellText.text = mark;
            _button.interactable = false; // Prevent further clicks
        }

        /// <summary>Clears the cell and re-enables clicking.</summary>
        public void Reset()
        {
            cellText.text = "";
            _button.interactable = true;
        }

        private void OnClicked()
        {
            _onCellClicked?.Invoke(_cellIndex);
        }
    }
}