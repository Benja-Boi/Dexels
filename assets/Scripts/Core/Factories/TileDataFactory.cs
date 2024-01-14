using Core.Tile_Structure.Scriptable_Objects;

namespace Core.Factories
{
    public static class TileDataFactory
    {
        public static TileData CreateTileData(TileDataConfig config)
        {
            return config.Create();
        }
    }
}