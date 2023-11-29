using System;
using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]
    public class TileData : ScriptableObject
    {
        public ColorPool colorPool; // Reference to a color pool
        public int gridSize; // Default to 3x3, can be changed in Inspector
        public Color[] colors; // Flat array to represent grid colors
        public RotationState currentRotation; // Rotation state of the tile
        public RotationState baseRotation = RotationState.Up; // Rotation state of the tile

        private void OnValidate()
        {
            Debug.Log("On Validate");
            Debug.Log("Colors length: " + colors.Length);
            Debug.Log("Grid Size: " + gridSize);
            
            if (colors == null || colors.Length != gridSize * gridSize)
            {
                Debug.Log("Updated grid size");
                UpdateGridSize();
            }
        }

        private void UpdateGridSize()
        {
            var oldColors = colors;
            colors = new Color[gridSize * gridSize];
            
            var colorsLength = (int) Math.Sqrt(colors.Length);
            var minGridSize = colorsLength < gridSize ? colorsLength : gridSize;
            
            for (int i=0; i<minGridSize*minGridSize; i++)
            {
                colors[i] = oldColors[i];
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