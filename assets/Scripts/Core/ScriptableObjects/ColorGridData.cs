using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Core.ScriptableObjects
{
    [Serializable]
    public class ColorGridData
    {
        [SerializeField] private int gridSize; // Default to 5x5, can be changed in Inspector
        [SerializeField] private int[] colors; // Array to represent grid colors

        public void Initialize(int newGridSize)
        {
            gridSize = newGridSize;
            InitializeColors();
        }
        
        public int GridSize
        {
            get => gridSize;
            set
            {
                if (gridSize == value) return;
                gridSize = value;
                UpdateGridSize();
            }
        }
        
        public void SetColor(int x, int y, int color)
        {
            ValidateCoordinates(x, y);
            colors[y * gridSize + x] = color;
        }
        
        public void SetColor(int i, int color)
        {
            ValidateCoordinates(i);
            colors[i] = color;
        }

        private void ValidateCoordinates(int x, int y)
        {
            if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
            {
                throw new IndexOutOfRangeException($"Coordinates ({x}, {y}) are out of range.");
            }
        }
        
        private void ValidateCoordinates(int i)
        {
            if (i < 0 || i >= gridSize * gridSize)
            {
                throw new IndexOutOfRangeException($"Index {i} is out of range.");
            }
        }

        public void OnEnable()
        {
            InitializeColors();
        }
        
        private void UpdateGridSize()
        {
            if (colors != null && colors.Length == gridSize * gridSize) return; // No update needed
            
            colors ??= new int[gridSize * gridSize]; // If Colors is null, initialize it
            var oldColors = colors;             // Otherwise, save its current contents
            colors = new int[gridSize * gridSize];
            
            var oldGridSize = (int) Math.Sqrt(oldColors.Length);
            var minGridSize = oldGridSize < gridSize ? oldGridSize : gridSize;
            
            // Copy the colors from the old array to the new one
            for (var y=0; y<minGridSize; y++)
            {
                for (var x = 0; x < minGridSize; x++)
                {
                    SetColor(x,y,oldColors[y * oldGridSize + x]);
                }
            }
        }
        
        public int GetColorInt(int x, int y)
        {
            ValidateCoordinates(x, y);
            return colors[y * gridSize + x];
        }
        
        public int GetColorInt(int i)
        {
            ValidateCoordinates(i);
            return colors[i];
        }

        private void InitializeColors()
        {
            if (colors == null)
                Debug.Log("Colors array is null, initializing");

            colors ??= new int[gridSize * gridSize]; 
        }
    }
}