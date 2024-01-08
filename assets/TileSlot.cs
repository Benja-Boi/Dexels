using Core.ScriptableObjects;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    public TileData tileData;
    
    public void PlaceTile(TileData newTileData)
    {
        tileData = newTileData;
    }
    
    public void RemoveTile()
    {
        tileData = null;
    }
}
