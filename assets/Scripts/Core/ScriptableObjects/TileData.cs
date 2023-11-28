using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]
    public class TileData : ScriptableObject
    {
        public ColorPool colorPool; // Reference to a color pool
        public int gridSize = 3; // Default to 3x3, can be changed in Inspector
        public Color[] colors; // Flat array to represent grid colors
        public RotationState currentRotation; // Rotation state of the tile
        public RotationState baseRotation = RotationState.Up; // Rotation state of the tile

        private void OnValidate()
        {
            if (colors == null || colors.Length != gridSize * gridSize)
            {
                colors = new Color[gridSize * gridSize];
            }
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
            switch (state)
            {
                case RotationState.Up:
                    return RotationState.Right;
                case RotationState.Right:
                    return RotationState.Down;
                case RotationState.Down:
                    return RotationState.Left;
                case RotationState.Left:
                    return RotationState.Up;
                default:
                    return RotationState.Up;
            }
        }
    } 
}