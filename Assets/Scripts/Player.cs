using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState { Idle, Move, Attack }
public class Player : MonoBehaviour
{
    public PlayerState playerState;
    public float moveSpeed = 5f;
    public Rigidbody2D rb2d;
    Vector2 movement;
    Vector2 animDirection;
    public Animator anim;
    private bool isAttack = false;
    private bool isTarget = false;
    public Vector2 targetDirection;

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
}
