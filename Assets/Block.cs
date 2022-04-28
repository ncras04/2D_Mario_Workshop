using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isAlive => alive;

    private bool alive;
    private SpriteRenderer sprite;
    private Animator animate;
    private Rigidbody2D rb;

    private Vector2 groundCheckSize;
    private Vector2 playerHitCheck;
    private float groundCheckPosY;

    [SerializeField]
    private LayerMask playerLayer;
    private RaycastHit2D hit;

    private void Start()
    {
        animate = GetComponentInParent<Animator>();
        sprite = GetComponentInParent<SpriteRenderer>();
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        groundCheckPosY = tmp.size.y;
        groundCheckSize = new Vector2(tmp.size.x, 0.5f);

        rb = GetComponent<Rigidbody2D>();
        alive = true;

    }

    private void Update()
    {
        if (!alive)
            return;
        
        playerHitCheck = new Vector2(transform.position.x, transform.position.y - groundCheckPosY * 0.5f) * transform.localScale;

        if (BoxCast.Cast(playerHitCheck, groundCheckSize, 0f, Vector2.down, 0.1f, playerLayer))
        {
            alive = false;
            Game.Manager.GetCoin();
            animate.SetTrigger("Dead");
        }


    }



}
