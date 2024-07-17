using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    public Transform Player;
    public float XMoveSpeed;
    private Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector2((Player.position.x * XMoveSpeed) + startPos.x, transform.position.y);
    }
}
