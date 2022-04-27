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

    private Vector3 m_vel;
    private Transform m_followTransform;

    private void Start()
    {
        if (m_followObject == null)
            m_followObject = FindObjectOfType<PlayerController>().gameObject;

        m_followRigidbody = m_followObject.GetComponent<Rigidbody2D>();

        m_followTransform = m_followObject.transform;

        m_threshold = CalculateThreshold();

    }

    private void LateUpdate()
    {
        m_followPos = m_followTransform.position;

        float xDiff = Vector2.Distance(Vector2.right * (transform.position.x + m_centerOffset.x), Vector2.right * m_followPos.x);
        float yDiff = Vector2.Distance(Vector2.up * (transform.position.y + m_centerOffset.y), Vector2.up * m_followPos.y);
        
        m_newPos = transform.position;

        if (Mathf.Abs(xDiff) >= m_threshold.x)
            m_newPos.x = m_followPos.x + Mathf.Abs(m_centerOffset.x);
        if (Mathf.Abs(yDiff) >= m_threshold.y)
            m_newPos.y = m_followPos.y;

        transform.position = Vector3.MoveTowards(transform.position, m_newPos, m_followRigidbody.velocity.magnitude * Time.unscaledDeltaTime);
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
