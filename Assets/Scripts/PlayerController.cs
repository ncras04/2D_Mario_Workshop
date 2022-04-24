using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Vector2 m_moveDirection;

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
        m_isGrounded = Physics2D.OverlapBox(m_groundCheckPos, m_groundCheckSize, m_groundLayer);

        m_moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_jumpForce = 10;
        }
        else
        {
            m_jumpForce = 0;
        }

    }

    private void FixedUpdate()
    {
        m_rigidbody.AddForce(m_moveDirection * m_movementSpeed * 100f * Time.fixedDeltaTime, ForceMode2D.Force);

        m_rigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(m_groundCheckPos, m_groundCheckSize);
    }
}
