using System;
using Debug = UnityEngine.Debug;

namespace Core.ScriptableObjects
{
    [Serializable]
    public class ColorGridData
    {
        private int _gridSize; // Default to 5x5, can be changed in Inspector
        private int[] _colors; // Array to represent grid colors
    
        public ColorGridData(int gridSize)
        {
            _gridSize = gridSize;
            InitializeColors();
        }
        
        public int GridSize
        {
            get => _gridSize;
            set
            {
                if (_gridSize == value) return;
                _gridSize = value;
                UpdateGridSize();
            }
        }
        
        public void SetColor(int x, int y, int color)
        {
            ValidateCoordinates(x, y);
            _colors[y * _gridSize + x] = color;
        }

        private void ValidateCoordinates(int x, int y)
        {
            if (x < 0 || x >= _gridSize || y < 0 || y >= _gridSize)
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
            if (_colors != null && _colors.Length == _gridSize * _gridSize) return; // No update needed
            
            _colors ??= new int[_gridSize * _gridSize]; // If Colors is null, initialize it
            var oldColors = _colors;             // Otherwise, save its current contents
            _colors = new int[_gridSize * _gridSize];
            
            var oldGridSize = (int) Math.Sqrt(oldColors.Length);
            var minGridSize = oldGridSize < _gridSize ? oldGridSize : _gridSize;
            
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
            return _colors[y * _gridSize + x];
        }

        private void InitializeColors()
        {
            if (_colors == null)
                Debug.Log("Colors array is null, initializing");

            _colors ??= new int[_gridSize * _gridSize]; 
        }
    }
}