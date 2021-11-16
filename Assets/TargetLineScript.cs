using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class TargetLineScript : MonoBehaviour
{
    public LineRenderer Line;
    public float minimumDistance;

    private bool drawing;

    // Start is called before the first frame update
    void Start()
    {
        drawing = false;
        Line.positionCount = 0;
    }

    void Awake()
    {
        InteractionManager.InteractionSourceUpdatedLegacy += HandUpdated;
    }

    private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
    {
        Vector3 pos;
        if (state.anyPressed)
        {
            if (state.sourcePose.TryGetPosition(out pos))
            {
                if (!drawing)
                {
                    Line.positionCount = 0;
                    Line.SetPosition(0, pos);
                    Line.SetPosition(1, pos);
                    Line.positionCount = 2;
                    drawing = true;
                }

                if (drawing)
                {
                    float distance = Vector3.Distance(pos, Line.GetPosition(Line.positionCount - 1));
                    Line.SetPosition(Line.positionCount - 1, pos);
                    if (distance > minimumDistance)
                    {
                        Line.positionCount++;
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
