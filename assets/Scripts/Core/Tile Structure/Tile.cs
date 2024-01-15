using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;

namespace Core.Tile_Structure
{
    public class Tile
    {
        private ColorGridData _colorGridData;
        private ColorMap _colorMap;

        public Tile(ColorGridData colorGridData, ColorMap colorMap)
        {
            _colorGridData = colorGridData;
            _colorMap = colorMap;
        }
        
        public int GridSize
        {
            get => ColorGridData.GridSize;
            set => ColorGridData.GridSize = value;
        }
        
        public ColorMap ColorMap => _colorMap; 
        public ColorGridData ColorGridData => _colorGridData;

        public Color GetColor(int x, int y)
        {
            return ColorMap.GetColor(ColorGridData.GetColorInt(x, y));
        }
    }
}