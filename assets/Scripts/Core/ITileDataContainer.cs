using Core.ScriptableObjects;
using UnityEngine;

namespace Core
{
    public interface ITileDataContainer
    {
        public TileData GetTileData();

        public (int, int) WorldToCellCoordinates(Vector3 worldPosition);
    }
}