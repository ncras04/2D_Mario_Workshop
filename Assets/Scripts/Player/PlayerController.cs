using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private Animator m_animator;
    private SpriteRenderer m_sprite;


    private float m_moveInput;
    [Header("Movement")]
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private float m_acceleration;
    [Space]
    [SerializeField]
    private float m_frictionAmount;

    [Header("Jumping")]
    [SerializeField]
    private float m_jumpForce;
    [SerializeField]
    private float m_reboundForce;
    [Space]
    [SerializeField]
    private float m_gravityScale;
    [SerializeField]
    private float m_fallGravityMultiplier;
    [Space]
    [SerializeField]
    private float m_jumpCoyoteTime;
    [SerializeField]
    private float m_jumpBufferTime;

    private float m_lastGroundedTime = 0;
    private float m_lastJumpTime = 0;
    private bool m_isJumping = false;

    [Header("Ground Check")]
    [SerializeField]
    private Transform m_groundCheckPos;
    [SerializeField]
    private Vector2 m_groundChecksize;
    [SerializeField]
    private LayerMask m_groundLayer;

    [Header("Fireball")]
    [SerializeField]
    private GameObject m_fireballPrefab;
    [SerializeField]
    private float m_fireballSpeed;



    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
        m_sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        m_lastGroundedTime -= Time.deltaTime;
        m_lastJumpTime -= Time.deltaTime;

        Collider2D collision = Physics2D.OverlapBox(m_groundCheckPos.position, m_groundChecksize, 0, m_groundLayer);
        if (collision is not null)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyController>().Kill();
                m_rb.AddForce(Vector2.up * m_reboundForce, ForceMode2D.Impulse);
            }
            else
            {
                if (m_rb.velocity.y < 0.01f)
                    m_isJumping = false;

                m_lastGroundedTime = m_jumpCoyoteTime;
                m_animator.SetBool("Grounded", true);
            }
        }
        else
            m_animator.SetBool("Grounded", false);

        if (m_moveInput != 0)
        {
            m_sprite.flipX = m_moveInput < -0.01f;
        }
    }

    private void FixedUpdate()
    {
        //rigidbody.velocity = new Vector2(moveInput * moveSpeed, rigidbody.velocity.y);
        float targetSpeed = m_moveInput * m_moveSpeed;
        float speedDifference = targetSpeed - m_rb.velocity.x;

        //float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        //float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelRate, velPower) * Mathf.Sign(speedDifference);

        float movement = speedDifference * m_acceleration;

        m_rb.AddForce(movement * Vector2.right);

        if (m_lastGroundedTime > 0 && MathF.Abs(m_moveInput) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(m_rb.velocity.x), Mathf.Abs(m_frictionAmount));

            amount *= Mathf.Sign(m_rb.velocity.x);
            m_rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        if (m_rb.velocity.y < 0)
        {
            m_rb.gravityScale = m_gravityScale * m_fallGravityMultiplier;
        }
        else
        {
            m_rb.gravityScale = m_gravityScale;
        }

        m_animator.SetFloat("Movement", Mathf.Abs(m_rb.velocity.x));

    }
    public void OnMovement(InputAction.CallbackContext _ctx)
    {
        m_moveInput = _ctx.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext _ctx)
    {
        if (_ctx.performed)
        {
            m_lastJumpTime = m_jumpBufferTime;

            if (m_lastGroundedTime > 0 && m_lastJumpTime > 0 && !m_isJumping)
                Jump();
        }
        if (_ctx.canceled)
        {
            if (m_rb.velocity.y > 0 && m_isJumping)
            {
                m_rb.AddForce(Vector2.down * m_rb.velocity.y, ForceMode2D.Impulse);
            }

            m_lastJumpTime = 0;
        }
    }
    private void Jump()
    {
        m_rb.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
        m_lastGroundedTime = 0;
        m_lastJumpTime = 0;
        m_isJumping = true;

        Audio.Manager.PlaySound(ESounds.JUMP);
    }
    public void OnShoot(InputAction.CallbackContext _ctx)
    {
        if (_ctx.performed)
        {
            GameObject fb = Instantiate(m_fireballPrefab, transform.position, Quaternion.identity);
            Rigidbody2D fbrb = fb.GetComponent<Rigidbody2D>();

            int direction = m_sprite.flipX ? -1 : 1; 
            fbrb.velocity = new Vector2(direction * (m_fireballSpeed + Mathf.Abs(m_rb.velocity.x)), 0);

            Audio.Manager.PlaySound(ESounds.FIREBALL);
        }
    }
}
