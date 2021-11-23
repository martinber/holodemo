using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeScript : MonoBehaviour
{
    public float rotationSpeed = 150;
    public GameObject cylinder;
    public ConstantForce force;
    
    void Start()
    {
    }

    public void ActAsFound()
    {
        // Set upward velocity
        GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 0);
        // Destroy after some seconds
        Destroy(gameObject, 2.0f);
        // Enable gravity
        force.enabled = true;
        // Show
        cylinder.GetComponent<Renderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}