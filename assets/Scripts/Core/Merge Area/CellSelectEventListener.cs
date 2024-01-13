using Core.Merge_Area.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Merge_Area
{
    public class CellSelectEventListener : MonoBehaviour
    {
        public CellSelectEvent @event;
        public UnityEvent<int, CellSelectionOption> response;

        private void OnEnable()
        {
            @event.RegisterListener(this);
        }

        private void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised(int cellIndex, CellSelectionOption selectionOption)
        {
            response.Invoke(cellIndex, selectionOption);
        }
    }
}