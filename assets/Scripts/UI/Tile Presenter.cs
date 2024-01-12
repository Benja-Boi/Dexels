using Core;
using Core.ScriptableObjects;
using Core.Tile_Structure;
using UnityEngine;

namespace UI
{
    public class TilePresenter : MonoBehaviour, ITileManagerObserver
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private MeshRenderer meshRenderer;

        private TileData _tileData;
        
        private void OnEnable()
        {
            tileManager.RegisterObserver(this);
        }

        private void OnDisable()
        {
            tileManager.UnregisterObserver(this);
        }

        public void FixedUpdate()
        {
            if (_tileData == null) return;
            UpdateTexture();
        }

        public void OnTileDataChanged(TileData tileData)
        {
            _tileData = tileData;
        }
        
        private void UpdateTexture()
        {
            var material = new Material(Shader.Find("Standard"))
            {
                mainTexture = GenerateCardTexture()
            };
            meshRenderer.material = material;
        }

        private Texture2D GenerateCardTexture()
        {
            var gridSize = _tileData.GridSize;
            var texture = new Texture2D(gridSize, gridSize);

            for (var y = 0; y < gridSize; y++)
            {
                for (var x = 0; x < gridSize; x++)
                {
                    var color = _tileData.GetColor(x, y);
                    texture.SetPixel(x, FlipY(y), color);  // Texture coordinates start at bottom left, our 0,0 is top left.
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.Apply();
            return texture;
        }

        private int FlipY(int y)
        {
            return _tileData.GridSize - 1 - y;
        }
    }
}
