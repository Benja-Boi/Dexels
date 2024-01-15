using Core;
using Core.Tile_Structure;
using UnityEngine;

namespace UI
{
    public class TilePresenter : MonoBehaviour, IObserver<Tile>, ITilePresenter
    {
        [SerializeField] private Renderer tileRenderer;
        private ITileManager _tileManager;
        private Tile _tile;

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
            if (_tile == null) return;
            UpdateTexture();
        }
        
        public void Notify(Tile newTile)
        {
            OnTileDataChanged(newTile);
        }

        private void OnTileDataChanged(Tile tile)
        {
            _tile = tile;
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
            var gridSize = _tile.GridSize;
            var texture = new Texture2D(gridSize, gridSize);

            for (var y = 0; y < gridSize; y++)
            {
                for (var x = 0; x < gridSize; x++)
                {
                    var color = _tile.GetColor(x, y);
                    texture.SetPixel(x, FlipY(y), color);  // Texture coordinates start at bottom left, our 0,0 is top left.
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.Apply();
            return texture;
        }

        private int FlipY(int y)
        {
            return _tile.GridSize - 1 - y;
        }

        public void PresentTile(Tile tile)
        {
            _tile = tile;
        }
    }
}
