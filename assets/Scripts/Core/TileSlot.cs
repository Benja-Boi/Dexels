using Core.Tile_Structure;
using Obvious.Soap;
using UnityEngine;
using Utils;

namespace Core
{
    [RequireComponent(typeof(Collider))]
    public class TileSlot : MonoBehaviour
    {
        [SerializeField] private ScriptableEventNoParam onTileSet; 
        [SerializeField] private ScriptableEventNoParam onTileRemoved; 
        private Tile _tile;

        private void Awake()
        {
            gameObject.tag = Constants.TileSlotTag;
        }

        public void SetTile(Tile newTile)
        {
            _tile = newTile;
            onTileSet.Raise();
        }
    
        public Tile GetTile()
        {
            return _tile;
        }
        
        public void RemoveTile()
        {
            _tile = null;
            onTileRemoved.Raise();
        }
    }
}
