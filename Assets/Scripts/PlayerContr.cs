using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContr : MonoBehaviour
{
    public float MovementSpeed = 8.0f;
    public float JumpPower = 8.0f;

    public float HorizontalDirection = 0.0f;
    public float VerticalDirection = 0.0f;


    private Rigidbody2D rigidbody;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalDirection = Input.GetAxis("Horizontal") * MovementSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(HorizontalDirection, rigidbody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Abfrage ob es ein Bodentile oder sowas ist und kein enemy
        isJumping=false;
    }
}
