using Core.ScriptableObjects;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TileSlot))]
    [RequireComponent(typeof(BoxCollider))]
    public class CellSelectorUI : MonoBehaviour
    {
        [SerializeField] private CellSelectionOption cellSelectionOption;
        [SerializeField] private CellSelectEvent cellSelectedEvent;

        private TileSlot _tileSlot;
        private BoxCollider _boxCollider;
        private Vector2 _cellSize;

        #region Monobehaviour
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _tileSlot = GetComponent<TileSlot>();
        }

        private void Start()
        {
            // Calculate cell size based on the collider bounds
            var colliderBounds = _boxCollider.bounds;
            _cellSize = new Vector2(colliderBounds.size.x / _tileSlot.tileData.GridSize, colliderBounds.size.y / _tileSlot.tileData.GridSize);
        }
        
        private void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            if (hit.collider.gameObject != _boxCollider.gameObject) return;
            
            var hitPoint = hit.point;
            var (row, col) = WorldToGridPosition(hitPoint);

            if (row >= 0 && row < _tileSlot.tileData.GridSize && col >= 0 && col < _tileSlot.tileData.GridSize)
            {
                HandleHoverEffect(row, col);

                if (Input.GetMouseButtonDown(0))
                {
                    OnCellClicked(row, col);
                }
            }
        }
        
        #endregion

        private void OnCellClicked(int row, int col)
        {
            var cellIndex = row * _tileSlot.tileData.GridSize + col;
            cellSelectedEvent.Raise(cellIndex, cellSelectionOption);
        }

        private void HandleHoverEffect(int row, int col)
        {
            Debug.Log("Hovering over cell: " + row + ", " + col);
        }


        private (int, int) WorldToGridPosition(Vector3 pos)
        {
            // Translate world position to grid position
            var position = _boxCollider.transform.position;
            var row = (int)((pos.x - position.x) / _cellSize.x);
            var col = (int)((pos.y - position.y) / _cellSize.y);

            return (row, col);
        }
        
        
    }
}
