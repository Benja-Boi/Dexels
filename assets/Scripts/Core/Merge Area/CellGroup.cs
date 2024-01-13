using System.Collections.Generic;
using UnityEngine;

namespace Core.Merge_Area
{
    public class CellGroup
    {
        public List<(int, int)> Indices;
        public Color Color;
        public int ColorIndex;

        public CellGroup(List<(int, int)> indices, Color color, int colorIndex)
        {
            Indices = indices;
            Color = color;
            ColorIndex = colorIndex;
        }
    }
}