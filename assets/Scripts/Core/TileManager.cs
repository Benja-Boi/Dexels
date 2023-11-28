using System.Collections.Generic;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core
{
    public class TileManager : MonoBehaviour
    {
        private readonly List<ITileDataObserver> _observers = new();
        public TileData tileData;

        private void Start()
        {
            NotifyObservers();
        }

        private void OnValidate()
        {
            NotifyObservers();
        }

        public void RegisterObserver(ITileDataObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void UnregisterObserver(ITileDataObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        private void NotifyObservers()
        {
            if (tileData == null) return;
            foreach (ITileDataObserver observer in _observers)
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