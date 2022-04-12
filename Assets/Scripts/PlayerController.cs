using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Vector2 moveDirection;


    [SerializeField]
    private float movementSpeed;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
    }

    private void FixedUpdate()
    {
        m_rigidbody.AddForce(moveDirection * movementSpeed * 100f * Time.fixedDeltaTime, ForceMode2D.Force);
    }
}
