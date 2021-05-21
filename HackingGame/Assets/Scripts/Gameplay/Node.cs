using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public enum NodeType { START_NODE, TREASURE_NODE, FIREWALL_NODE, DATA_NODE, SPAM_NODE }
    public enum InteractionType { OPENED, CLOSED, CLICKABLE }


    public int difficulty;
    public TextMesh difficultyText;
    public TextMesh tracePercentageText;
    public SpriteRenderer frame;
    public GameObject trap;

    public NodeType nodeType;
    public InteractionType interactionType;
    public bool isNuked;
    public bool isTrap;

    public List<NodeConnectionHelpler> connections;

    private SpriteRenderer spriteRenderer;

    public void DoSetup()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch (nodeType)
        {
            case NodeType.START_NODE:
                spriteRenderer.sprite = LevelSpowner.instance.startNodeSprite;
                frame.sprite = LevelSpowner.instance.startNodeSprite;
                spriteRenderer.color = Color.blue;
                break;
            case NodeType.TREASURE_NODE:
                spriteRenderer.sprite = LevelSpowner.instance.treasureNodeSprite;
                frame.sprite = LevelSpowner.instance.treasureNodeSprite;
                spriteRenderer.color = Color.yellow;
                break;
            case NodeType.FIREWALL_NODE:
                spriteRenderer.sprite = LevelSpowner.instance.firewallNodeSprite;
                frame.sprite = LevelSpowner.instance.firewallNodeSprite;
                spriteRenderer.color = Color.red;
                break;
            case NodeType.SPAM_NODE:
                spriteRenderer.sprite = LevelSpowner.instance.spamNodeSprite;
                frame.sprite = LevelSpowner.instance.spamNodeSprite;
                spriteRenderer.color = Color.cyan;
                break;
            default:
                spriteRenderer.sprite = LevelSpowner.instance.dataNodeSprite;
                frame.sprite = LevelSpowner.instance.dataNodeSprite;
                spriteRenderer.color = Color.white;
                break;
        }

        SetDifficulty(Random.Range(1, 10));
        CheckInteractionOnStart();
    }

    public void SetDifficulty(int diff)
    {
        difficulty = diff;
        difficultyText.text = difficulty.ToString();
        tracePercentageText.text = (difficulty * LevelController.instance.tracerPercentagePerDifficulty) + "%";
    }

    public void SetInteraction(InteractionType type)
    {
        interactionType = type;
        Color c = spriteRenderer.color;
        switch (interactionType)
        {
            case InteractionType.OPENED:
                if (nodeType == NodeType.SPAM_NODE)
                {
                    LevelController.spamReached++;
                }
                frame.enabled = true;
                c.a = 1f;
                spriteRenderer.color = c;
                break;
            case InteractionType.CLICKABLE:
                frame.enabled = false;
                c.a = 1f;
                spriteRenderer.color = c;
                break;
            case InteractionType.CLOSED:
                frame.enabled = false;
                c.a = 0.2f;
                spriteRenderer.color = c;
                break;
        }
    }

    public void Reset()
    {
        CheckInteractionOnStart();
        isNuked = false;
        isTrap = false;
        trap.SetActive(false);
    }

    private void CheckInteractionOnStart()
    {
        if (nodeType == NodeType.START_NODE)
        {
            SetInteraction(InteractionType.OPENED);
            return;
        }
        foreach (NodeConnectionHelpler connection in connections)
        {
            if (connection.nodeA.nodeType == NodeType.START_NODE || connection.nodeB.nodeType == NodeType.START_NODE)
            {
                SetInteraction(InteractionType.CLICKABLE);
                return;
            }
        }
        SetInteraction(InteractionType.CLOSED);
    }

    private void AnimateHacking()
    {
        foreach (NodeConnectionHelpler connection in connections)
        {
            Node neighbor = connection.nodeA != this ? connection.nodeA : connection.nodeB;
            if (neighbor.interactionType == InteractionType.OPENED)
            {
                LineAnimator.AnimationType animationType = !isNuked ? LineAnimator.AnimationType.HACK : LineAnimator.AnimationType.NUKE;
                LineAnimator.instance.AnimateLine(new List<Node> { neighbor, this }, animationType);
            }
        }
    }
    private void RefreshConnections()
    {
        foreach (NodeConnectionHelpler connection in connections)
        {
            Node neighbor = connection.nodeA != this ? connection.nodeA : connection.nodeB;
            if (neighbor.interactionType == InteractionType.CLOSED)
            {
                neighbor.SetInteraction(InteractionType.CLICKABLE);
            }
        }
    }
    private void CheckReward()
    {
        if (nodeType==NodeType.TREASURE_NODE)
        {
            int randomNumber = Random.Range(0, 99);
            if (randomNumber<33)
            {
                PlayerPrefsController.instance.AddXp(1000);
            }
            else if (randomNumber < 66)
            {
                PlayerPrefsController.instance.AddNukes(1);
            }
            else if (randomNumber < 99)
            {
                PlayerPrefsController.instance.AddTraps(1);
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        LevelController.instance.SelectNode(this);
    }

    public void HackNode()
    {
        SoundManager.instance.PlaySound("hacking", true);
        AnimateHacking();
    }
    public void HackFinished()
    {
        SoundManager.instance.StopSound("hacking");
        PlayerPrefsController.instance.AddXp(difficulty*10);
        SetInteraction(InteractionType.OPENED);
        RefreshConnections();
        CheckReward();
    }

    public void NukeNode()
    {
        PlayerPrefsController.instance.AddNukes(-1);
        isNuked = true;
        SetInteraction(InteractionType.OPENED);
        AnimateHacking();
        RefreshConnections();
        CheckReward();
    }

    public void TrapNode()
    {
        PlayerPrefsController.instance.AddTraps(-1);
        isTrap = true;
        trap.SetActive(true);
    }


    public Vector2 GetPositionAsVector2()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
}
