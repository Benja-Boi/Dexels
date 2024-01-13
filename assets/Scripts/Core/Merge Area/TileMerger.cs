using System;
using Core.Merge_Area.Scriptable_Objects;
using Core.Tile_Structure;
using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;

namespace Core.Merge_Area
{
    public class TileMerger : MonoBehaviour
    {
        [SerializeField] private TileData tileData1;
        [SerializeField] private TileData tileData2;
        [SerializeField] private TileManager mergedTileManager;
        [SerializeField] private CellSelection cellSelection;

        public void MergeTiles()
        {
            Debug.Log("Merging Tiles!!");
            
            if (tileData1.GridSize != tileData2.GridSize) return;   // Different grid sizes, can't merge.
            if (tileData1.ColorMap != tileData2.ColorMap) return;   // Different color map, can't merge.
            var mergedTileData = ScriptableObject.CreateInstance<TileData>();
            mergedTileData.GridSize = tileData1.GridSize;
            
            // For each cell, select a color from tileData1, tileData2, or neither.
            for (var i = 0; i < cellSelection.selection.Length; i++)
            {
                var cellColor = cellSelection.selection[i] switch
                {
                    CellSelectionOption.Tile1 => tileData1.ColorGridData.GetColorInt(i),
                    CellSelectionOption.Tile2 => tileData2.ColorGridData.GetColorInt(i),
                    CellSelectionOption.Neither => tileData1.ColorMap.GetRandomColorInt(),
                    _ => throw new ArgumentOutOfRangeException()
                };
    
                mergedTileData.ColorGridData.SetColor(i, cellColor);
            }
            
            mergedTileManager.SetTileData(mergedTileData);
        }
    }
}
