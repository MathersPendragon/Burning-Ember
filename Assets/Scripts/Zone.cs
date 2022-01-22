using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneState { Clean, Sick, MegaSick, Canceled }
public class Zone : MonoBehaviour
{
    public LayerMask layerZone;//Capa que afectan las zonas
    public ZoneState zoneState = ZoneState.Clean;

    public float contamination; //Grado en que la niebla afecto a la zona
    public bool isSick = false; //Si esta enferma la zona o no
    public bool isMegaSick = false;
    public float sickCadence; //Velocidad en que suma contaminacion
    public float sickAmount; //Cantidad de contaminacion que suma
    public float sickAmountInfect; //Cantidada de contaminacion que se resta una vez llega a 100
    public float sickProbabilityInfect;
    public float sickProbabilityMegaSick;
    float sickNextCheck; //Parametro secundario para calcular la cadencia de suma de contaminacion

    public float illumination; //Cantidad de iluminacion en la zona

    public ParticleSystem fogParticle;
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
                if (isMegaSick)
                {
                    zoneState = ZoneState.MegaSick;
                }
                break;
            case ZoneState.Sick:
                SickState();
                if (isMegaSick)
                {
                    zoneState = ZoneState.MegaSick;
                }
                break;
            case ZoneState.MegaSick:
                MegaSickState();
                if (isSick)
                {
                    zoneState = ZoneState.Sick;
                }
                break;
            case ZoneState.Canceled:
                break;
            default:
                break;
        }

    }


    public void GetSick() //Se enferma por una zona vecina
    {
        if(isSick) //Si esta enferma
        {
            return; //Corta la funcion aqui
        }
        fogParticle.Play();
        Debug.Log(transform.name + " se enfermo");
        isMegaSick = false;
        isSick = true; //Enferma modo on
    }

    public void GetMegaSick()
    {
        if(isMegaSick)
        {
            return;
        }
        Debug.Log(transform.name + " se Transformo en Mega");
        contamination = 0;
        isSick = false;
        isMegaSick = true;
    }

    void Infect()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale * 2, 0f, layerZone);
        int i = 0;
        Debug.Log(hitColliders.Length);
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            float random = Random.Range(1, 100);
            if(random < sickProbabilityInfect)
            {
                SelectEffectSick(hitColliders[i].GetComponent<Zone>());
            }
            random = Random.Range(1, 100);
            if (random < sickProbabilityMegaSick)
            {
                GetMegaSick();
            }
            Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }
    }

    void MegaInfect()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale * 2, 0f, layerZone);
        int i = 0;
        Debug.Log(hitColliders.Length);
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            float random = Random.Range(1, 100);
            if (random < sickProbabilityInfect)
            {
                SelectEffectSick(hitColliders[i].GetComponent<Zone>());
            }
            if (random < sickProbabilityMegaSick/2)
            {
                GetSick();
            }
            Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }
    }
    void SickState()
    {
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

    void MegaSickState()
    {
        if (contamination >= 100)
        {
            MegaInfect();
            contamination -= sickAmountInfect;
        }
        else
        {
            if (Time.time > sickNextCheck)
            {
                contamination += sickAmount;
                sickNextCheck = Time.time + sickCadence/2;
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
            case ZoneState.MegaSick:
                sick.GetMegaSick();
                break;
            case ZoneState.Canceled:
                break;
            default:
                break;
        }

    }
}
