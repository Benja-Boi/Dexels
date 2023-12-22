using System.Collections.Generic;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(BoxCollider))]
    public class TileManager : MonoBehaviour, ITileDataContainer
    {
        private readonly List<ITileManagerObserver> _observers = new();
        [SerializeField] private TileData tileData;
        private BoxCollider _boxCollider;

        public (int, int) WorldToCellCoordinates(Vector3 worldPosition)
        {
            var thisTransform = transform;
            var localPosition = thisTransform.InverseTransformPoint(worldPosition);
            var cellSize = (_boxCollider.size.x * thisTransform.localScale.x) / tileData.GridSize; // Assume box collider and transform are squares.

            var x = Mathf.FloorToInt(localPosition.x / cellSize);
            var y = Mathf.FloorToInt(localPosition.y / cellSize);

            Debug.Log("Cell Coordinates: (" + x + ", " + y + ")" );
            
            return (x, y);
        }
        
        private void Awake()
        {
            NotifyObservers();
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnValidate()
        {
            NotifyObservers();
        }

        public void RegisterObserver(ITileManagerObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void UnregisterObserver(ITileManagerObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        private void NotifyObservers()
        {
            if (tileData == null) return;
            foreach (ITileManagerObserver observer in _observers)
            {
                observer.OnTileDataChanged(tileData);
            }
        }

        public void SetTileData(TileData tileData)
        {
            this.tileData = tileData;
            NotifyObservers();
        }

        public TileData GetTileData()
        {
            return tileData;
        }
    }
}