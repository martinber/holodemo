using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueScript : MonoBehaviour
{
    public ParticleSystem continuousParticleSystem;
    private ParticleSystem.MainModule continuousParticleSystemMain;
    public ParticleSystem burstParticleSystem;
    private ParticleSystem.MainModule burstParticleSystemMain;

    bool powerReady = false;
    public float powerCooldown;

    public Color normalColor;
    public Color powerColor;

    // Start is called before the first frame update
    void Start()
    {
        continuousParticleSystemMain = continuousParticleSystem.main;
        burstParticleSystemMain = burstParticleSystem.main;

        continuousParticleSystemMain.startColor = normalColor;
        burstParticleSystemMain.startColor = powerColor;

        CooldownStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePower()
    {
        if (powerReady)
        {
            burstParticleSystem.Play();
            CooldownStart();
        }
    }

    public void CooldownStart()
    {
        StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        powerReady = false;
        continuousParticleSystemMain.startColor = normalColor;

        yield return new WaitForSeconds(powerCooldown);

        powerReady = true;
        burstParticleSystemMain.startColor = powerColor;
    }
}
    
