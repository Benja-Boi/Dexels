using System.Collections.Generic;
using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New CellSelectEvent", menuName = "Event/CellSelectEvent")]
    public class CellSelectEvent : ScriptableObject
    {
        private List<CellSelectEventListener> listeners = new List<CellSelectEventListener>();

        public void Raise(int cellIndex, CellSelectionOption selectionOption)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(cellIndex, selectionOption);
            }
        }

        public void RegisterListener(CellSelectEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(CellSelectEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}