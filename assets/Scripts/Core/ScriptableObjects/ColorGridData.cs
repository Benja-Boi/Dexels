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

        private void ValidateCoordinates(int x, int y)
        {
            if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
            {
                throw new IndexOutOfRangeException($"Coordinates ({x}, {y}) are out of range.");
            }
        }
        
        public void OnEnable()
        {
            InitializeColors();
        }

        // public CellGroup GetCellGroup(int sourceX, int sourceY)
        // {
        //     // Get the list of cells that are the same color as the cell at x,y and can be reached from it by "flooding"
        //     var sourceColor = colors[sourceY * gridSize + sourceX];
        //
        //     var connectedCells = new List<(int x, int y)>();
        //     var visited = new bool[gridSize, gridSize];
        //     var queue = new Queue<(int s_x, int s_y)>();
        //
        //     queue.Enqueue((sourceX, sourceY));
        //     visited[sourceX, sourceX] = true;
        //
        //     while (queue.Count > 0)
        //     {
        //         var (x, y) = queue.Dequeue();
        //         connectedCells.Add((x, y));
        //
        //         // Check adjacent cells: up, down, left, right
        //         foreach (var (dx, dy) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) })
        //         {
        //             int nx = x + dx, ny = y + dy;
        //
        //             if (IsWithinBounds(nx, ny) && 
        //                 !visited[ny, nx] && 
        //                 colors[ny * gridSize + nx] == sourceColor)
        //             {
        //                 queue.Enqueue((nx, ny));
        //                 visited[ny, nx] = true;
        //             }
        //         }
        //     }
        //
        //     return new CellGroup(connectedCells, colorMap.GetColor(sourceColor), sourceColor);
        // }
        //
        // private bool IsWithinBounds(int x, int y)
        // {
        //     return x >= 0 && y >= 0 && x < gridSize && y < gridSize;
        // }

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

        private void InitializeColors()
        {
            if (colors == null)
                Debug.Log("Colors array is null, initializing");

            colors ??= new int[gridSize * gridSize]; 
        }
    }
}