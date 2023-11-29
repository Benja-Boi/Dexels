using System.Collections.Generic;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core
{
    public class TileManager : MonoBehaviour
    {
        private readonly List<ITileManagerObserver> _observers = new();
        public TileData tileData;

        private void Awake()
        {
            NotifyObservers();
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

        public void SetTileData(TileData newTileData)
        {
            tileData = newTileData;
            NotifyObservers();
        }
    }
}