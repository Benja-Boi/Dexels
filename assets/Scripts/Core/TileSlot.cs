using Core.Tile_Structure.Scriptable_Objects;
using Obvious.Soap;
using UnityEngine;
using Utils;

namespace Core
{
    [RequireComponent(typeof(Collider))]
    public class TileSlot : MonoBehaviour
    {
        [SerializeField]
        private ScriptableEventNoParam onTileDataSet; 
        [SerializeField] private TileData tileData;

        private void Awake()
        {
            gameObject.tag = Constants.TileSlotTag;
        }

        public void SetTile(TileData newTileData)
        {
            tileData = newTileData;
            onTileDataSet.Raise();
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
