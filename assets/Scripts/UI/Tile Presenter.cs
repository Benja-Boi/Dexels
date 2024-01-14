using System;
using Core;
using Core.Tile_Structure;
using Core.Tile_Structure.Scriptable_Objects;
using UnityEngine;

namespace UI
{
    public class TilePresenter : MonoBehaviour, Core.Tile_Structure.IObserver<TileData>, ITilePresenter
    {
        [SerializeField] private Renderer tileRenderer;
        private ITileManager _tileManager;
        private TileData _tileData;

        private void Awake()
        {
            _tileManager = GetComponent<ITileManager>();
        }

        private void OnEnable()
        {
            _tileManager.RegisterObserver(this);
        }

        private void OnDisable()
        {
            _tileManager.UnregisterObserver(this);
        }

        public void FixedUpdate()
        {
            if (_tileData == null) return;
            UpdateTexture();
        }
        
        public void Notify(TileData newTileData)
        {
            OnTileDataChanged(newTileData);
        }

        private void OnTileDataChanged(TileData tileData)
        {
            _tileData = tileData;
        }
        
        private void UpdateTexture()
        {
            var material = new Material(Shader.Find("Standard"))
            {
                mainTexture = GenerateCardTexture()
            };
            tileRenderer.material = material;
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

        public void PresentTile(TileData tileData)
        {
            _tileData = tileData;
        }
    }
}
