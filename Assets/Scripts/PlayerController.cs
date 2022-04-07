using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidbody;

    public Vector2 moveDirection;
    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0) * movementSpeed * Time.deltaTime;

    }
}
