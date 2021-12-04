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
    public float trackingWaitTime;
    private float lastUpdateTime;

    public Color normalColor;
    public Color powerColor;

    // Start is called before the first frame update
    void Start()
    {
        continuousParticleSystemMain = continuousParticleSystem.main;
        burstParticleSystemMain = burstParticleSystem.main;

        continuousParticleSystemMain.startColor = normalColor;
        burstParticleSystemMain.startColor = powerColor;

        lastUpdateTime = Time.time;

        CooldownStart();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Time: {Time.time}, {lastUpdateTime}, {lastUpdateTime + trackingWaitTime}");
        if (Time.time > lastUpdateTime + trackingWaitTime)
        {
            TrackingLost();
        }
    }

    public void UpdatePosition(Vector3 pos)
    {
        transform.position = pos;

        if (Time.time > lastUpdateTime + trackingWaitTime)
        {
            TrackingRecovered();
        }
        lastUpdateTime = Time.time;
    }

    public void TrackingLost()
    {
        continuousParticleSystem.Stop();
        burstParticleSystem.Stop();
    }

    public void TrackingRecovered()
    {
        continuousParticleSystem.Play();
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
        continuousParticleSystemMain.startColor = powerColor;
    }
}
    
