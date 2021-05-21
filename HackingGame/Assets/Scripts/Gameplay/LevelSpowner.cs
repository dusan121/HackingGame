using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpowner : MonoBehaviour
{
    public static LevelSpowner instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject nodeTemplate;
    public GameObject connectionTemplate;
    public Sprite startNodeSprite;
    public Sprite treasureNodeSprite;
    public Sprite firewallNodeSprite;
    public Sprite spamNodeSprite;
    public Sprite dataNodeSprite;


    private List<Node> allnodes;
    private List<NodeConnectionHelpler> connections;
    private List<Vector3> allPositions;

    private Vector2 topRightNode;
    private Vector2 botLeftNode;

    public void SpownNewLevel(int nodeCount)
    {
        DestoryCurrentMap();
        SpownLevel(nodeCount);
    }

    public void SpownLevel(int nodeCount)
    {
        GenereateAllPositions(nodeCount);
        SpownNodes(nodeCount);
        GenerateAllPosibleConnections();
        CheckForIntersections();
        CheckStartNodeConnected();
        ShowConnections();
        SetCameraFieldOfView();
    }

    private void GenereateAllPositions(int nodeCount)
    {
        allPositions = new List<Vector3>();
        int matrixSize = nodeCount;
        if (nodeCount < 10)
        {
            matrixSize = 10;
        }

        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                allPositions.Add(new Vector3(i, j, 0));
            }
        }
    }

    private void SpownNodes(int nodeCount)
    {
        allnodes = new List<Node>();

        topRightNode = Vector2.zero;
        botLeftNode = Vector2.zero;

        int treasureNodeForSpown = LevelController.instance.treasureNodeCount;
        int firewallNodeForSpown = LevelController.instance.firewallNodeCount;
        int spamNodeForSpown = LevelController.instance.spamNodeCount;

        for (int i = 0; i < nodeCount; i++)
        {
            if (i == 0)
            {
                SpownNode(Node.NodeType.START_NODE, i + " Start node ");
            }
            else if (treasureNodeForSpown > 0)
            {
                treasureNodeForSpown--;
                SpownNode(Node.NodeType.TREASURE_NODE, i + " Treasure node ");
            }
            else if (firewallNodeForSpown > 0)
            {
                firewallNodeForSpown--;
                SpownNode(Node.NodeType.FIREWALL_NODE, i + " Firewall node ");
            }
            else if (spamNodeForSpown > 0)
            {
                spamNodeForSpown--;
                SpownNode(Node.NodeType.SPAM_NODE, i + " Spam node ");
            }
            else
            {
                SpownNode(Node.NodeType.DATA_NODE, i + " Data node ");
            }
        }
    }

    private Node SpownNode(Node.NodeType nodeType, string name)
    {
        GameObject newGameObject = Instantiate(nodeTemplate, nodeTemplate.transform.parent);
        newGameObject.name = name;
        int randomIndex = UnityEngine.Random.Range(0, allPositions.Count);
        Vector3 localPosition = allPositions[randomIndex];
        allPositions.RemoveAt(randomIndex);
        newGameObject.transform.localPosition = localPosition;
        Node nodeComponnent = newGameObject.GetComponent<Node>();
        nodeComponnent.nodeType = nodeType;
        allnodes.Add(nodeComponnent);
        newGameObject.SetActive(true);

        topRightNode.x = (newGameObject.transform.position.x > topRightNode.x) ? newGameObject.transform.position.x : topRightNode.x;
        topRightNode.y = (newGameObject.transform.position.y > topRightNode.y) ? newGameObject.transform.position.y : topRightNode.y;
        botLeftNode.x = (newGameObject.transform.position.x < botLeftNode.x) ? newGameObject.transform.position.x : botLeftNode.x;
        botLeftNode.y = (newGameObject.transform.position.y < botLeftNode.y) ? newGameObject.transform.position.y : botLeftNode.y;

        return nodeComponnent;
    }

    private void SetCameraFieldOfView()
    {
        CameraController.instance.SetCameraFieldOfView(botLeftNode, topRightNode);
    }

    private void GenerateAllPosibleConnections()
    {
        connections = new List<NodeConnectionHelpler>();

        for (int i = 0; i < allnodes.Count - 1; i++)
        {
            Transform currentNode = allnodes[i].transform;
            currentNode.GetComponent<BoxCollider2D>().enabled = false;
            Vector2 currentNodeVec2 = new Vector2(currentNode.position.x, currentNode.position.y);
            for (int j = i + 1; j < allnodes.Count; j++)
            {
                if (allnodes[i].nodeType == Node.NodeType.START_NODE && (allnodes[j].nodeType == Node.NodeType.FIREWALL_NODE || allnodes[j].nodeType == Node.NodeType.TREASURE_NODE))
                {
                    continue;
                }
                Transform nextNode = allnodes[j].transform;
                Vector2 nextNodeVec2 = new Vector2(nextNode.position.x, nextNode.position.y);
                RaycastHit2D hit = Physics2D.Raycast(currentNodeVec2, (nextNodeVec2 - currentNodeVec2), 100);
                if (hit.collider != null && hit.collider.transform == nextNode)
                {
                    NodeConnectionHelpler newConnection = new NodeConnectionHelpler(allnodes[i], allnodes[j]);
                    connections.Add(newConnection);
                    allnodes[i].connections.Add(newConnection);
                    allnodes[j].connections.Add(newConnection);
                }

            }
            currentNode.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void GenerateConnections(Node currentNode)
    {
        currentNode.GetComponent<BoxCollider2D>().enabled = false;
        Vector2 currentNodeVec2 = currentNode.GetPositionAsVector2();
        int currentIndex = allnodes.IndexOf(currentNode);
        for (int i = 0; i < allnodes.Count; i++)
        {
            if (currentNode.nodeType == Node.NodeType.START_NODE && (allnodes[i].nodeType == Node.NodeType.FIREWALL_NODE || allnodes[i].nodeType == Node.NodeType.TREASURE_NODE))
            {
                continue;
            }
            Transform nextNode = allnodes[i].transform;
            Vector2 nextNodeVec2 = new Vector2(nextNode.position.x, nextNode.position.y);
            RaycastHit2D hit = Physics2D.Raycast(currentNodeVec2, (nextNodeVec2 - currentNodeVec2), 100);
            if (hit.collider != null && hit.collider.transform == nextNode)
            {
                NodeConnectionHelpler newConnection = new NodeConnectionHelpler(currentNode, allnodes[i]);
                connections.Add(newConnection);
                allnodes[currentIndex].connections.Add(newConnection);
                allnodes[i].connections.Add(newConnection);
            }
        }
        currentNode.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void CheckForIntersections()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            List<NodeConnectionHelpler> interseptedNodeConnections = new List<NodeConnectionHelpler>();
            interseptedNodeConnections.Add(connections[i]);

            for (int j = 0; j < connections.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                if (lineSegmentsIntersect(connections[i].nodeA.GetPositionAsVector2(), connections[i].nodeB.GetPositionAsVector2(), connections[j].nodeA.GetPositionAsVector2(), connections[j].nodeB.GetPositionAsVector2()))
                {
                    interseptedNodeConnections.Add(connections[j]);
                }
            }
            if (interseptedNodeConnections.Count > 1)
            {
                int randomIndex = UnityEngine.Random.Range(0, interseptedNodeConnections.Count);
                for (int j = 0; j < interseptedNodeConnections.Count; j++)
                {
                    if (randomIndex != j)
                    {
                        interseptedNodeConnections[j].nodeA.connections.Remove(interseptedNodeConnections[j]);
                        interseptedNodeConnections[j].nodeB.connections.Remove(interseptedNodeConnections[j]);
                        connections.Remove(interseptedNodeConnections[j]);
                        if (j <= i)
                        {
                            i--;
                        }
                    }
                }
            }
        }
    }

    private void CheckForIntersections(Node node)
    {
        for (int i = 0; i < node.connections.Count; i++)
        {
            List<NodeConnectionHelpler> interseptedNodeConnections = new List<NodeConnectionHelpler>();
            interseptedNodeConnections.Add(connections[i]);

            for (int j = 0; j < connections.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                if (lineSegmentsIntersect(connections[i].nodeA.GetPositionAsVector2(), connections[i].nodeB.GetPositionAsVector2(), connections[j].nodeA.GetPositionAsVector2(), connections[j].nodeB.GetPositionAsVector2()))
                {
                    interseptedNodeConnections.Add(connections[j]);
                }
            }
            if (interseptedNodeConnections.Count > 1)
            {
                for (int j = 0; j < interseptedNodeConnections.Count; j++)
                {
                    interseptedNodeConnections[j].nodeA.connections.Remove(interseptedNodeConnections[j]);
                    interseptedNodeConnections[j].nodeB.connections.Remove(interseptedNodeConnections[j]);
                    connections.Remove(interseptedNodeConnections[j]);
                    if (j <= i)
                    {
                        i--;
                    }
                }
            }
        }
    }

    private bool lineSegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);
        if (d == 0.0f)
        {
            return false;
        }
        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;
        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }
        if (p1 == p3 || p1 == p4 || p2 == p3 || p2 == p4)
        {
            return false;
        }
        return true;
    }

    private void CheckStartNodeConnected()
    {
        Node startNode = GetStartNode();
        if (startNode.connections.Count == 0)
        {
            Debug.Log("RESPOWNING NODE " + startNode + " allPositions=" + allPositions.Count);

            allnodes.Remove(startNode);

            Node node = SpownNode(startNode.nodeType, startNode.gameObject.name);
            GenerateConnections(node);
            CheckForIntersections(node);

            Destroy(startNode.gameObject);
            CheckStartNodeConnected();
            return;
        }
        allPositions.Clear();
    }

    private void ShowConnections()
    {
        foreach (NodeConnectionHelpler conn in connections)
        {
            conn.objectInScene = Instantiate(connectionTemplate, connectionTemplate.transform.parent);
            conn.objectInScene.name = "conn " + conn.nodeA.name + "_" + conn.nodeB.name;
            conn.DrawLine();
        }
        foreach (Node node in allnodes)
        {
            node.DoSetup();
        }
    }

    public void Restart()
    {
        LineAnimator.instance.StopAllCoroutines();
        foreach (Node node in allnodes)
        {
            node.Reset();
        }
        Transform lineHolder = connectionTemplate.transform.parent;
        for (int i = 0; i < lineHolder.childCount; i++)
        {
            if (!lineHolder.GetChild(i).name.Contains("conn") && lineHolder.GetChild(i).gameObject.activeInHierarchy)
            {
                Destroy(lineHolder.GetChild(i).gameObject);
            }
        }

    }

    public void DestoryCurrentMap()
    {
        LineAnimator.instance.StopAllCoroutines();
        if (allnodes==null || allnodes.Count==0)
        {
            return;
        }
        foreach (Node node in allnodes)
        {
            Destroy(node.gameObject);
        }
        allnodes = new List<Node>();
        Transform lineHolder = connectionTemplate.transform.parent;
        for (int i = 0; i < lineHolder.childCount; i++)
        {
            if (lineHolder.GetChild(i).gameObject.activeInHierarchy)
            {
                Destroy(lineHolder.GetChild(i).gameObject);
            }
        }
        connections = new List<NodeConnectionHelpler>();
    }

    public Node GetStartNode()
    {
        foreach (Node node in allnodes)
        {
            if (node.nodeType == Node.NodeType.START_NODE)
            {
                return node;
            }
        }
        return null;
    }

    public List<Node> GetFirewallNodes()
    {
        List<Node> tracers = new List<Node>();
        foreach (Node node in allnodes)
        {
            if (node.nodeType == Node.NodeType.FIREWALL_NODE)
            {
                tracers.Add(node);
            }
        }
        return tracers;
    }
}
