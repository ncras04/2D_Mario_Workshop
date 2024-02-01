using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private float m_timer = 0f;

    private void Start()
    {
        Destroy(this.gameObject, m_timer);
    }
}

