using Core.Tile_Structure;
using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;
using Utils;

namespace Core
{
    public class DragDropHandler : MonoBehaviour {
        
        [SerializeField] private TileManager tileManager;
        [SerializeField] private TileData draggedTile;
        [SerializeField] private TileSlot originalSlot;
        
        private Camera _mainCamera;
        private bool _isDragging;
        private Vector3 _offset;

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
            Debug.Log("Dragging");
            OnBeginDrag();
        }

        private void OnMouseUp() {
            if (!_isDragging) return;
            OnEndDrag();
        }


        private void OnBeginDrag()
        {
            _isDragging = true;
            _offset = transform.position - GetMouseWorldPos();
            originalSlot = GetComponentInParent<TileSlot>();
            draggedTile = tileManager.GetTileData();
        }

        private void OnDrag()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            transform.position = ray.GetPoint(GetDistanceToCamera()) + _offset;
        }

        private void OnEndDrag() {
            _isDragging = false;
            var targetSlot = GetTargetSlot();
            // Tile Slot found and is empty
            if (targetSlot != null && targetSlot != originalSlot && targetSlot.GetComponent<TileSlot>().GetTile() == null)
            { 
                targetSlot.SetTile(draggedTile);
                ResetPosition(targetSlot.transform);
            } // Tile Slot not found
            else {
                originalSlot.SetTile(draggedTile);
                ResetPosition(originalSlot.transform);
            }
        }
        
        private Vector3 GetMouseWorldPos() {
            var mousePoint = Input.mousePosition;
            mousePoint.z = GetDistanceToCamera();
            return _mainCamera.ScreenToWorldPoint(mousePoint);
        }

        private float GetDistanceToCamera() {
            return Vector3.Distance(transform.position, _mainCamera.transform.position);
        }
        private TileSlot GetTargetSlot() {
            var ray = _mainCamera.ScreenPointToRay(GetMouseWorldPos());
            RaycastHit[] hits = { };
            Physics.RaycastNonAlloc(ray, hits);
            foreach (var hit in hits) {
                if (hit.collider.gameObject.CompareTag(Constants.TileSlotTag)) {
                    return hit.collider.gameObject.GetComponent<TileSlot>();
                }
            }
            return null;
        }

        private void ResetPosition(Transform slotTransform)
        {
            transform.parent = slotTransform;
            transform.position = slotTransform.position;
        }
    }

}
