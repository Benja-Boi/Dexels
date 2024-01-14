using UnityEditor;
using UnityEngine;

namespace Core.Tile_Structure.Scriptable_Objects
{
    public class TileDataConfig : ScriptableObject
    {
        [SerializeField] private ColorGridData colorGridData;
        [SerializeField] private ColorMap colorMap;
        [SerializeField] private ColorMap defaultColorMap;

        public TileData Create()
        {
            return null;
        }
    }
    
    [CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]
    public class TileData : ScriptableObject
    {
        [SerializeField] private ColorGridData colorGridData;
        [SerializeField] private ColorMap colorMap;
        [SerializeField] private ColorMap defaultColorMap;

        public int GridSize
        {
            get => ColorGridData.GridSize;
            set => ColorGridData.GridSize = value;
        }
        
        public ColorMap ColorMap => colorMap; 
        public ColorGridData ColorGridData => colorGridData;

        public void OnEnable()
        {
            TryInitColorGridData();
            LoadDefaultColorMap();
        }

        private void TryInitColorGridData()
        {
            if (colorGridData != null) return;
            colorGridData = new ColorGridData();
            colorGridData.Initialize(5); // Default size, or retrieve from saved data
        }

        private void LoadDefaultColorMap()
        {
            defaultColorMap = AssetDatabase.LoadAssetAtPath<ColorMap>("Assets/SO Instances/ColorMaps/DefaultColorMap.asset");
            if (defaultColorMap == null)
                Debug.LogError("Default Color Map not found.");
            
            if (colorMap == null)
                colorMap = defaultColorMap;
        }

        public Color GetColor(int x, int y)
        {
            return ColorMap.GetColor(ColorGridData.GetColorInt(x, y));
        }
    }
    
    
    public enum RotationState
    {
        Up,
        Down,
        Right,
        Left
    }

    public static class RotationStateMethods
    {
        public static RotationState GetNextRotationState(RotationState state)
        {
            return state switch
            {
                RotationState.Up => RotationState.Right,
                RotationState.Right => RotationState.Down,
                RotationState.Down => RotationState.Left,
                RotationState.Left => RotationState.Up,
                _ => RotationState.Up
            };
        }
    } 
}