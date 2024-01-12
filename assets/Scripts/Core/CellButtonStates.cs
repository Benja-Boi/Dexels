using UI;

namespace Core
{
    public abstract class CellButtonState
    {
        protected readonly IndividualCellButtonUI Button;

        protected CellButtonState(IndividualCellButtonUI button)
        {
            Button = button;
        }
        
        public abstract void OnMouseDown();
        public abstract void OnMouseOver();
        public abstract void OnMouseExit();
        public abstract void OnEnter();
    }

    public class CellButtonSelectedState : CellButtonState
    {
        public CellButtonSelectedState(IndividualCellButtonUI button) : base(button)
        { }
        
        public override void OnMouseDown()
        {
            Button.OnCellSelected();
            Button.SetState(new CellButtonNotSelectedState(Button));
        }

        public override void OnMouseOver()
        { }

        public override  void OnMouseExit()
        { }

        public override void OnEnter()
        {
            Button.SetColor(Button.selectedColor);
        }
    }
    
    public class CellButtonNotSelectedState : CellButtonState
    {
        public CellButtonNotSelectedState(IndividualCellButtonUI button) : base(button)
        { }
        
        public override  void OnMouseDown()
        {
            Button.OnCellSelected();
            Button.SetState(new CellButtonSelectedState(Button));
        }

        public override  void OnMouseOver()
        {
            Button.SetState(new CellButtonHoverState(Button));
        }

        public override  void OnMouseExit()
        { }

        public override void OnEnter()
        {
            Button.SetColor(Button.notSelectedColor);
        }
    }
    
    public class CellButtonHoverState : CellButtonState
    {
        public CellButtonHoverState(IndividualCellButtonUI button) : base(button)
        { }
        
        public override  void OnMouseDown()
        {
            Button.OnCellSelected();
            Button.SetState(new CellButtonSelectedState(Button));
        }

        public override void OnMouseOver()
        { }

        public override  void OnMouseExit()
        {
            Button.SetState(new CellButtonNotSelectedState(Button));
        }

        public override void OnEnter()
        {
            Button.SetColor(Button.hoverColor);
        }
    }
}