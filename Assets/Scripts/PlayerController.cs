using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private float m_moveDirection;

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

        /*TODO:  Ground check box size ?? why doesnt it work on edges though...
         /        Sprite Sheet setup und animation
         /        tilemap für level
         */
        
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        m_groundCheckPosY = tmp.size.y;
        m_groundCheckSize = new Vector2(tmp.size.x, 0.5f);
        m_groundCheckPos = new Vector2(transform.position.x, transform.position.y - m_groundCheckPosY * 0.5f) * transform.localScale;

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        m_groundCheckPos = new Vector2(transform.position.x, transform.position.y - m_groundCheckPosY * 0.5f) * transform.localScale;
        m_isGrounded = Physics2D.BoxCast(m_groundCheckPos, m_groundCheckSize, 0f, Vector2.down, 0f, m_groundLayer);
        
        //m_isGrounded = Physics2D.OverlapBox(m_groundCheckPos, m_groundCheckSize * transform.localScale * 0.5f, 0f, m_groundLayer);
        m_moveDirection = Input.GetAxisRaw("Horizontal");

        if (m_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            m_isJumping = true;
            m_jumpCounter = m_jumpTime;
        }

        if (m_isJumping)
        {
            if (m_jumpCounter > 0)
            {
                m_rigidbody.velocity = Vector2.up * m_jumpForce;
                //m_rigidbody.AddForce(Vector2.up * m_jumpForce * 10f, ForceMode2D.Impulse);
                m_jumpCounter -= Time.deltaTime;
            }
            else
                m_isJumping = false;
        }

        m_rigidbody.velocity = new Vector2(m_moveDirection * m_movementSpeed, m_rigidbody.velocity.y);
        //m_rigidbody.AddForce(m_moveDirection * m_movementSpeed * 1000f * Time.fixedDeltaTime, ForceMode2D.Force);

        if (Input.GetKeyUp(KeyCode.Space))
            m_isJumping = false;
    }

    private void FixedUpdate()
    {

    }

    static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");

        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        clearMethod.Invoke(null, null);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(m_groundCheckPos, 0.1f);
    //    Gizmos.DrawCube(m_groundCheckPos, Physics2D.OverlapBox(m_groundCheckPos, m_groundCheckSize * transform.localScale, 0f, m_groundLayer).bounds.size);
    //}

}
