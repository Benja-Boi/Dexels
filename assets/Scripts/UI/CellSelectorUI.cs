using Core;
using Core.Merge_Area;
using Core.Merge_Area.Scriptable_Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(BoxCollider))]
    public class CellSelectorUI : MonoBehaviour
    {
        [SerializeField] private CellSelectionOption cellSelectionOption;
        [SerializeField] private GameObject individualCellButton;
        [SerializeField] private CellSelection cellSelection; 
        
        private IndividualCellButtonUI[] _individualCellButtons;
        private TileSlot _tileSlot;
        private BoxCollider _boxCollider;
        private Vector2 _cellSize;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _tileSlot = GetComponent<TileSlot>();
        }

        private void Start()
        {
            if (_tileSlot.GetTile() != null)
            {
                Activate();
            }
        }

        public void Activate()
        {
            var colliderBounds = _boxCollider.bounds;
            var gridSize = _tileSlot.GetTile().GridSize;
            _cellSize = new Vector2(colliderBounds.size.x / gridSize, colliderBounds.size.y / gridSize);
            SpawnSingleCellSelectors();
        }

        public void Deactivate()
        {
            if (_individualCellButtons.Length == 0) return;
            var selectorsParent = _individualCellButtons[0].transform.parent.gameObject;
            foreach (var button in _individualCellButtons)
            {
                Destroy(button.gameObject);
            }
            Destroy(selectorsParent);
        }

        private void SpawnSingleCellSelectors()
        {
            if (_tileSlot.GetTile() == null) return;
            var gridSize = _tileSlot.GetTile().GridSize;
            var selectorsParent = Instantiate(new GameObject(), transform.position, Quaternion.identity,
                gameObject.transform);
            var numCells = gridSize * gridSize;
            _individualCellButtons = new IndividualCellButtonUI[numCells];

            for (var i = 0; i < numCells; i++)
            {
                var spawnLocation = CalculateCellCenter(i);
                var newSelector = Instantiate(individualCellButton, spawnLocation, Quaternion.identity,
                    selectorsParent.gameObject.transform);
                var boxCollider = newSelector.GetOrAddComponent<BoxCollider>();
                boxCollider.size = new Vector3(_cellSize.x, _cellSize.y, 0.3f);
                var singleCellButton = newSelector.GetOrAddComponent<IndividualCellButtonUI>();
                singleCellButton.SetCellIndex(i);
                singleCellButton.SetSelectionOption(cellSelectionOption);
                if (cellSelection.selection[i] == cellSelectionOption)
                {
                    singleCellButton.SetState(new CellButtonSelectedState(singleCellButton));
                }
                _individualCellButtons[i] = singleCellButton;
            }
        }

        private Vector3 CalculateCellCenter(int i)
        {
            var gridSize = _tileSlot.GetTile().GridSize;
            var halfGrid = gridSize / 2;
            var position = transform.position;
            var topLeft = new Vector2(position.x - _cellSize.x * halfGrid, position.y + _cellSize.y * halfGrid);
            var (x, y) = GridCoordinates.Coords1DTo2D(i, gridSize);
            var centerPos =
                new Vector3(topLeft.x + _cellSize.x * x, topLeft.y - _cellSize.y * y,
                    position.z - .1f); // Button is brought forward in front of the tile
            return centerPos;
        }

        public void OnCellSelect(int cellIndex, CellSelectionOption selectionOption)
        {
            if (selectionOption != cellSelectionOption)
            {
                var button = _individualCellButtons[cellIndex];
                button.SetState(new CellButtonNotSelectedState(button));
            }
        }
    }
}
