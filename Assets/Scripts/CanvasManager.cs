using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public LayerMask interactableLayer;
    private Interactable actualInteraction;
    public RectTransform boxInteraction;
    private Text boxInteractionText;
    private Image boxInteractionImage;
    private Slider boxInteractionSlider;
    public Canvas canvas;

    public float workValue = 0;
    public float workCadence = 0.1f;
    float nextCheck;
    public Sprite[] interactionSprites;
    // Start is called before the first frame update
    void Start()
    {
        boxInteractionText = boxInteraction.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        boxInteractionImage = boxInteraction.transform.GetChild(1).GetComponent<Image>();
        boxInteractionSlider = boxInteraction.transform.GetChild(2).GetComponent<Slider>();
    }

    void Update()
    {
        if(workValue > 0)
        {
            if (Time.time > nextCheck)
            {
                SetWorkValue(-1, false);
                nextCheck = Time.time + workCadence;
            }
        }
    }
    void LateUpdate()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, interactableLayer);
        if (rayHit.collider != null)
        {
            if (rayHit.transform.GetComponent<Interactable>() != null)
            {
                ShowInteractionBox(rayHit.transform.GetComponent<Interactable>());
            }
        }else
        {
            if(actualInteraction != null)
            {
                boxInteraction.gameObject.SetActive(false);
                SetWorkValue(0, true);
                actualInteraction = null;
            }
        }
    }

    void ShowInteractionBox(Interactable interactable)
    {
        if (actualInteraction == null)
        {
            actualInteraction = interactable;
        }

        if (!boxInteraction.gameObject.activeSelf)
        {
            SetWorkValue(0, true);
            boxInteraction.gameObject.SetActive(true);
            boxInteractionText.text = interactable.nameAction;
        }
        if(DistanceToInteractable(interactable))
        {
            boxInteractionImage.sprite = interactionSprites[0];
        }
        else
        {
            boxInteractionImage.sprite = interactionSprites[1];
        }

        boxInteraction.position = Input.mousePosition;
    }

    bool DistanceToInteractable(Interactable interactable)
    {
        bool result = false;
        if(Vector2.Distance(transform.position, interactable.transform.position) < 0.4f)
        {
            result = true;
        }
        return result;
    }

    public void ButtonInteraction()
    {
        if(actualInteraction == null)
        {
            return;
        }
        if(!DistanceToInteractable(actualInteraction))
        {
            return;
        }
        SetWorkValue(Random.Range(4,10), false);
        if(workValue > 99)
        {
            RewardInteraction();
            SetWorkValue(0, true);
        }
    }    

    public void SetWorkValue(int value, bool reemplace)
    {
        if (reemplace)
        {
            workValue = value;
        }
        else
        {
            workValue += value;
        }
        boxInteractionSlider.value = workValue;
    }
    public void RewardInteraction()
    {
        if(actualInteraction != null)
        {
            actualInteraction.Action();
        }
    }
}
