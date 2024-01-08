using Core.ScriptableObjects;

namespace Core
{
    public interface ITileManagerObserver
    {
        void OnTileDataChanged(TileData tileData);
    }
}