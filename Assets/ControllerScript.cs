using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


public class ControllerScript : MonoBehaviour
{
    GameObject sphere;

    void Awake()
    {
        InteractionManager.InteractionSourceUpdatedLegacy += HandUpdated;
        sphere = GameObject.Find("Sphere");
        sphere.transform.position = new Vector3(0, 0, 0);
    }

    private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
    {
        if (state.anyPressed)
        {
            Vector3 pos;
            if (state.sourcePose.TryGetPosition(out pos))
            {
                Debug.Log(pos);

                sphere.transform.position = pos;
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