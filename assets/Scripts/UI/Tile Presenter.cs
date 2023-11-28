using Core;
using Core.ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class TilePresenter : MonoBehaviour, ITileDataObserver
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private MeshRenderer meshRenderer;
        public float rotationSpeed = 100f;

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
            Material material = new Material(Shader.Find("Standard"))
            {
                mainTexture = GenerateCardTexture()
            }; // You can use a different shader if needed
            meshRenderer.material = material;
        }

        Texture2D GenerateCardTexture()
        {
            int gridSize = _tileData.gridSize;
            Texture2D texture = new Texture2D(gridSize, gridSize);

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Color color = _tileData.colors[x + y * gridSize];
                    texture.SetPixel(x, y, color);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.Apply();
            return texture;
        }

        public void OnTileDataChanged(TileData tileData)
        {
            _tileData = tileData;
        }
    }
}
