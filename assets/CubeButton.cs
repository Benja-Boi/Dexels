using Obvious.Soap;
using UnityEngine;

public class CubeButton : MonoBehaviour
{
    [SerializeField] private ScriptableEventNoParam eventToRaise;

    private void OnMouseUp()
    {
        eventToRaise.Raise();
    }
}
