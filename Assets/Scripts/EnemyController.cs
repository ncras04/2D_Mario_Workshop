using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
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

    private void Start()
    {
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        wallCheckPosX = tmp.size.x * 0.5f;
        wallCheckSize = new Vector2(tmp.size.y * 0.5f, 0.5f);

        rb = GetComponent<Rigidbody2D>();

        movingLeft = true;
        movementSpeed = -movementSpeed;
        wallCheckPosX = -wallCheckPosX;
    }

    private void Update()
    {
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
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    }
}

