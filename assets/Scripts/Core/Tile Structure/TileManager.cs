using System.Collections.Generic;
using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;

namespace Core.Tile_Structure
{
    public interface ITileManager : IObservable<Tile>
    {
        void SetTileConfig(TileConfig newTileConfig);
        void SetTile(Tile newTile);
        Tile GetTile();
    }

    [RequireComponent(typeof(BoxCollider))]
    public class TileManager : MonoBehaviour, ITileManager
    {
        [SerializeField] private TileConfig tileConfig;
        private readonly List<IObserver<Tile>> _observers = new();
        private Tile _tile;
        private BoxCollider _boxCollider;

        public (int, int) WorldToCellCoordinates(Vector3 worldPosition)
        {
            var thisTransform = transform;
            var localPosition = thisTransform.InverseTransformPoint(worldPosition);
            var cellSize = (_boxCollider.size.x * thisTransform.localScale.x) / _tile.GridSize; // Assume box collider and transform are squares.

            var x = Mathf.FloorToInt(localPosition.x / cellSize);
            var y = Mathf.FloorToInt(localPosition.y / cellSize);

            Debug.Log("Cell Coordinates: (" + x + ", " + y + ")" );
            
            return (x, y);
        }
        
        private void Awake()
        {
            if (tileConfig != null)
            {
                _tile = tileConfig.Create();
            }
            _boxCollider = GetComponent<BoxCollider>();
            NotifyObservers();
        }

        private void OnValidate()
        {
            NotifyObservers();
        }
        
        private void NotifyObservers()
        {
            if (_tile == null) return;
            foreach (var observer in _observers)
            {
                observer.Notify(_tile);
            }
        }
        
        public void SetTileConfig(TileConfig newTileConfig)
        {
            tileConfig = newTileConfig;
            _tile = tileConfig.Create();
        }

        public void SetTile(Tile newTile)
        {
            _tile = newTile;
            NotifyObservers();
        }

        public Tile GetTile()
        {
            return _tile;
        }

        public void RegisterObserver(IObserver<Tile> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void UnregisterObserver(IObserver<Tile> observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }
    }
}