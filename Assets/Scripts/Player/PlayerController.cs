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
        Collider2D collision = Physics2D.OverlapBox(m_groundCheckPos.position, m_groundChecksize, 0, m_groundLayer);

        if (collision is not null)
        {
            m_animator.SetBool("Grounded", true);
        }
        else
        {
            m_animator.SetBool("Grounded", false);
        }
    }


    private void FixedUpdate()
    {
        m_animator.SetFloat("Movement", Mathf.Abs(m_rb.velocity.x));
    }


    private void Jump()
    {
        Audio.Manager.PlaySound(ESounds.JUMP);
    }


    public void OnMovement(InputAction.CallbackContext _ctx)
    {
        m_moveInput = _ctx.ReadValue<Vector2>().x;
    }


    public void OnJump(InputAction.CallbackContext _ctx)
    {
        if (_ctx.performed)
        {

        }
        if (_ctx.canceled)
        {

        }
    }


    public void OnShoot(InputAction.CallbackContext _ctx)
    {
        if (_ctx.performed)
        {

        }
    }
}
