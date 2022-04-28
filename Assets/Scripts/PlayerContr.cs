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
        hit = Physics2D.BoxCast(groundCheckPos, groundCheckSize, 0f, Vector2.down, 0.5f, groundAndEnemyLayer);

        isGrounded = hit.collider != null;

        currentState = CheckState(currentState);
        Debug.Log(currentState);

    }

    private EPlayerStates CheckState(EPlayerStates _currentState)
    {
        switch (_currentState)
        {
            case EPlayerStates.IDLE:
                {
                    Move();
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
                    Move();

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
                    Move();

                    if (CheckEnemy())
                        return EPlayerStates.DEAD;

                    return Jump();
                }
            case EPlayerStates.FALLING:
                {
                    Move();

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
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                rb.velocity = Vector2.up * ReboundForce;
                Destroy(hit.collider.gameObject);
                return true;
            }
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
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheckPos, groundCheckSize);
    }
}
