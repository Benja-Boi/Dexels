using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Color Pool", menuName = "Color Pool")]
    public class ColorPool : ScriptableObject
    {
        public Color[] colors;
    }
}