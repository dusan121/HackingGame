using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public PopupButton nukeBtn, trapBtn, hackBtn;

    public void Show(Node selectedNode)
    {
        transform.position = selectedNode.transform.position;
        CheckInteractions(selectedNode);
        gameObject.SetActive(true);
    }
    public void HideDialog()
    {
        gameObject.SetActive(false);
    }

    public void UseNuke()
    {
        LevelController.instance.NukeSelectedNode();
    }
    public void UseTrap()
    {
        LevelController.instance.TrapSelectedNode();
    }
    public void Hack()
    {
        LevelController.instance.HackSelectedNode();
    }


    public void CheckInteractions(Node selectedNode)
    {
        nukeBtn.GetComponent<PopupButton>().SetEnabled(PlayerPrefs.GetInt("nukes") > 0);
        trapBtn.GetComponent<PopupButton>().SetEnabled(PlayerPrefs.GetInt("traps") > 0);
        hackBtn.GetComponent<PopupButton>().SetEnabled(selectedNode.interactionType == Node.InteractionType.CLICKABLE && !LevelController.isHacking);
    }
}
