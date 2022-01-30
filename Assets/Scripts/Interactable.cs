using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType { Recurso, Objeto, Lampara, Curacion }
public class Interactable : MonoBehaviour
{
    public InteractionType interactionType = InteractionType.Objeto;
    public bool barAction = false;
    public float barMaxValue = 100;
    public string nameAction;
    public SpriteRenderer spriteRenderer;
    private BoxCollider2D interactCollider;
    //Recurso
    public GameObject[] recursoObjects;
    public Sprite[] recursoSprites;
    public Transform[] recursoPositions;
    public ParticleSystem[] recursoParticles;

    //Objeto
    public int idObjeto;
    public int amountObjeto;


    public AudioSource soundInteraction;

    // Start is called before the first frame update
    void Start()
    {
        interactCollider = GetComponent<BoxCollider2D>();

        if(interactionType == InteractionType.Objeto)
        {
            Destroy(transform.root.gameObject, 20);
        }
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
                ObjectAction();
                break;
            case InteractionType.Lampara:
                break;
            case InteractionType.Curacion:
                CuracionAction();
                break;
            default:
                break;
        }
        if (soundInteraction != null)
        {
            float pitch = Random.Range(0.8f, 1.1f);
            soundInteraction.pitch = pitch;
            soundInteraction.Play();
        }
    }

    public void Effect()
    {
        switch (interactionType)
        {
            case InteractionType.Recurso:
                recursoParticles[0].Play();
                break;
            case InteractionType.Objeto:
                break;
            case InteractionType.Lampara:
                break;
            case InteractionType.Curacion:
                break;
            default:
                break;
        }
    }

    public void RecursoAction()
    {
        interactCollider.enabled = false;
        spriteRenderer.sprite = recursoSprites[0];
        GameManager.Instance.canvas.HiddenInteractionBox();
        for (int i = 0; i < recursoObjects.Length; i++)
        {
            Vector2 randomOffset = new Vector2(recursoPositions[0].position.x + Random.Range(-1f, 1f), recursoPositions[0].position.y + Random.Range(-1f, 1f));
            GameObject recurso = Instantiate(recursoObjects[i], randomOffset, transform.rotation) as GameObject;
        }
    }

    public void ObjectAction()
    {
        GameManager.Instance.canvas.HiddenInteractionBox();
        GameManager.Instance.Inventory.ItemAdd(idObjeto, amountObjeto);
        transform.root.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke("destroyItem", 1);
    }

    void destroyItem()
    {
        Destroy(transform.root.gameObject);
    }

    public void CuracionAction()
    {
        recursoParticles[0].Play();
        GameManager.Instance.Inventory.ItemAdd(5, -1);
        GameManager.Instance.wife.SetHealth(5);
    }
}
