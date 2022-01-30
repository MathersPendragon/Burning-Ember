using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState { Idle, Move, Attack }
public class Player : MonoBehaviour
{
    //STATE
    public int health = 100;
    public bool damage = false;
    public float healthCadence;
    float nextCheck;


    //MOVE
    public PlayerState playerState;
    public float moveSpeed = 5f;
    private Rigidbody2D rb2d;
    Vector2 movement;
    Vector2 animDirection;
    private Animator anim;
    private bool isAttack = false;
    private bool isTarget = false;
    private Vector2 targetDirection;


    public AudioSource soundHacha;

    public AudioSource soundPasos;
    private AudioClip pasoAnterior;
    public bool isWoodSound = false;
    public AudioClip[] stepWood;
    public AudioClip[] stepGrass;
    public float cadenceStep;
    float nextCheckStep;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isTarget && GameManager.Instance.canvas.DistanceToInteractable(GameManager.Instance.canvas.actualInteraction))
        {
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            movement = Vector2.zero;
            anim.SetBool("Run", false);
            playerState = PlayerState.Attack;
        }

        if(damage)
        {
            if (Time.time > nextCheck)
            {
                SetHealth(-1);
                nextCheck = Time.time + healthCadence;
            }
        }else
        {
            if (Time.time > nextCheck)
            {
                SetHealth(1);
                nextCheck = Time.time + healthCadence/2;
            }
        }

        switch (playerState)
        {
            case PlayerState.Idle:
                if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    if (!isAttack)
                    {
                        anim.SetBool("Run", true);
                        playerState = PlayerState.Move;
                    }
                }
                break;
            case PlayerState.Move:
                if (movement == Vector2.zero)
                {
                    anim.SetBool("Run", false);
                    playerState = PlayerState.Idle;
                }
                MoveState();
                break;
            case PlayerState.Attack:
                if(!isAttack)
                {
                    Attack();
                    isAttack = true;

                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetDirection = mousePosition - transform.position;
                    anim.SetFloat("Vertical", targetDirection.y);
                    anim.SetFloat("Horizontal", targetDirection.x);
                }

                break;
            default:
                break;
        }


    }
    void FixedUpdate()
    {
        if (!isAttack)
        {
            rb2d.MovePosition(rb2d.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void MoveState()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.y != 0 || movement.x != 0)
        {
            animDirection = movement;

            if (Time.time > nextCheckStep)
            {
                PlayStep();
                nextCheckStep = Time.time + cadenceStep;
            }
        }

        anim.SetFloat("Vertical", animDirection.y);
        anim.SetFloat("Horizontal", animDirection.x);
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public void StopAttack()
    {
        playerState = PlayerState.Idle;
        isAttack = false;
    }

    public void SetTarget(bool value)
    {
        isTarget = value;
    }

    void SetHealth(int value)
    {
        health += value;

        if(health > 99)
        {
            health = 100;
        }

        if(health < 1)
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }

        GameManager.Instance.canvas.UpdateHealth(health);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Zone>() != null)
        {
            if (collision.GetComponent<Zone>().zoneState != ZoneState.Clean)
            {
                damage = true;
            }
            else
            {
                damage = false;
            }
        }else
        {
            damage = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Zone>() != null)
        {
            damage = false;
        }
    }

    public void PlayAxe()
    {
        float pitch = Random.Range(0.75f, 1.25f);
        soundHacha.pitch = pitch;
        soundHacha.Play();
    }

    public void PlayStep()
    {
        AudioClip[] sounds = stepGrass;
        if(isWoodSound)
        {
            sounds = stepWood;
        }
        float pitch = Random.Range(0.8f, 1.1f);
        soundPasos.pitch = pitch;
        int n = Random.Range(1, sounds.Length);
        pasoAnterior = sounds[n];
        soundPasos.clip = pasoAnterior;
        sounds[n] = sounds[0];
        sounds[0] = pasoAnterior;
        soundPasos.Play();
    }
}
