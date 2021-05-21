using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupButton : MonoBehaviour
{
    public UnityEvent onClick;

    public bool interactable;
    private void OnMouseUpAsButton()
    {
        if (interactable)
        {
            onClick.Invoke();
        }
    }

    public void SetEnabled(bool enabled)
    {
        interactable = enabled;
        Color c = GetComponent<SpriteRenderer>().color;
        c.a = enabled ? 1 : 0.3f;
        GetComponent<SpriteRenderer>().color = c;
    }
}
