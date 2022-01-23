using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType { Recurso, Objeto, Lampara, Texto }
public class Interactable : MonoBehaviour
{
    public InteractionType interactionType = InteractionType.Objeto;
    public string nameAction;
    public GameObject RecursoObject;
    public Sprite[] RecursoSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action()
    {
        switch (interactionType)
        {
            case InteractionType.Recurso:
                RecursoAction();
                break;
            case InteractionType.Objeto:
                break;
            case InteractionType.Lampara:
                break;
            case InteractionType.Texto:
                break;
            default:
                break;
        }
    }

    public void RecursoAction()
    {

    }
}
