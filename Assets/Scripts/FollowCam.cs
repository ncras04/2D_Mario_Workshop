using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget;
    [SerializeField]
    private float offset;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector3 tempPos = new Vector3(followTarget.position.x + offset, transform.position.y, transform.position.z);
        transform.position = tempPos;
    }
}
