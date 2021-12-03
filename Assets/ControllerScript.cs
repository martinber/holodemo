using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public GameObject huntGameController;
    public GameObject evalGameController;

    // Start is called before the first frame update
    void Start()
    {
        //huntGameController.SetActive(true);
        evalGameController.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}