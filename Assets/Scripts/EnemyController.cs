using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool alive;
    bool movingLeft;

    [SerializeField]
    float movementSpeed;

    private Vector2 wallCheckSize;
    private Vector2 wallCheckPos;
    [SerializeField]
    private float wallCheckPosX;

    [SerializeField]
    private LayerMask wallLayer;

    private RaycastHit2D hit;

    private Rigidbody2D rb;

    private Animator animator;

    private void Start()
    {
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        wallCheckPosX = tmp.size.x * 0.5f;
        wallCheckSize = new Vector2(0.5f, tmp.size.y * 0.5f);

        rb = GetComponent<Rigidbody2D>();

        alive = true;
        movingLeft = true;
        movementSpeed = -movementSpeed;
        wallCheckPosX = -wallCheckPosX;
    }

    private void Update()
    {
        if (!alive)
            return;

            wallCheckPos = new Vector2(transform.position.x + wallCheckPosX, transform.position.y) * transform.localScale;

            if (movingLeft)
            {
                if (BoxCast.Cast(wallCheckPos, wallCheckSize, 90f, Vector2.left, 0.1f, wallLayer))
                {
                    wallCheckPosX = Mathf.Abs(wallCheckPosX);
                    movementSpeed = Mathf.Abs(movementSpeed);
                    movingLeft = !movingLeft;
                }
            }
            else
            {
                if (BoxCast.Cast(wallCheckPos, wallCheckSize, 90f, Vector2.right, 0.1f, wallLayer))
                {
                    wallCheckPosX = -wallCheckPosX;
                    movementSpeed = -movementSpeed;
                    movingLeft = !movingLeft;
                }
            }
    }

    private void FixedUpdate()
    {
        if (!alive)
            return;

        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("DeathZone"))
            Destroy(gameObject);

        return;
    }

    public void Kill()
    {
        alive = false;
        animator.SetTrigger("Death");
        gameObject.tag = "Dead";
        rb.simulated = false;
    }
}

