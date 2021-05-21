using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background instance;
    private void Awake()
    {
        instance = this;
    }

    Camera mainCam;
    Vector3 lastFramePosition;
    BoxCollider2D bacgroundCollider;
    bool interactable;

    private void Start()
    {
        mainCam = Camera.main;
        bacgroundCollider = GetComponent<BoxCollider2D>();
    }

    public void EnableInteraction(bool enabled)
    {
        interactable = enabled;
        bacgroundCollider.enabled = enabled;
    }

    private void OnMouseDown()
    {
        if (interactable)
        {
            LevelController.instance.popUp.HideDialog();
            lastFramePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseDrag()
    {
        if (interactable)
        {
            Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 deltaPosition = lastFramePosition - mousePosition;
            CameraController.instance.MoveCamera(deltaPosition);
            lastFramePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            CameraValuesChanged();
        }
    }

    public void CameraValuesChanged()
    {
        transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, transform.position.z);
        bacgroundCollider.size = new Vector2(Screen.width, Screen.height);
    }
}
