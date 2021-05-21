using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowAbstract : MonoBehaviour
{
    public virtual void Open()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
