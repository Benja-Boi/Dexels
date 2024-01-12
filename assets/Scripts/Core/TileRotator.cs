using System.Collections;
using Core.ScriptableObjects;
using Core.Tile_Structure;
using UnityEngine;

namespace Core
{
    public class TileRotator : MonoBehaviour, ITileManagerObserver
    {
        [SerializeField] private TileManager tileManager;
    
        public float rotationDuration = 1.0f; // Duration of the rotation
        public float rotationAngle = 90.0f; // Angle to rotate
    
        private bool _isRotating; // Is the prefab currently rotating?
        private Transform _transform;

        private void OnEnable()
        {
            tileManager.RegisterObserver(this);
        }

        private void OnDisable()
        {
            tileManager.UnregisterObserver(this);
        }

        private void Awake()
        {
            _transform = transform;
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
                Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);   // @TODO: Camera may be null

                if (!Physics.Raycast(ray, out var hit)) return; // No hits
                if (hit.transform != transform && !hit.transform.IsChildOf(transform)) return;  // Not the prefab or one of its children
                // Rotate the prefab if the hit object is the prefab or one of its children
                _isRotating = true;
                StartCoroutine(RotateTile());
            }
        }

        private IEnumerator RotateTile()
        {
        
            float time = 0.0f;
            float startRotation = _transform.eulerAngles.z;
            float endRotation = startRotation + rotationAngle; // Rotate around Z-axis

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

        public void OnTileDataChanged(TileData tileData)
        {
        }
    }
}