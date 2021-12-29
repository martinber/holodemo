using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class TargetLineScript : MonoBehaviour
{
    public LineRenderer Line;
    private string currentTarget = "";

    // Start is called before the first frame updates
    void Start()
    {
        SetSpiral();
    }

    private IEnumerable<(int index, float sin, float cos, float l)> GenerateSpiral(int nPoints, float nLoops, float endRadius, float startRadius)
    {
        for (int index = 0; index < nPoints; index++)
        {
            float l = (float)index / (float)nPoints;

            float angle = 2f * Mathf.PI * l * nLoops;
            float radius = startRadius * (1f - l) + endRadius * l;

            float sin = radius * Mathf.Sin(angle);
            float cos = radius * Mathf.Cos(angle);

            yield return (index, sin, cos, l);
        }
    }

    public void NextTarget()
    {
        if (currentTarget == "spiral")
        {
            SetHorizontalCircle();
        }
        else if (currentTarget == "horizontal circle")
        {
            SetVerticalCircle();
        }
        else if (currentTarget == "vertical circle")
        {
            SetHorizontalLine();
        }
        else
        {
            SetSpiral();
        }
    }

    public void SetSpiral()
    {
        currentTarget = "spiral";
        int nPoints = 1000;
        Line.positionCount = nPoints;

        foreach (var g in GenerateSpiral(nPoints, 5f, 0.5f, 1f))
        {
            float x = g.sin;
            float y = g.l;
            float z = g.cos;
            Line.SetPosition(g.index, new Vector3(x, y, z));
        }
    }

    public void SetVerticalCircle()
    {
        currentTarget = "vertical circle";
        int nPoints = 1000;
        Line.positionCount = nPoints;

        foreach (var g in GenerateSpiral(nPoints, 1f, 1f, 1f))
        {
            float x = g.sin;
            float y = g.cos;
            float z = 0;
            Line.SetPosition(g.index, new Vector3(x, y, z));
        }
    }

    public void SetHorizontalCircle()
    {
        currentTarget = "horizontal circle";
        int nPoints = 1000;
        Line.positionCount = nPoints;

        foreach (var g in GenerateSpiral(nPoints, 1f, 1f, 1f))
        {
            float x = g.sin;
            float y = 0;
            float z = g.cos;
            Line.SetPosition(g.index, new Vector3(x, y, z));
        }
    }

    public void SetHorizontalLine()
    {
        currentTarget = "horizontal line";
        int nPoints = 1000;
        Line.positionCount = nPoints;

        for (int index = 0; index < nPoints; index++)
        {
            float x = (float)index / (float)nPoints;
            float y = 0;
            float z = 0;
            Line.SetPosition(index, new Vector3(x, y, z));
        }
    }

    public Vector3[] GetVertices()
    {
        Vector3[] vertices = new Vector3[Line.positionCount];
        Line.GetPositions(vertices);
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }
        return vertices;
    }
}
