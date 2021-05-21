using UnityEngine;

[System.Serializable]
public class NodeConnectionHelpler
{
    public Node nodeA;
    public Node nodeB;
    public GameObject objectInScene;

    private LineRenderer lineRendererComp;

    public NodeConnectionHelpler(Node nodeA, Node nodeB)
    {
        this.nodeA = nodeA;
        this.nodeB = nodeB;
    }

    public void DrawLine()
    {
        lineRendererComp = objectInScene.GetComponent<LineRenderer>();
        lineRendererComp.SetPosition(0, nodeA.transform.position);
        lineRendererComp.SetPosition(1, nodeB.transform.position);
        objectInScene.SetActive(true);
    }

    public void SetColor(Color color)
    {
        lineRendererComp = objectInScene.GetComponent<LineRenderer>();
        lineRendererComp.startColor = color;
        lineRendererComp.endColor = color;
    }
}
