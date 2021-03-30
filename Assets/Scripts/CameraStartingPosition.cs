using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStartingPosition : MonoBehaviour
{
    [SerializeField] Vector2 cameraStartingPosition = new Vector2(10, 10);

    void SetCameraPosition()
    {
        transform.position = new Vector3(cameraStartingPosition.x, cameraStartingPosition.y, transform.position.z);
    }

    private void Start()
    {
        SetCameraPosition();
    }
}
