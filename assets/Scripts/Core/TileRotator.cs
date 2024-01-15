using System.Collections;
using Core.Tile_Structure;
using UnityEngine;

namespace Core
{
    public class TileRotator : MonoBehaviour, IObserver<Tile>
    {
        private ITileManager _tileManager;
    
        public float rotationDuration = 1.0f; // Duration of the rotation
        public float rotationAngle = 90.0f; // Angle to rotate
    
        private bool _isRotating; // Is the prefab currently rotating?
        private Transform _transform;
        private Camera _mainCamera;

        private void OnEnable()
        {
            _tileManager.RegisterObserver(this);
        }

        private void OnDisable()
        {
            _tileManager.UnregisterObserver(this);
        }

        private void Awake()
        {
            _transform = transform;
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            ResetRotation();
        }

        private void ResetRotation()
        {
            //_tileData.currentRotation = RotationState.Up;
            var eulerAngles = _transform.eulerAngles;
            eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0.0f);
            _transform.eulerAngles = eulerAngles;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isRotating) // Check for a left mouse button click
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if (!Physics.Raycast(ray, out var hit)) return; // No hits
                if (hit.transform != transform && !hit.transform.IsChildOf(transform)) return;  // Not the prefab or one of its children
                // Rotate the prefab if the hit object is the prefab or one of its children
                _isRotating = true;
                StartCoroutine(RotateTile());
            }
        }

        private IEnumerator RotateTile()
        {
        
            var time = 0.0f;
            var startRotation = _transform.eulerAngles.z;
            var endRotation = startRotation + rotationAngle; // Rotate around Z-axis

            while (time < rotationDuration)
            {
                time += Time.deltaTime;
                float angle = Mathf.Lerp(startRotation, endRotation, time / rotationDuration);
                var eulerAngles = _transform.eulerAngles;
                eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, angle);
                _transform.eulerAngles = eulerAngles;
                yield return null;
            }
        
            _isRotating = false;
        }

        public void Notify(Tile newTile)
        {
            OnTileDataChanged(newTile);
        }

        private void OnTileDataChanged(Tile tileData)
        { }
    }
}