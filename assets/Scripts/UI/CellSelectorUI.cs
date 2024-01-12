using Core;
using Core.ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(BoxCollider))]
    public class CellSelectorUI : MonoBehaviour
    {
        [SerializeField] private CellSelectionOption cellSelectionOption;
        [SerializeField] private CellSelectEvent cellSelectedEvent;
        [SerializeField] private GameObject singleCellSelectorObject;
        
        private TileSlot _tileSlot;
        private BoxCollider _boxCollider;
        private Vector2 _cellSize;
        private bool _isActive;
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _tileSlot = GetComponentInParent<TileSlot>();
        }

        private void Start()
        {
            // Calculate cell size based on the collider bounds
            var colliderBounds = _boxCollider.bounds;
            _cellSize = new Vector2(colliderBounds.size.x / _tileSlot.tileData.GridSize, colliderBounds.size.y / _tileSlot.tileData.GridSize);
            SpawnSingleCellSelectors();
        }

        private void SpawnSingleCellSelectors()
        {
            if (_tileSlot.tileData == null) return;

            for (var i = 0; i < _tileSlot.tileData.GridSize * _tileSlot.tileData.GridSize; i++)
            {
                var spawnLocation = CalculateCellCenter(i);
                var newSelector = Instantiate(singleCellSelectorObject, spawnLocation, Quaternion.identity, this.gameObject.transform);
                var boxCollider = newSelector.GetOrAddComponent<BoxCollider>();
                boxCollider.size = new Vector3(_cellSize.x, _cellSize.y, 0.2f);
                var singleCellSelector = newSelector.GetOrAddComponent<SingleCellSelectorUI>();
                singleCellSelector.SetCellIndex(9);
                singleCellSelector.SetSelectorUI(this);
            }
        }

        private Vector3 CalculateCellCenter(int i)
        {
            var halfGrid = _tileSlot.tileData.GridSize / 2;
            var position = transform.position;
            var topLeft = (Vector2) position - (_cellSize * halfGrid);
            var (x, y) = Utils.Coords1DTo2D(i, _tileSlot.tileData.GridSize);
            var centerPos = new Vector3(topLeft.x - _cellSize.x * x, topLeft.y - _cellSize.y * y, position.z);
            return centerPos;
        }

        public void OnCellHover(int cellIndex)
        {
            Debug.Log("Hovering over cell: " + cellIndex);
        }

        public void OnCellClick(int cellIndex)
        {
            cellSelectedEvent.Raise(cellIndex, cellSelectionOption);
        }
        
    }
}
