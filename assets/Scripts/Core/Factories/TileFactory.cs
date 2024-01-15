using Core.Tile_Structure;
using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;

namespace Core.Factories
{
    public class TileFactory : MonoBehaviour, ITileFactory
    {
        [SerializeField] private GameObject tilePrefab;
        
        public GameObject CreateTile(TileConfig config)
        {
            var tileObject = Instantiate(tilePrefab);
            var tileManager = tileObject.GetComponent<ITileManager>();
            tileManager.SetTileConfig(config);
            var tilePresenter = tileObject.GetComponent<ITilePresenter>();
            tilePresenter.PresentTile(config.Create());
            return tileObject;
        }
    }

    public interface ITileFactory
    {
        public GameObject CreateTile(TileConfig config);
    }
}