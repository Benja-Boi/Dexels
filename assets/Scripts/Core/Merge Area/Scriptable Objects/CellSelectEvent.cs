using System.Collections.Generic;
using UnityEngine;

namespace Core.Merge_Area.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New CellSelectEvent", menuName = "Event/CellSelectEvent")]
    public class CellSelectEvent : ScriptableObject
    {
        private readonly List<CellSelectEventListener> _listeners = new();

        public void Raise(int cellIndex, CellSelectionOption selectionOption)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(cellIndex, selectionOption);
            }
        }

        public void RegisterListener(CellSelectEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(CellSelectEventListener listener)
        {
            _listeners.Remove(listener);
        }
    }
}