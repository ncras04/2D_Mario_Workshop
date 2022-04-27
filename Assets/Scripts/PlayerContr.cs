using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContr : MonoBehaviour
{
    public float MovementSpeed = 8.0f;
    public float JumpPower = 8.0f;

    public float HorizontalDirection = 0.0f;
    public float VerticalDirection = 0.0f;

    public float JumpTime;
    private float jumpCounter;


    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isGrounded;

    private Vector2 groundCheckSize;
    private Vector2 groundCheckPos;
    private float groundCheckPosY;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        groundCheckPosY = tmp.size.y;
        groundCheckSize = new Vector2(tmp.size.x, 0.5f);
        groundCheckPos = new Vector2(transform.position.x, transform.position.y - groundCheckPosY * 0.5f) * transform.localScale;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.BoxCast(groundCheckPos, groundCheckSize, 0f, Vector2.down, 0f, groundLayer);

        HorizontalDirection = Input.GetAxis("Horizontal") * (MovementSpeed * Time.deltaTime);

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


        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            isJumping = true;
        }

        rb.velocity = new Vector2(HorizontalDirection, rb.velocity.y);
    }
}
