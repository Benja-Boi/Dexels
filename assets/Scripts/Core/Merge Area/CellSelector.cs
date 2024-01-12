using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Merge_Area
{
    public class CellSelector : MonoBehaviour
    {
        [SerializeField] private CellSelection cellSelection;

        public void SelectCell(int cellIndex, CellSelectionOption thisSelectionOption)
        {
            if (cellIndex < 0 || cellIndex >= cellSelection.selection.Length) return;
            
            if (cellSelection.selection[cellIndex] == thisSelectionOption)
            {
                cellSelection.selection[cellIndex] = CellSelectionOption.Neither;
            }
            else
            {
                cellSelection.selection[cellIndex] = thisSelectionOption;
            }
        }
    }
}