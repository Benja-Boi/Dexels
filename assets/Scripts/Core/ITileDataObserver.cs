using Core.ScriptableObjects;

namespace Core
{
    public interface ITileDataObserver
    {
        void OnTileDataChanged(TileData tileData);
    }
}