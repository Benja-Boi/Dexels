using Core;
using Core.Merge_Area;
using Core.Merge_Area.Scriptable_Objects;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(BoxCollider))]
    public class IndividualCellButtonUI : MonoBehaviour
    {
        private CellButtonState _state;
        private int  _cellIndex;
        
        [SerializeField] private CellSelectionOption cellSelectionOption;
        [SerializeField] private SpriteRenderer buttonRenderer;
        [SerializeField] private CellSelectEvent cellSelectedEvent;

        public Color selectedColor;

        public Color notSelectedColor;

        public Color hoverColor;

        private void Awake()
        {
            SetColor(notSelectedColor);
            SetState(new CellButtonNotSelectedState(this));
        }
        
        public void SetCellIndex(int newIndex)
        {
            _cellIndex = newIndex;
        }
        
        private void OnMouseOver()
        {
            _state.OnMouseOver();
        }

        private void OnMouseDown()
        {
            _state.OnMouseDown();
        }
        
        private void OnMouseExit()
        {
            _state.OnMouseExit();
        }

        public void SetState(CellButtonState newState)
        {
            _state = newState;
            _state.OnEnter();
        }

        public void SetColor(Color color)
        {
            buttonRenderer.color = color;
        }
        
        public void OnCellSelected()
        {
            cellSelectedEvent.Raise(_cellIndex, cellSelectionOption);
        }

        public void SetSelectionOption(CellSelectionOption newCellSelectionOption)
        {
            cellSelectionOption = newCellSelectionOption;
        }
    }
}