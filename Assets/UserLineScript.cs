using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class UserLineScript : MonoBehaviour
{
    public GameObject controller;
    private ControllerScript controllerScript;
    public LineRenderer Line;
    public float minimumDistance;
    public Vector3 lastPos;

    private bool drawing;

    // Start is called before the first frame update
    void Start()
    {
        controllerScript = controller.GetComponent<ControllerScript>();

        InteractionManager.InteractionSourceUpdatedLegacy += HandUpdated;

        drawing = false;
        Line.positionCount = 0;
    }

    /// <summary>
    /// Draw line if necessary, this method is called when the hand or the wand is detected/
    /// </summary>
    /// <param name="pos"></param>
    private void UpdatePosition(Vector3 pos)
    {
        lastPos = pos;
        if (drawing)
        {
            if (Line.positionCount == 0)
            {
                Line.positionCount = 2;
                Line.SetPosition(0, pos);
                Line.SetPosition(1, pos);
            }
            else
            {
                float distance = Vector3.Distance(pos, Line.GetPosition(Line.positionCount - 2));
                if (distance > minimumDistance)
                {
                    Line.positionCount++;
                }
                Line.SetPosition(Line.positionCount - 1, pos);
            }
        }
    }

    void Update()
    {
        Vector3? wandPos = controllerScript.GetWandPosition();

        if (wandPos is Vector3 pos)
        {
            UpdatePosition(pos);
        }
    }

    private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
    {
        Vector3 pos;
        if (state.anyPressed)
        {
            if (state.sourcePose.TryGetPosition(out pos))
            {
                UpdatePosition(pos);
            }
        }
    }

    public void StartDrawing()
    {
        Line.positionCount = 0;
        drawing = true;
    }

    public void StopDrawing()
    {
        drawing = false;
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
