using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject m_followObject;
    [SerializeField]
    private Vector2 m_offset;
    [SerializeField]
    private Vector2 m_centerOffset;
    private Rigidbody2D m_followRigidbody;

    private Vector2 m_followPos;
    private Vector3 m_newPos;
    private Vector2 m_threshold;

    [SerializeField]
    private float m_speed;

    private void Start()
    {
        if (m_followObject == null)
            m_followObject = FindObjectOfType<PlayerController>().gameObject;

        m_followRigidbody = m_followObject.GetComponent<Rigidbody2D>();

        m_threshold = CalculateThreshold();

    }


    private void LateUpdate()
    {
        m_threshold = CalculateThreshold();
        m_followPos = m_followObject.transform.position;
        float xDiff = Vector2.Distance(Vector2.right * (transform.position.x + m_centerOffset.x), Vector2.right * m_followPos.x);
        float yDiff = Vector2.Distance(Vector2.up * (transform.position.y + m_centerOffset.y) , Vector2.up * m_followPos.y);

        float zPos = transform.position.z;
        m_newPos = transform.position;

        if (Mathf.Abs(xDiff) >= m_threshold.x)
            m_newPos.x = m_followPos.x;
        if (Mathf.Abs(yDiff) >= m_threshold.y)
            m_newPos.y = m_followPos.y;

        m_newPos.z = zPos;
        m_speed = m_followRigidbody.velocity.magnitude;

        transform.position = Vector3.MoveTowards(transform.position, m_newPos, m_speed * Time.deltaTime);


    }
    private Vector2 CalculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 tmp = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        tmp -= m_offset;

        return tmp;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(m_centerOffset.x, m_centerOffset.y), CalculateThreshold() * 2);
    }



}
