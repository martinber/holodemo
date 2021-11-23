using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class HuntGameControllerScript : MonoBehaviour
{
    public GameObject clue;
    public GameObject prizePrefab;
    public Color farClueColor = new Color(1f, 1f, 1f, 0.4f); // Transparent White
    public Color closeClueColor = new Color(1f, 0f, 0f, 0.4f); // Transparent Red
    public float maxClueDist = 6; // 6 meters, the distance where the clue will be of color farClueColor

    private List<GameObject> prizes;
    private GameObject prize = null;
    private PrizeScript prizeScript;

    void Start()
    {
        prizes = new List<GameObject>();
        prizes.Add((GameObject)Instantiate(prizePrefab, new Vector3(1, 0, 2), Quaternion.identity));
        prizes.Add((GameObject)Instantiate(prizePrefab, new Vector3(1, 0, 0), Quaternion.identity));
        prizes.Add((GameObject)Instantiate(prizePrefab, new Vector3(1, 0, -2), Quaternion.identity));
       
        clue.SetActive(true);

        InteractionManager.InteractionSourceUpdatedLegacy += HandUpdated;
        clue.transform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Start tracking a random prize
    /// </summary>
    private void SelectPrize()
    {
        int i = Random.Range(0, prizes.Count);
        prize = prizes[i];
        prizeScript = prize.GetComponent<PrizeScript>();
    }

    private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
    {

        if (prize != null)
        {
            Vector3 pos;
            if (state.sourcePose.TryGetPosition(out pos))
            {
                clue.transform.position = pos;
                
                float dist = Vector3.Distance(pos, prize.transform.position);
                clue.GetComponent<Renderer>().material.color = Color.Lerp(closeClueColor, farClueColor, dist / maxClueDist);

                if (dist < 0.3)
                {
                    prizeScript.ActAsFound();
                }
            }
        }
    }

    void Update()
    {
        if (prize == null)
        {
            SelectPrize();
            //prizeScript.ActAsFound();
        }
        else
        {
            clue.transform.LookAt(prize.transform.position);
        }
    }
}