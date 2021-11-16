using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class HuntGameControllerScript : MonoBehaviour
{
    public GameObject clue;
    public GameObject cube;
    public Color farClueColor = new Color(1f, 1f, 1f, 0.4f); // Transparent White
    public Color closeClueColor = new Color(1f, 0f, 0f, 0.4f); // Transparent Red
    public float maxClueDist = 6; // 6 meters, the distance where the clue will be of color farClueColor

    void Start()
    {
        clue.SetActive(true);
        cube.SetActive(true);

        InteractionManager.InteractionSourceUpdatedLegacy += HandUpdated;
        clue.transform.position = new Vector3(0, 0, 0);
        cube.GetComponent<Renderer>().enabled = false;
    }

    private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
    {
        Vector3 pos;
        if (state.sourcePose.TryGetPosition(out pos))
        {
            clue.transform.position = pos;

            float dist = Vector3.Distance(pos, cube.transform.position);
            clue.GetComponent<Renderer>().material.color = Color.Lerp(closeClueColor, farClueColor, dist / maxClueDist);

            if (dist < 0.3)
            {
                cube.GetComponent<Renderer>().enabled = true;
            }
        }
    }
}