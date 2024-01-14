using Core.Tile_Structure;
using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;
using Utils;

namespace Core
{
    public class DragDropHandler : MonoBehaviour {
        
        [SerializeField] private TileData draggedTile;
        [SerializeField] private TileSlot originalSlot;
        
        private ITileManager _tileManager;
        private Camera _mainCamera;
        private bool _isDragging;
        private Vector3 _offset;

        private void Awake()
        {
            _tileManager = GetComponent<ITileManager>();
        }

        private void Start() {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!_isDragging) return;
            OnDrag();
        }
        
        private void OnMouseDown()
        {
            OnBeginDrag();
        }

        private void OnMouseUp() {
            if (!_isDragging) return;
            OnEndDrag();
        }


        private void OnBeginDrag()
        {
            _isDragging = true;
            _offset = transform.position - GetMousePos();
            originalSlot = GetComponentInParent<TileSlot>();
            draggedTile = _tileManager.GetTileData();
        }

        private void OnDrag()
        {
            transform.position = _mainCamera.ScreenToWorldPoint(Input.mousePosition) + _offset;
        }

        private void OnEndDrag() {
            _isDragging = false;
            var targetSlot = GetTargetSlot();
            // Tile Slot found and is empty
            if (targetSlot != null  && targetSlot.GetComponent<TileSlot>().GetTile() == null)
            {
                originalSlot.RemoveTile();
                targetSlot.SetTile(draggedTile);
                ResetPosition(targetSlot.transform);
            } // Tile Slot not found
            else {
                originalSlot.SetTile(draggedTile);
                ResetPosition(originalSlot.transform);
            }
        }
        
        private Vector3 GetMousePos() {
            return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        private TileSlot GetTargetSlot() {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var hits = new RaycastHit[10];
            Physics.RaycastNonAlloc(ray, hits);
            foreach (var hit in hits)
            {
                if (hit.collider == null || (!hit.collider.gameObject.CompareTag(Constants.TileSlotTag))) continue;
                return hit.collider.gameObject.GetComponent<TileSlot>();
            }
            return null;
        }

        private void ResetPosition(Transform slotTransform)
        {
            var thisTransform = transform;
            thisTransform.parent = slotTransform;
            thisTransform.position = slotTransform.position;
        }
    }

}
