using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;
using Utils;

namespace Core
{
    [RequireComponent(typeof(Collider))]
    public class TileSlot : MonoBehaviour
    {
        [SerializeField] private TileData tileData;

        private void Awake()
        {
            gameObject.tag = Constants.TileSlotTag;
        }

        public void SetTile(TileData newTileData)
        {
            tileData = newTileData;
        }
    
        public TileData GetTile()
        {
            return tileData;
        }
        
        public void RemoveTile()
        {
            tileData = null;
        }
    }
}
