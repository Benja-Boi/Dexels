using System;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(BoxCollider))]
    public class SingleCellSelectorUI : MonoBehaviour
    {
        private CellSelectorUI _selectorUI;
        private int  _cellIndex;

        public void SetSelectorUI(CellSelectorUI cellSelectorUI)
        {
            _selectorUI = cellSelectorUI;
        }
        
        public void SetCellIndex(int newIndex)
        {
            _cellIndex = newIndex;
        }
        
        private void OnMouseOver()
        {
            _selectorUI.OnCellHover(_cellIndex);
        }

        private void OnMouseDown()
        {
            _selectorUI.OnCellClick(_cellIndex);
        }
    }
}