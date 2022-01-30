using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneState { Clean, Sick, Canceled }
public class Zone : MonoBehaviour
{
    public bool zoneStart = false;

    public LayerMask layerZone;//Capa que afectan las zonas
    public LayerMask layerLight;//Capa que almacena las luces
    public ZoneState zoneState = ZoneState.Clean;

    public float contamination; //Grado en que la niebla afecto a la zona
    public bool isSick = false; //Si esta enferma la zona o no
    public float sickCadence; //Velocidad en que suma contaminacion
    public float sickAmount; //Cantidad de contaminacion que suma
    public float sickAmountInfect; //Cantidada de contaminacion que se resta una vez llega a 100
    public float sickProbabilityInfect;
    float sickNextCheck; //Parametro secundario para calcular la cadencia de suma de contaminacion

    public bool illumination; //si hay iluminacion en la zona

    public ParticleSystem fogParticle;
    public Color lightColor;
    public Color fogColor;

    //Items
    public GameObject[] itemsDrop;
    public float[] itemsRate;

    public AudioSource audioZona;
    void Start()
    {
        
    }
    void Update()
    {
        switch (zoneState)
        {
            case ZoneState.Clean:
                if(isSick)
                {
                    zoneState = ZoneState.Sick;
                }
                break;
            case ZoneState.Sick:
                SickState();
                if (illumination)
                {
                    zoneState = ZoneState.Canceled;
                }
                break;
            case ZoneState.Canceled:
                CanceledState();
                break;
            default:
                break;
        }


        if(zoneStart)
        {
            GameManager.Instance.startZonesSick.Add(this);
            zoneStart = false;
        }
    }

    public void GetClean()
    {
        isSick = false;
        zoneState = ZoneState.Clean;
        contamination = 0;
        audioZona.Stop();
        //Debug.Log(transform.name + " SE LIMPIO");
        fogParticle.Stop();
    }
    public void GetLight()
    {
        if(illumination)
        {
            return;
        }
        isSick = false;
        illumination = true;
        var main = fogParticle.main;
        main.startColor = lightColor;
    }
    public void GetSick() //Se enferma por una zona vecina
    {
        if(isSick) //Si esta enferma
        {
            return; //Corta la funcion aqui
        }
        fogParticle.Play();
        audioZona.Play();
        var main = fogParticle.main;
        main.startColor = fogColor;
        //Debug.Log(transform.name + " se enfermo");
        contamination = 30;
        isSick = true; //Enferma modo on
    }

    void Infect() //Accion de infectar a vecinos
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale * 2, 0f, layerZone);
        int i = 0;
        //Debug.Log(hitColliders.Length);
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            float random = Random.Range(1, 100);
            if(random < sickProbabilityInfect) //Es la probabilidad de contagiar a los vecinos
            {
                if(hitColliders[i].GetComponent<Zone>().zoneState != ZoneState.Sick && hitColliders[i].GetComponent<Zone>().zoneState != ZoneState.Canceled)
                SelectEffectSick(hitColliders[i].GetComponent<Zone>());
            }
            //Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }
    }

    void SickState()
    {
        if(illumination)
        {
            return;
        }
        if (contamination >= 100)
        {
            Infect();
            contamination -= sickAmountInfect;
        }
        else
        {
            if (Time.time > sickNextCheck)
            {
                contamination += sickAmount;
                sickNextCheck = Time.time + sickCadence;
            }
        }

    }

    void SelectEffectSick(Zone sick)
    {
        switch (zoneState)
        {
            case ZoneState.Clean:
                break;
            case ZoneState.Sick:
                sick.GetSick();
                break;
            case ZoneState.Canceled:
                break;
            default:
                break;
        }

    }

    public void CanceledState()
    {
        if (contamination <= 0)
        {
            ItemSpawner();
            GetClean();
        }
        else
        {
            if (illumination)
            {
                if (Time.time > sickNextCheck)
                {
                    contamination -= LightPotence();
                    sickNextCheck = Time.time + sickCadence;
                }
            }
            else
            {
                GetSick();
            }
        }
    }

    public void ReceiveLight()
    {
        if(!illumination)
        {
            GetLight();
        }
    }
    public void OutEnergyLight() //Se apaga una luz porque se quedo sin energia
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale * 2, 0f, layerLight);
        int i = 0;
        //Debug.Log("Cantidad de luces: " + hitColliders.Length);
        if (hitColliders.Length > 1)
        {
            return;
        }
        illumination = false;
    }

    public float LightPotence()
    {
        float result = 0f;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale * 2, 0f, layerLight);
        int i = 0;
        //Debug.Log("Cantidad de luces: " + hitColliders.Length);
        if (hitColliders.Length > 0)
        {
            for (int a = 0; a < hitColliders.Length; a++)
            {
                result += hitColliders[a].GetComponent<LightSource>().potence;
            }
        }
        return result;
    }

    void ItemSpawner()
    {
        int random = ItemRandom();
        if(random == 999)
        {
            return;
        }
        Vector2 randomPosition = new Vector2(transform.position.x + Random.Range(2, -2), transform.position.y + Random.Range(2, -2));
        GameObject item = Instantiate(itemsDrop[random], randomPosition, transform.rotation) as GameObject; 
    }

    int ItemRandom()
    {
        int result = 0;
        int random = Random.Range(0, 100);

        if(random < itemsRate[2])
        {
            result = 2;
        }
        else if(random < itemsRate[1])
        {
            result = 1;
        }
        else if (random < itemsRate[0])
        {
            result = 0;
        }
        else
        {
            result = 999;
        }

        return result;
    }
}
