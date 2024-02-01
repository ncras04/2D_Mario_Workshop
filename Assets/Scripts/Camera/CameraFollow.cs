using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject m_followObject;
    [SerializeField]
    private Vector2 m_centerOffset;
    private Rigidbody2D m_followRigidbody;
    private Transform m_followTransform;

    private Vector3 m_currentVelocity;

    [SerializeField]
    private Transform m_minPos;
    [SerializeField]
    private Transform m_maxPos;

    private Vector3 m_clampPos;
    private Vector3 m_newPos;

    [SerializeField]
    float m_smoothTime;

    private void Start()
    {
        if (m_followObject == null)
            m_followObject = FindObjectOfType<PlayerContr>().gameObject;

        m_followRigidbody = m_followObject.GetComponent<Rigidbody2D>();
        m_followTransform = m_followRigidbody.transform;
    }
    private void LateUpdate()
    {
        if (m_followRigidbody.velocity.x != 0)
            m_centerOffset.x = Mathf.Abs(m_centerOffset.x) * (Mathf.Abs(m_followRigidbody.velocity.x) / m_followRigidbody.velocity.x);

        m_newPos = new Vector3(m_followTransform.position.x + m_centerOffset.x, m_centerOffset.y, transform.position.z);

        m_clampPos = new Vector3(Mathf.Clamp(m_newPos.x, m_minPos.position.x, m_maxPos.position.x),
                               m_newPos.y,
                               m_newPos.z);

        transform.position = Vector3.SmoothDamp(transform.position, m_clampPos, ref m_currentVelocity, m_smoothTime);
    }
}
