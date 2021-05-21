using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTracer : MonoBehaviour
{

    [System.Serializable]
    public class Path
    {
        public List<Node> path;
        public int time;
    }

    public static bool tracerStarted;

    public static void HackingStarted(Node node)
    {
        if (tracerStarted)
        {
            return;
        }
        float randomFloat = Random.Range(1, 101);
        if (randomFloat <= LevelController.instance.tracerPercentagePerDifficulty*node.difficulty)
        {
            tracerStarted = true;
            StartTracer();
        }
    }

    static Path shortestPath;

    public static void StartTracer()
    {
        SoundManager.instance.PlaySound("alarm", true);
        shortestPath = null;
        List<Node> tracers = LevelSpowner.instance.GetFirewallNodes();
        Node startNode = LevelSpowner.instance.GetStartNode();

        foreach (Node tracer in tracers)
        {
            FindPath(tracer, startNode, new List<Node>());
            LineAnimator.instance.AnimateLine(shortestPath.path, LineAnimator.AnimationType.TRACER);
            shortestPath = null;
        }

    }

    private static void FindPath(Node startNode, Node endNode, List<Node> existingPath)
    {
        existingPath.Add(startNode);
        int time = 0;
        for (int i = 1; i < existingPath.Count; i++)
        {
            time += existingPath[i].difficulty;
        }
        if (shortestPath != null && time > shortestPath.time)
        {
            return;
        }
        foreach (NodeConnectionHelpler connection in startNode.connections)
        {
            Node neighbor = connection.nodeA != startNode ? connection.nodeA : connection.nodeB;
            if (neighbor == endNode)
            {
                existingPath.Add(neighbor);
                time += neighbor.difficulty;
                if (shortestPath == null || time < shortestPath.time)
                {
                    shortestPath = new Path();
                    shortestPath.path = existingPath;
                    shortestPath.time = time;
                }
            }
            else if (!existingPath.Contains(neighbor))
            {
                FindPath(neighbor, endNode, new List<Node>(existingPath));
            }
        }
    }
}
