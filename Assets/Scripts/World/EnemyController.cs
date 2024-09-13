using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool m_alive;
    private bool m_movingLeft;

    [SerializeField]
    private float m_movementSpeed;

    private Vector2 m_wallCheckSize;
    private Vector2 m_wallCheckPos;

    [SerializeField]
    private float m_wallCheckPosX;

    [SerializeField]
    private LayerMask m_wallLayer;
    private RaycastHit2D m_hit;

    private Rigidbody2D m_rb;
    private Animator m_animator;
    private SpriteRenderer m_sprite;

    private void Start()
    {
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        m_animator = GetComponentInChildren<Animator>();
        m_sprite = GetComponentInChildren<SpriteRenderer>();
        m_wallCheckPosX = tmp.size.x * 0.5f;
        m_wallCheckSize = new Vector2(0.5f, tmp.size.y * 0.5f);

        m_rb = GetComponent<Rigidbody2D>();

        m_alive = true;
        m_movingLeft = true;
        m_movementSpeed = -m_movementSpeed;
        m_wallCheckPosX = -m_wallCheckPosX;
    }

    private void Update()
    {
        if (!m_alive)
            return;

            m_wallCheckPos = new Vector2(transform.position.x + m_wallCheckPosX, transform.position.y) * transform.localScale;

            if (m_movingLeft)
            {
                if (BoxCast.Cast(m_wallCheckPos, m_wallCheckSize, 90f, Vector2.left, 0.1f, m_wallLayer))
                {
                    m_wallCheckPosX = Mathf.Abs(m_wallCheckPosX);
                    m_movementSpeed = Mathf.Abs(m_movementSpeed);
                    m_movingLeft = !m_movingLeft;
                    m_sprite.flipX = true;
                }
            }
            else
            {
                if (BoxCast.Cast(m_wallCheckPos, m_wallCheckSize, 90f, Vector2.right, 0.1f, m_wallLayer))
                {
                    m_wallCheckPosX = -m_wallCheckPosX;
                    m_movementSpeed = -m_movementSpeed;
                    m_movingLeft = !m_movingLeft;
                    m_sprite.flipX = false;
                }
            }
    }
    private void FixedUpdate()
    {
        if (!m_alive)
            return;

        m_rb.velocity = new Vector2(m_movementSpeed, m_rb.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.collider.CompareTag("DeathZone") || _collision.collider.CompareTag("Fireball"))
            Kill();

        return;
    }

    public void Kill()
    {
        m_alive = false;
        m_animator.SetTrigger("Death");
        gameObject.tag = "Dead";
        m_rb.simulated = false;
        Audio.Manager.PlaySound(ESounds.KILLENEMY);
    }
}

