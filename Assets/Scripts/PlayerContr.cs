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

    void Start()
    {
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        groundCheckPosY = tmp.size.y;
        groundCheckSize = new Vector2(tmp.size.x, 0.5f);

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalDirection = Input.GetAxisRaw("Horizontal");

        groundCheckPos = new Vector2(transform.position.x, transform.position.y - groundCheckPosY * 0.5f) * transform.localScale;
        hit = BoxCast.Cast(groundCheckPos, groundCheckSize, 0f, Vector2.down, 0.1f, groundAndEnemyLayer);

        isGrounded = hit.collider;

        Move();
        CheckCoin();
        currentState = CheckState(currentState);
    }

    private void CheckCoin()
    {

    }

    private EPlayerStates CheckState(EPlayerStates _currentState)
    {
        switch (_currentState)
        {
            case EPlayerStates.IDLE:
                {
                    if (CheckEnemy())
                        return EPlayerStates.DEAD;

                    if (rb.velocity.x != 0)
                        return EPlayerStates.WALKING;

                    if (CheckJump())
                    {
                        Jump();
                        return EPlayerStates.JUMPING;
                    }

                    if (!isGrounded)
                        return EPlayerStates.FALLING;

                    return EPlayerStates.IDLE;
                }
            case EPlayerStates.WALKING:
                {
                    if (CheckEnemy())
                        return EPlayerStates.DEAD;

                    if (CheckJump())
                    {
                        Jump();
                        return EPlayerStates.JUMPING;
                    }

                    if (rb.velocity.x != 0)
                        return EPlayerStates.WALKING;

                    return EPlayerStates.IDLE;
                }
            case EPlayerStates.JUMPING:
                {
                    if (CheckEnemy())
                        return EPlayerStates.DEAD;

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
            case EPlayerStates.DEAD:
                {
                    Audio.Manager.StopBGM();
                    Audio.Manager.PlaySound(ESounds.DEATH);
                    Destroy(this.gameObject);
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
                return EPlayerStates.FALLING;
            }
            return EPlayerStates.JUMPING;
        }

        return EPlayerStates.FALLING;
    }

    private bool CheckEnemyKill()
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            rb.velocity = Vector2.up * ReboundForce;
            Audio.Manager.PlaySound(ESounds.KILLENEMY);
            Destroy(hit.collider.gameObject);
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
                currentState = EPlayerStates.DEAD;
            else
            {
                rb.velocity = Vector2.up * ReboundForce;
                Audio.Manager.PlaySound(ESounds.KILLENEMY);
                Destroy(hit.collider.gameObject);
            }
        }

        if (collision.collider.CompareTag("DeathZone"))
            if (currentState != EPlayerStates.FALLING)
                currentState = EPlayerStates.DEAD;


    }
}
