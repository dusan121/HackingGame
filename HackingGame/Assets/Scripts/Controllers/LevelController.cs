using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    private void Awake()
    {
        instance = this;
    }

    public int nodeCount;
    public int treasureNodeCount;
    private int treasureCollected;
    public int firewallNodeCount;
    public int spamNodeCount;
    public float spamNodeDecrease;
    public float trapDelayTime;
    public float tracerPercentagePerDifficulty;

    public Popup popUp;

    public static bool levelCompletedSuccessfully;
    public static bool isHacking;
    public static int spamReached;

    private void Start()
    {
    }

    public void SpownNewLevel()
    {
        LevelSpowner.instance.SpownNewLevel(nodeCount);
        ResetParametars();
        Background.instance.EnableInteraction(true);
    }
    public void Restart()
    {
        LevelSpowner.instance.Restart();
        ResetParametars();
        Background.instance.EnableInteraction(true);
    }
    public void DestroyCurrentLevel()
    {
        LevelSpowner.instance.DestoryCurrentMap();
        ResetParametars();
        Background.instance.EnableInteraction(false);
        SoundManager.instance.StopLevelSounds();
    }

    private void ResetParametars()
    {
        levelCompletedSuccessfully = false;
        spamReached = 0;
        treasureCollected = 0;
        isHacking = false;
        hackingNode = null;
        selectedNode = null;
        PathTracer.tracerStarted = false;
        popUp.HideDialog();
    }

    Node selectedNode;
    Node hackingNode;

    public void LineRichedDestination(Node start, Node end)
    {
        if (start.nodeType == Node.NodeType.FIREWALL_NODE && end.nodeType == Node.NodeType.START_NODE)
        {
            GameOver();
            return;
        }
        else if (isHacking)
        {
            if (hackingNode)
            {
                hackingNode.HackFinished();
            }
            isHacking = false;
            hackingNode = null;
            popUp.CheckInteractions(selectedNode);
        }
        if (end.nodeType == Node.NodeType.TREASURE_NODE)
        {
            treasureCollected++;
            if (treasureCollected == treasureNodeCount)
            {
                LevelCompleted();
            }
        }
    }
    private void GameOver()
    {
        levelCompletedSuccessfully = false;
        LineAnimator.instance.StopAllCoroutines();
        UiManager.instance.OpenWindow(LevelFinishedDialog.instance);
        Background.instance.EnableInteraction(false);
        SoundManager.instance.StopLevelSounds();
    }
    private void LevelCompleted()
    {
        levelCompletedSuccessfully = true;
        LineAnimator.instance.StopAllCoroutines();
        UiManager.instance.OpenWindow(LevelFinishedDialog.instance);
        Background.instance.EnableInteraction(false);
        SoundManager.instance.StopLevelSounds();
    }

    public void SelectNode(Node selected)
    {
        selectedNode = selected;
        popUp.Show(selectedNode);
    }

    public void HackSelectedNode()
    {
        isHacking = true;
        hackingNode = selectedNode;
        selectedNode.HackNode();
        PathTracer.HackingStarted(selectedNode);
        popUp.HideDialog();
    }

    public void NukeSelectedNode()
    {
        selectedNode.NukeNode();
        LineRichedDestination(selectedNode, selectedNode);
        popUp.HideDialog();
    }
    public void TrapSelectedNode()
    {
        selectedNode.TrapNode();
        popUp.HideDialog();
    }



}
