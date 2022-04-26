using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject m_followObject;
    [SerializeField]
    private Vector2 m_offset;
    private Rigidbody2D m_followRigidbody;

    private Vector2 m_followPos;
    private Vector2 m_newPos;
    private Vector2 m_threshold;

    [SerializeField]
    private float m_speed;

    private void Start()
    {
        if (m_followObject is null)
            m_followObject = FindObjectOfType<PlayerController>().gameObject;

        m_followRigidbody = m_followObject.GetComponent<Rigidbody2D>();

        m_threshold = CalculateThreshold();

    }

    private Vector2 CalculateThreshold()
    {
        throw new System.NotImplementedException();
    }

    private void LateUpdate()
    {
        
    }
    private void OnDrawGizmos()
    {
        
    }



}
