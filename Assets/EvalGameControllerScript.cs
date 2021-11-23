using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvalGameControllerScript : MonoBehaviour
{
    public GameObject targetLine;
    public GameObject userLine;

    // Start is called before the first frame update
    void Start()
    {
        targetLine.SetActive(true);
        userLine.SetActive(true);
    }

    void LineCompare()
    {
        Vector3[] line_a;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
