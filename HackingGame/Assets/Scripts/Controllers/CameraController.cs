using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private void Awake()
    {
        instance = this;
    }

    public float maxZoomIn;
    float maxZoomOut;

    Camera mainCam;
    private void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    public void SetCameraFieldOfView(Vector2 bottomLeft, Vector2 topRight)
    {
        Vector2 middlePosition = Vector2.Lerp(bottomLeft, topRight, 0.5f);
        transform.position = new Vector3(middlePosition.x, middlePosition.y, transform.position.z);
        float newOrtographicSize = Vector2.Distance(bottomLeft, transform.position);
        mainCam.orthographicSize = (newOrtographicSize < maxZoomIn) ? maxZoomIn : newOrtographicSize;
        maxZoomOut = newOrtographicSize;
        Background.instance.CameraValuesChanged();
    }

    public void MoveCamera(Vector3 moveBy)
    {
        transform.position += moveBy;
    }

    public void SetCameraBackgroundColor(Color color)
    {
        mainCam.backgroundColor = color;
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float newOrtographicSize = mainCam.orthographicSize + Input.mouseScrollDelta.y;
            mainCam.orthographicSize = Mathf.Clamp(newOrtographicSize, maxZoomIn, maxZoomOut);
        }
    }

}
