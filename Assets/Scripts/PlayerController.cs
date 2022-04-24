using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Vector2 m_moveDirection;

    [SerializeField]
    private float m_jumpTime;
    private float m_jumpCounter;

    [SerializeField]
    private float m_jumpForce;

    private bool m_isGrounded;
    private bool m_isJumping;

    [SerializeField]
    private LayerMask m_groundLayer;


    [SerializeField]
    private float m_movementSpeed;

    private Vector2 m_groundCheckSize;
    private Vector2 m_groundCheckPos;
    private float m_groundCheckPosY;

    void Start()
    {
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        m_groundCheckPosY = tmp.size.y;
        m_groundCheckSize = new Vector2(tmp.size.x, 0.3f);
        m_groundCheckPos = new Vector2(transform.position.x, transform.position.y - m_groundCheckPosY);

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        m_groundCheckPos = new Vector2(transform.position.x, transform.position.y - m_groundCheckPosY);

        m_moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);

        if (m_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            m_isJumping = true;
            m_jumpCounter = m_jumpTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
            m_isJumping = false;
    }

    private void FixedUpdate()
    {
        m_isGrounded = Physics2D.OverlapBox(m_groundCheckPos, m_groundCheckSize, 0f, m_groundLayer);

        if (m_isJumping)
        {
            if (m_jumpCounter > 0)
            {
                m_rigidbody.AddForce(Vector2.up * m_jumpForce * 10f, ForceMode2D.Impulse);
                m_jumpCounter -= Time.fixedDeltaTime;
            }
            else
                m_isJumping = false;
        }
        
        if(m_isGrounded)
            m_rigidbody.AddForce(m_moveDirection * m_movementSpeed * 1000f * Time.fixedDeltaTime, ForceMode2D.Force);

    }
}
