using System;
using Core.Factories;
using Core.Merge_Area.Scriptable_Objects;
using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;
using Utils.DependencyInjection;

namespace Core.Merge_Area
{
    public class TileMerger : MonoBehaviour
    {
        [SerializeField] private TileSlot tileSlot1;
        [SerializeField] private TileSlot tileSlot2;
        [SerializeField] private TileSlot mergedTileSlot;
        [SerializeField] private CellSelection cellSelection;
        [Inject] private TileFactory _tileFactory;
        public void MergeTiles()
        {
            if (!MergingEnabled())
            {
                Debug.LogWarning("MergeTiles: attempted to merge but conditions are not met.");
                return;
            }
            
            var tileData1 = tileSlot1.GetTile();
            var tileData2 = tileSlot2.GetTile();
            var mergedTileConfig = ScriptableObject.CreateInstance<TileConfig>();
            mergedTileConfig.GridSize = tileData1.GridSize;
            
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
    
                mergedTileConfig.ColorGridData.SetColor(i, cellColor);
            }

            var mergedTile = _tileFactory.CreateTile(mergedTileConfig);
            mergedTileSlot.SetTile(mergedTileConfig.Create());
            mergedTile.transform.SetParent(mergedTileSlot.transform);
            mergedTile.transform.localPosition = Vector3.zero; 
        }

        private bool MergingEnabled()
        {
            if (tileSlot1.GetTile() == null || tileSlot2.GetTile() == null) return false;  // One or more slots missing tiles
            var tileData1 = tileSlot1.GetTile();
            var tileData2 = tileSlot1.GetTile();
            if (tileData1.GridSize != tileData2.GridSize) return false;   // Different grid sizes, can't merge.
            if (tileData1.ColorMap != tileData2.ColorMap) return false;   // Different color map, can't merge.
            return true;
        }
    }
}
