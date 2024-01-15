using UnityEditor;
using UnityEngine;

namespace Core.Tile_Structure.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Tile Config", menuName = "Tile Config")]
    public class TileConfig : ScriptableObject
    {
        [SerializeField] private ColorGridData colorGridData;
        [SerializeField] private ColorMap colorMap;
        [SerializeField] private ColorMap defaultColorMap;

        public Tile Create()
        {
            var tileColorMap = colorMap != null ? colorMap : defaultColorMap;
            return new Tile(colorGridData, tileColorMap);
        }
        
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
}