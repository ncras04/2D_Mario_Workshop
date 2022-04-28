using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject m_followObject;
    [SerializeField]
    private Vector2 centerOffset;
    private Rigidbody2D followRigidbody;
    private Transform followTransform;

    private Vector3 currentVelocity;

    [SerializeField]
    private Transform minPos;
    [SerializeField]
    private Transform maxPos;

    Vector3 clampPos;
    Vector3 newPos;

    [SerializeField]
    float smoothTime;

    private void Start()
    {
        if (m_followObject == null)
            m_followObject = FindObjectOfType<PlayerContr>().gameObject;

        followRigidbody = m_followObject.GetComponent<Rigidbody2D>();

        followTransform = m_followObject.transform;
    }
    private void LateUpdate()
    {
        if (followRigidbody.velocity.x != 0)
            centerOffset.x = Mathf.Abs(centerOffset.x) * (Mathf.Abs(followRigidbody.velocity.x) / followRigidbody.velocity.x);

        newPos = new Vector3(followTransform.position.x + centerOffset.x, transform.position.y + centerOffset.y, transform.position.z);

        clampPos = new Vector3(Mathf.Clamp(newPos.x, minPos.position.x, maxPos.position.x),
                               Mathf.Clamp(newPos.y, minPos.position.y, maxPos.position.y),
                               transform.position.z);
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, clampPos, ref currentVelocity, smoothTime);
    }
}
