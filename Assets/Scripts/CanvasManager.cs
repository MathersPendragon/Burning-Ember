using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public LayerMask interactableLayer;
    public Interactable actualInteraction;
    public RectTransform boxInteraction;
    private Text boxInteractionText;
    private Image boxInteractionImage;
    private Slider boxInteractionSlider;
    private Canvas canvas;
    private Transform playerTransform;

    //Health
    public Slider healthBar;

    //Wife
    public Slider healthWifeBar;

    //Pause
    public GameObject pausePanel;
    bool pauseOn = false;

    //Interaction
    public float workValue = 0;
    public float workCadence = 0.1f;
    float nextCheck;
    public Sprite[] interactionSprites;

    //Object
    public Text[] ResourceText;
    public Button[] LampsButtons;

    //Overlay
    public GameObject overlay;
    public Overlay actualOverlay;
    public Text overlayNameText;
    public Image imageMain;
    public GameObject panelLampara;
    public Text textLampara;
    public Image imageLampara;
    public GameObject panelRecurso;
    public Text textRecurso;

    //Reloj
    public Image relojImage;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1f;
        canvas = GetComponent<Canvas>();
        playerTransform = GameManager.Instance.player.transform;
        boxInteractionText = boxInteraction.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        boxInteractionImage = boxInteraction.transform.GetChild(1).GetComponent<Image>();
        boxInteractionSlider = boxInteraction.transform.GetChild(2).GetComponent<Slider>();

        overlay.SetActive(false);
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        SearchUI();
        CheckWork();



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseOn = !pauseOn;
            pausePanel.SetActive(pauseOn);
            if (pauseOn)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        if (pauseOn)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                //SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                //SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                Application.Quit();
            }
        }
    }
    void LateUpdate()
    {
        SearchInteraction();
    }

    void SearchUI()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Count < 1)
        {
            actualOverlay = null;
            overlay.SetActive(false);
        }
        else
        {
            actualOverlay = results[0].gameObject.GetComponent<Overlay>();
            if (actualOverlay != null)
            {
                ReadOverlay(actualOverlay);
                overlay.SetActive(true);
            }
        }
        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        //foreach (RaycastResult result in results)
        //{
        //Debug.Log("Hit " + result.gameObject.name);
        //}
    }

    #region InteractionBox
    void ShowInteractionBox(Interactable interactable)
    {
        if (interactable.interactionType == InteractionType.Curacion && GameManager.Instance.Inventory.itemAmount[5] < 1)
        {
            HiddenInteractionBox();
            return;
        }

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

    public void HiddenInteractionBox()
    {
        actualInteraction = null;
        boxInteraction.gameObject.SetActive(false);
    }

    void SearchInteraction()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, interactableLayer);
        if (rayHit.collider != null)
        {
            if (rayHit.transform.GetComponent<Interactable>() != null)
            {
                ShowInteractionBox(rayHit.transform.GetComponent<Interactable>());
                if (rayHit.transform.GetComponent<Interactable>().interactionType == InteractionType.Recurso)
                {
                    GameManager.Instance.player.SetTarget(true);
                }
            }else
            {
                GameManager.Instance.player.SetTarget(false);
            }
        }
        else
        {
            if (actualInteraction != null)
            {
                boxInteraction.gameObject.SetActive(false);
                SetWorkValue(0, true);
                actualInteraction = null;
            }
            GameManager.Instance.player.SetTarget(false);
        }
    }
    void CheckWork()
    {
        if (workValue > 0)
        {
            if (Time.time > nextCheck)
            {
                SetWorkValue(-1, false);
                nextCheck = Time.time + workCadence;
            }
        }
    }
    public bool DistanceToInteractable(Interactable interactable)
    {
        bool result = false;
        if(Vector2.Distance(playerTransform.position, interactable.transform.position) < 1f)
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

        if (actualInteraction.barAction)
        {
            SetWorkValue(Random.Range(4, 10), false);
            boxInteractionSlider.maxValue = actualInteraction.barMaxValue;
            if (workValue > actualInteraction.barMaxValue)
            {
                RewardInteraction();
                SetWorkValue(0, true);
            }
        }else
        {
            RewardInteraction();
        }
        if (actualInteraction != null)
        {
            actualInteraction.Effect();
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
    #endregion

    public void ResourceAdd(int value, int id)
    {
        ResourceText[id].text = value.ToString();
    }

    void ReadOverlay(Overlay overlay)
    {
        if(overlay.isLamp)
        {
            panelLampara.SetActive(true);
            panelRecurso.SetActive(false);

            textLampara.text = overlay.ContentText;
            imageLampara.sprite = overlay.spriteResource;

        }
        else
        {
            panelLampara.SetActive(false);
            panelRecurso.SetActive(true);
            textRecurso.text = overlay.ContentText;
        }
        imageMain.sprite = overlay.spriteMain;
        overlayNameText.text = overlay.NameText;
    }

    public void UpdateButtonLamp(int id, bool value)
    {
        LampsButtons[id].interactable = value;
    }

    public void LampButton(int id)
    {
        GameManager.Instance.Inventory.CreateLamp(id);
    }

    public void UpdateHealth(int value)
    {
        healthBar.value = value;
    }

    public void PauseButton()
    {
        pauseOn = !pauseOn;
        pausePanel.SetActive(pauseOn);
        if (pauseOn)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void SetReloj(float time)
    {
        float value = 0;
        float aux = time - 32400;
        if(aux >= 0)
        {
            value = (aux * 100) / 86400;
            value = value / 100;
        }
        else
        {
            aux = aux + 86400;
            value = (aux * 100) / 86400;
            value = value / 100;
        }

        relojImage.fillAmount = value;
    }

    public void ShowWifeHealth(int health, bool active)
    {
        if(active)
        {
            healthWifeBar.value = health;
            healthWifeBar.gameObject.SetActive(active);
        }else
        {
            healthWifeBar.gameObject.SetActive(active);
        }
    }
}
