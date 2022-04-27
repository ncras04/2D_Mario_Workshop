using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform WaypointLeft;
    public Transform WaypointRight;
    public bool LeftSide = false;

    public float Movementspeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftSide)
        {
            transform.position += (WaypointLeft.position - transform.position).normalized * Movementspeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, WaypointLeft.position) < 0.1f)
                LeftSide = !LeftSide;
        }
        else
        {
            transform.position += (WaypointRight.position - transform.position).normalized * Movementspeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, WaypointRight.position) < 0.1f)
                LeftSide = !LeftSide;
        }
    }
}
