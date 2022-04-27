using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContr : MonoBehaviour
{
    public float MovementSpeed = 8.0f;
    public float JumpPower = 8.0f;

    private float horizontalDirection;

    public float JumpTime;
    private float jumpCounter;


    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isGrounded;

    private Vector2 groundCheckSize;
    private Vector2 groundCheckPos;
    private float groundCheckPosY;

    public LayerMask groundAndEnemyLayer;

    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        groundCheckPosY = tmp.size.y;
        groundCheckSize = new Vector2(tmp.size.x, 0.5f);

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        groundCheckPos = new Vector2(transform.position.x, transform.position.y - groundCheckPosY * 0.5f) * transform.localScale;

        hit = Physics2D.BoxCast(groundCheckPos, groundCheckSize, 0f, Vector2.down, 0f, groundAndEnemyLayer);

        if (!isGrounded && hit.collider)
        {
            if (hit.collider.CompareTag("Enemy"))
                Destroy(hit.collider.gameObject);
        }

        isGrounded = hit;

        Debug.Log(isGrounded);

        horizontalDirection = Input.GetAxisRaw("Horizontal") * MovementSpeed;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpCounter = JumpTime;
        }

        if (isJumping)
        {
            if (jumpCounter > 0)
            {
                rb.velocity = Vector2.up * JumpPower;
                jumpCounter -= Time.deltaTime;
            }
            else
                isJumping = false;
        }

        rb.velocity = new Vector2(horizontalDirection, rb.velocity.y);

        if (Input.GetKeyUp(KeyCode.Space))
            isJumping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheckPos, groundCheckSize);
    }
}
