using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueScript : MonoBehaviour
{
    public ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
