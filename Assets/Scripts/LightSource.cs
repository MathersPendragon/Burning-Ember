using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float energy;
    public float potence;
    public float cadence;
    float nextCheck;
    private Zone actualZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(actualZone != null)
        {
            if(actualZone.zoneState != ZoneState.Clean && actualZone.zoneState != ZoneState.Canceled)
            ZoneDetectedState();
        }
    }

    void ZoneDetectedState()
    {
        if (Time.time > nextCheck)
        {
            energy -= potence;
            nextCheck = Time.time + cadence;
            actualZone.ReceiveLight();
            if(energy < 1)
            {
                energy = 0;
                OutEnergy();
            }
        }
    }

    void OutEnergy()
    {
        actualZone.OutEnergyLight();
        Debug.Log(transform.name + ": Sin Energia");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Zone>() != null)
        {
            actualZone = collision.GetComponent<Zone>();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(actualZone == null)
        {
            if (collision.GetComponent<Zone>() != null)
            {
                actualZone = collision.GetComponent<Zone>();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Zone>() != null)
        {
            actualZone = null;
        }
    }
}
