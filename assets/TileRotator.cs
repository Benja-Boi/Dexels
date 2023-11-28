using UnityEngine;
using System.Collections;

public class TileRotator : MonoBehaviour
{
    public float rotationDuration = 1.0f; // Duration of the rotation
    public float rotationAngle = 90.0f; // Angle to rotate

    private void Start()
    {
        ResetRotation();
    }

    private void ResetRotation()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for a left mouse button click
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    // Rotate the prefab if the hit object is the prefab or one of its children
                    StartCoroutine(RotateTile());
                }
            }
        }
    }

    private IEnumerator RotateTile()
    {
        
        float time = 0.0f;
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + rotationAngle; // Rotate around Z-axis

        while (time < rotationDuration)
        {
            time += Time.deltaTime;
            float angle = Mathf.Lerp(startRotation, endRotation, time / rotationDuration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            yield return null;
        }
    }
}