using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wife : MonoBehaviour
{
    public int health = 100;
    public float healthCadence = 3;
    float nextCheck;
    private bool inZone = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextCheck)
        {
            SetHealth(-1);
            nextCheck = Time.time + healthCadence;
        }

        if(inZone)
        {
            ShowHealth(health, true);
        }
    }

    public void SetHealth(int value)
    {
        health += value;

        if (health > 99)
        {
            health = 100;
        }

        if (health < 1)
        {
            Debug.Log("GAME OVER");
        }

        //GameManager.Instance.canvas.UpdateHealth(health);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            inZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            inZone = false;
            ShowHealth(health, false);
    }

    void ShowHealth(int healthValue, bool activeValue)
    {
        GameManager.Instance.canvas.ShowWifeHealth(healthValue, activeValue);
    }
}
