using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLineScript : MonoBehaviour
{
    public LineRenderer Line;

    // Start is called before the first frame update
    void Start()
    {
        int nPoints = 1000;
        float nLoops = 5f;
        float endRadius = 0.5f;
        float startRadius = 1f;
        Line.positionCount = nPoints;

        for (int index = 0; index < nPoints; index++)
        {
            float i = (float)index / (float)nPoints;

            float angle = 2f * Mathf.PI * i * nLoops;
            float radius = startRadius * (1f - i) + endRadius * i;

            float x = radius * Mathf.Sin(angle);
            float y = i;
            float z = radius * Mathf.Cos(angle);

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
