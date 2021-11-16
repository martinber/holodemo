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
                if (!drawing)
                {
                    Line.positionCount = 2;
                    Line.SetPosition(0, pos);
                    Line.SetPosition(1, pos);
                    drawing = true;
                }

                if (drawing)
                {
                    Line.SetPosition(Line.positionCount - 1, pos);
                    float distance = Vector3.Distance(pos, Line.GetPosition(Line.positionCount - 2));
                    
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
