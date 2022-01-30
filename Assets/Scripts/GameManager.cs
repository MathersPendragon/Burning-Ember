using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //Global Zone
    public float Dificulty = 1f;
    public Player player;
    public Wife wife;
    public Inventory Inventory;
    public CanvasManager canvas;

    public bool isNight = false;
    public List<Zone> startZonesSick = new List<Zone>();
    public float cadenceSick;
    float nextCheck;

    public int day = 0;

    public AudioSource grillos;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isNight)
        {
            if (Time.time > nextCheck)
            {
                GrupalSick();
                nextCheck = Time.time + cadenceSick;
            }
        }

        if(day >= 4)
        {
            SceneManager.LoadScene("Win", LoadSceneMode.Single);
        }
    }

    public void transitionDay(bool _isNight)
    {
        if (_isNight)
        {
            isNight = true;
            grillos.Play();
        } else
        {
            day += 1;
            Regenerate();
            grillos.Stop();
            isNight = false;
        }
    }

    public void GrupalSick()
    {
        for (int i = 0; i < startZonesSick.Count; i++)
        {
            int random = Random.Range(1, 100);
            if(random < 15)
            {
                startZonesSick[i].GetSick();
            }
        }
    }

    public void Regenerate()
    {
        var zones = FindObjectsOfType<Zone>();

        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].GetClean();
        }

        var lights = FindObjectsOfType<LightSource>();

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].OutEnergy();
        }

        var interactables = FindObjectsOfType<Interactable>();

        for (int i = 0; i < interactables.Length; i++)
        {
            if(interactables[i].interactionType == InteractionType.Objeto)
            {
                Destroy(interactables[i].transform.root);
            }
        }

    }
}
