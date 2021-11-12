using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


public class ControllerScript : MonoBehaviour
{
    GameObject clue;
    GameObject cube;
    Color farClueColor = Color.white;
    Color closeClueColor = Color.red;
    float maxClueDist = 6; // 6 meters, the distance where the clue will be of color farClueColor

    void Awake()
    {
        InteractionManager.InteractionSourceUpdatedLegacy += HandUpdated;
        clue = GameObject.Find("Clue");
        cube = GameObject.Find("Cube");
        clue.transform.position = new Vector3(0, 0, 0);
    }

    private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
    {
        if (state.anyPressed)
        {
            Vector3 pos;
            if (state.sourcePose.TryGetPosition(out pos))
            {
                clue.transform.position = pos;

                float dist = Vector3.Distance(pos, cube.transform.position);
                clue.GetComponent<Renderer>().material.color = Color.Lerp(closeClueColor, farClueColor, dist / maxClueDist);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}