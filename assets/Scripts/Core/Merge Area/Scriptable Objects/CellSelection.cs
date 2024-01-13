using UnityEngine;

namespace Core.Merge_Area.Scriptable_Objects
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