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

    private float xVelocity;

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
        hit = BoxCast(groundCheckPos, groundCheckSize, 0f, Vector2.down, 0.1f, groundAndEnemyLayer);

        isGrounded = hit.collider;

        currentState = CheckState(currentState);
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
        if (hit.collider.CompareTag("Enemy"))
        {
            rb.velocity = Vector2.up * ReboundForce;
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
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {

        // Gizmos.DrawWireCube(groundCheckPos, groundCheckSize);
    }

    static public RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int mask)
    {
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, angle, direction, distance, mask);

        //Setting up the points to draw the cast
        Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
        float w = size.x * 0.5f;
        float h = size.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += origin;
        p2 += origin;
        p3 += origin;
        p4 += origin;

        Vector2 realDistance = direction.normalized * distance;
        p5 = p1 + realDistance;
        p6 = p2 + realDistance;
        p7 = p3 + realDistance;
        p8 = p4 + realDistance;


        //Drawing the cast
        Color castColor = hit ? Color.red : Color.green;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Debug.DrawLine(p5, p6, castColor);
        Debug.DrawLine(p6, p7, castColor);
        Debug.DrawLine(p7, p8, castColor);
        Debug.DrawLine(p8, p5, castColor);

        Debug.DrawLine(p1, p5, Color.grey);
        Debug.DrawLine(p2, p6, Color.grey);
        Debug.DrawLine(p3, p7, Color.grey);
        Debug.DrawLine(p4, p8, Color.grey);
        if (hit)
        {
            Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
        }

        return hit;

    }
}
