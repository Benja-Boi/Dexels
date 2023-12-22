using UnityEditor;
using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]

    public class TileData : ScriptableObject
    {
        [SerializeField] private ColorGridData colorGridData = new ColorGridData(5);
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
            LoadDefaultColorMap();
        }

        private void LoadDefaultColorMap()
        {
            defaultColorMap = AssetDatabase.LoadAssetAtPath<ColorMap>("Assets/SO Instances/DefaultColorMap.asset");
            if (defaultColorMap == null)
            {
                Debug.LogError("Default Color Map not found.");
            }

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