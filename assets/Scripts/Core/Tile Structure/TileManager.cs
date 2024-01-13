using System.Collections.Generic;
using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;

namespace Core.Tile_Structure
{
    [RequireComponent(typeof(BoxCollider))]
    public class TileManager : MonoBehaviour, IObservable<TileData>
    {
        private readonly List<IObserver<TileData>> _observers = new();
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
        
        private void NotifyObservers()
        {
            if (tileData == null) return;
            foreach (var observer in _observers)
            {
                observer.Notify(tileData);
            }
        }

        public void SetTileData(TileData newTileData)
        {
            tileData = newTileData;
            NotifyObservers();
        }

        public TileData GetTileData()
        {
            return tileData;
        }

        public void RegisterObserver(IObserver<TileData> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void UnregisterObserver(IObserver<TileData> observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }
    }
}