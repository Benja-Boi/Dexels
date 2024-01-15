using Core.Factories;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils.DependencyInjection
{
    public class Provider : MonoBehaviour, IDependencyProvider
    {
        private TileFactory _tileFactory; 
    
        [Provide]
        public TileFactory ProvideTileFactory()
        {
            return _tileFactory ??= gameObject.GetOrAddComponent<TileFactory>();
        }
    }
}
