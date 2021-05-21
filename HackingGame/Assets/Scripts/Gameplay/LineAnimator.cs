using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAnimator : MonoBehaviour
{
    public enum AnimationType {TRACER,HACK,NUKE}

    public static LineAnimator instance;
    private void Awake()
    {
        instance = this;
    }

    public Color hackColor;
    public bool hackShowPercent;
    public int hackSortingOrder;
    public float hackLineWidth;
    public float nukeTime;
    [Space]
    public Color tracerColor;
    public bool tracerShowPercent;
    public int tracerSortingOrder;
    public float tracerLineWidth;
    [Space]
    public GameObject lineTemplate;

    public void AnimateLine(List<Node> nodes, AnimationType animationType)
    {
        GameObject newLineObject = Instantiate(lineTemplate, transform);
        newLineObject.transform.parent = transform;
        LineRenderer line = newLineObject.GetComponent<LineRenderer>();
        line.positionCount = nodes.Count;
        for(int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, nodes[0].transform.position);
        }
        bool showPercent;
        if (animationType==AnimationType.TRACER)
        {
            line.startColor = tracerColor;
            line.endColor = tracerColor;
            line.sortingOrder = tracerSortingOrder;
            line.startWidth = tracerLineWidth;
            line.endWidth = tracerLineWidth;
            showPercent = tracerShowPercent;
        }
        else
        {
            line.startColor = hackColor;
            line.endColor = hackColor;
            line.sortingOrder = hackSortingOrder;
            line.startWidth = hackLineWidth;
            line.endWidth = hackLineWidth;
            showPercent = animationType==AnimationType.HACK ? hackShowPercent : false;
        }
        newLineObject.SetActive(true);
        StartCoroutine(AnimationCorutine(line, nodes,showPercent, animationType));
    }

    IEnumerator AnimationCorutine(LineRenderer line, List<Node> nodes, bool showPercent, AnimationType animationType)
    {
        TextMesh percentage = line.transform.GetChild(0).GetComponentInChildren<TextMesh>();
        percentage.gameObject.SetActive(showPercent);
        for (int i = 1; i < nodes.Count; i++)
        {
            if (animationType == AnimationType.TRACER && nodes[i - 1].isTrap)
            {
                yield return new WaitForSeconds(LevelController.instance.trapDelayTime);
            }
            percentage.transform.position = Vector3.Lerp(nodes[i - 1].transform.position, nodes[i].transform.position, 0.5f);
            float timePassed = 0;
            float time = (animationType!=AnimationType.NUKE) ? nodes[i].difficulty : nukeTime;
            while (timePassed <= time)
            {
                float deltaTime = Time.deltaTime;
                if (animationType==AnimationType.TRACER)
                {
                    deltaTime -= deltaTime * LevelController.instance.spamNodeDecrease*LevelController.spamReached / 100;
                    if (deltaTime<0)
                    {
                        deltaTime = 0;
                    }
                }
                timePassed += deltaTime;
                percentage.text = (int)(timePassed * 100 / time) + "%";
                Vector3 newPosition = Vector3.Lerp(nodes[i-1].transform.position, nodes[i].transform.position, timePassed / time);
                for (int j = i; j < line.positionCount; j++)
                {
                    line.SetPosition(j, newPosition);
                }
                yield return null;
            }
        }
        LevelController.instance.LineRichedDestination(nodes[0], nodes[nodes.Count - 1]);
        percentage.gameObject.SetActive(false);
    }
}


