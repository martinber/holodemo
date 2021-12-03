using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class UserLineScript : MonoBehaviour
{
    public LineRenderer Line;
    public float minimumDistance;

    private bool drawing;

    // Start is called before the first frame update
    void Start()
    {
        InteractionManager.InteractionSourceUpdatedLegacy += HandUpdated;

        drawing = false;
        Line.positionCount = 0;
    }

    private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
    {
        Vector3 pos;
        if (state.anyPressed)
        {
            if (state.sourcePose.TryGetPosition(out pos))
            {
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
                        Line.SetPosition(Line.positionCount - 1, pos);
                        float distance = Vector3.Distance(pos, Line.GetPosition(Line.positionCount - 2));

                        Debug.Log(Line.positionCount);
                        Debug.Log(distance);

                        if (distance > minimumDistance)
                        {
                            Line.positionCount++;
                        }
                    }
                }
            }
        }
    }

    public void StartDrawing()
    {
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
        return vertices;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
