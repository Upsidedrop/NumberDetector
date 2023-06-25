using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineChart : MonoBehaviour
{
    LineRenderer lineRenderer;
    List<float> points = new();
    [SerializeField]
    Train trainScript;
    float localCost;
    readonly int smoothnessFactor = 5;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    IEnumerator FetchDistance()
    {
        yield return new WaitForSeconds(0.1f);
        if (trainScript.isTraining)
        {
            if (points.Count == lineRenderer.positionCount)
            {
                points.RemoveAt(0);
            }
            localCost = trainScript.cost;
            points.Add(localCost);

            for (int i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, new Vector2(i, points[i]));
            }
        }

        StartCoroutine(FetchDistance());
    }
    private void Start()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, new(i, 0));
        }
        StartCoroutine(FetchDistance());
    }
}
