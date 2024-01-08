using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CellSelection", menuName = "ScriptableObjects/CellSelectionSO", order = 1)]
    public class CellSelection : ScriptableObject
    {
        public CellSelectionOption[] selection;
    }
    
    public enum CellSelectionOption
    {
        Tile1,
        Tile2,
        Neither
    }
}