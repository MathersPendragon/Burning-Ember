using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casa : MonoBehaviour
{
    public GameObject[] Techo;

    public AudioSource ambient;
    public AudioClip[] ambientSounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            EnterHome();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ExitHome();
        }
    }


    void EnterHome()
    {
        for (int i = 0; i < Techo.Length; i++)
        {
            Techo[i].SetActive(false);
            GameManager.Instance.player.isWoodSound = true;
            ambient.clip = ambientSounds[1];
            ambient.Play();
        }
    }

    void ExitHome()
    {
        for (int i = 0; i < Techo.Length; i++)
        {
            Techo[i].SetActive(true);
            GameManager.Instance.player.isWoodSound = false;
            ambient.clip = ambientSounds[0];
            ambient.Play();
        }
    }
}
