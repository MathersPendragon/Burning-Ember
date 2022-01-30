using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wife : MonoBehaviour
{
    public int health = 100;
    public float healthCadence = 3;
    float nextCheck;
    private bool inZone = false;
    public bool damageZone = false;

    public Zone zoneWife;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (zoneWife.zoneState == ZoneState.Sick)
        {
            damageZone = true;
        }
        else
        {
            damageZone = false;
        }

        if (damageZone)
        {
            if (Time.time > nextCheck)
            {
                SetHealth(-1);
                nextCheck = Time.time + healthCadence/40;
            }
        }else
        {
            if (Time.time > nextCheck)
            {
                SetHealth(-1);
                nextCheck = Time.time + healthCadence;
            }
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
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
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
        {
            inZone = false;
            ShowHealth(health, false);
        }
    }

    void ShowHealth(int healthValue, bool activeValue)
    {
        GameManager.Instance.canvas.ShowWifeHealth(healthValue, activeValue);
    }
}
