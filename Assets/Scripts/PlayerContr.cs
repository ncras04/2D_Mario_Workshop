using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerStates
{
    IDLE,
    WALKING,
    JUMPING,
    FALLING,
    KILLED,
    DEAD,
}

public class PlayerContr : MonoBehaviour
{
    public EPlayerStates CurrentState => currentState;

    public float HorizontalDir => horizontalDirection;

    [SerializeField]
    private float MovementSpeed = 8.0f;
    [SerializeField]
    private float JumpForce = 8.0f;

    [SerializeField]
    private float ReboundForce = 3.0f;

    [SerializeField]
    private float horizontalDirection;

    [SerializeField]
    private float JumpTime;
    private float jumpCounter;


    private Rigidbody2D rb;
    private bool isGrounded;

    private Vector2 groundCheckSize;
    private Vector2 groundCheckPos;
    private float groundCheckPosY;

    [SerializeField]
    private LayerMask groundAndEnemyLayer;

    private RaycastHit2D hit;

    private EPlayerStates currentState;

    private SpriteRenderer sprite;

    [SerializeField]
    private Animator animator;

    public static PlayerContr Player { get; private set; }

    private void Awake()
    {
        if (Player != null)
        {
            Destroy(gameObject);
            return;
        }
        Player = this;
    }

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        groundCheckPosY = tmp.size.y;
        groundCheckSize = new Vector2(tmp.size.x, 0.5f);

        rb = GetComponent<Rigidbody2D>();

        animator.SetTrigger("Idle");
        animator.SetFloat("Death", 1);
    }

    void Update()
    {
        horizontalDirection = Input.GetAxisRaw("Horizontal");


        groundCheckPos = new Vector2(transform.position.x, transform.position.y - groundCheckPosY * 0.5f) * transform.localScale;
        hit = BoxCast.Cast(groundCheckPos, groundCheckSize, 0f, Vector2.down, 0.1f, groundAndEnemyLayer);

        isGrounded = hit.collider;

        if (currentState != EPlayerStates.DEAD)
        {

            if (horizontalDirection > 0)
                sprite.flipX = false;
            else if (horizontalDirection < 0)
                sprite.flipX = true;

            Move();
        }

        currentState = CheckState(currentState);
    }

    private EPlayerStates CheckState(EPlayerStates _currentState)
    {
        switch (_currentState)
        {
            case EPlayerStates.IDLE:
                {
                    animator.SetTrigger("Idle");

                    if (CheckEnemy())
                        return EPlayerStates.KILLED;

                    if (rb.velocity.x != 0)
                    {
                        animator.ResetTrigger("Idle");
                        return EPlayerStates.WALKING;
                    }

                    if (CheckJump())
                    {
                        Jump();
                        animator.ResetTrigger("Idle");
                        return EPlayerStates.JUMPING;
                    }

                    if (!isGrounded)
                    {
                        animator.ResetTrigger("Idle");
                        return EPlayerStates.FALLING;
                    }

                    return EPlayerStates.IDLE;
                }
            case EPlayerStates.WALKING:
                {
                    animator.SetTrigger("Running");

                    if (CheckEnemy())
                    {
                        animator.ResetTrigger("Running");
                        return EPlayerStates.KILLED;
                    }

                    if (CheckJump())
                    {
                        Jump();
                        animator.ResetTrigger("Running");
                        return EPlayerStates.JUMPING;
                    }

                    if (rb.velocity.x != 0)
                    {
                        return EPlayerStates.WALKING;
                    }

                    animator.ResetTrigger("Running");
                    return EPlayerStates.IDLE;
                }
            case EPlayerStates.JUMPING:
                {
                    animator.SetTrigger("Jump");

                    if (CheckEnemy())
                        return EPlayerStates.KILLED;

                    return Jump();
                }
            case EPlayerStates.FALLING:
                {
                    if (isGrounded && !CheckEnemyKill())
                    {
                        if (rb.velocity.x != 0)
                            return EPlayerStates.WALKING;

                        return EPlayerStates.IDLE;
                    }

                    return EPlayerStates.FALLING;
                }
            case EPlayerStates.KILLED:
                {
                    Audio.Manager.StopBGM();
                    Audio.Manager.PlaySound(ESounds.DEATH);
                    animator.SetFloat("Death", 0);
                    return EPlayerStates.DEAD;
                }
            case EPlayerStates.DEAD:
                {
                    return EPlayerStates.DEAD;
                }

            default:
                return EPlayerStates.IDLE;
        }
    }
    private bool CheckEnemy()
    {
        if (hit.collider)
        {
            return hit.collider.CompareTag("Enemy");
        }
        return false;
    }

    private EPlayerStates Jump()
    {
        if (jumpCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            jumpCounter -= Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.ResetTrigger("Jump");
                return EPlayerStates.FALLING;
            }
            return EPlayerStates.JUMPING;
        }

        animator.ResetTrigger("Jump");
        return EPlayerStates.FALLING;
    }

    private bool CheckEnemyKill()
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            hit.collider.GetComponent<EnemyController>().Kill();
            rb.velocity = Vector2.up * ReboundForce;
            Audio.Manager.PlaySound(ESounds.KILLENEMY);
            return true;
        }

        return false;
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontalDirection * MovementSpeed, rb.velocity.y);
    }

    private bool CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpCounter = JumpTime;
            Audio.Manager.PlaySound(ESounds.JUMP);
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (currentState != EPlayerStates.FALLING)
                currentState = EPlayerStates.KILLED;
            else
            {
                collision.collider.GetComponent<EnemyController>().Kill();
                rb.velocity = Vector2.up * ReboundForce;
                Audio.Manager.PlaySound(ESounds.KILLENEMY);
            }
        }

        if (collision.collider.CompareTag("DeathZone"))
                currentState = EPlayerStates.KILLED;

        if (collision.collider.CompareTag("Block"))
        {
            if (currentState == EPlayerStates.JUMPING)
            {
                currentState = EPlayerStates.FALLING;
            }
        }
    }
}
