using UnityEngine;

public class Block : MonoBehaviour
{
    public bool IsAlive => m_alive;

    private bool m_alive;
    private SpriteRenderer m_sRenderer;
    private Animator m_animator;
    private Rigidbody2D m_rb;

    private Vector2 m_groundCheckSize;
    private Vector2 m_playerHitCheck;
    private float m_groundCheckPosY;

    [SerializeField]
    private LayerMask m_playerLayer;
    private RaycastHit2D m_hit;

    private void Start()
    {
        m_animator = GetComponentInParent<Animator>();
        m_sRenderer = GetComponentInParent<SpriteRenderer>();
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        m_groundCheckPosY = tmp.size.y;
        m_groundCheckSize = new Vector2(tmp.size.x, 0.5f);

        m_rb = GetComponent<Rigidbody2D>();
        m_alive = true;
    }

    private void Update()
    {
        if (!m_alive)
            return;
        
        m_playerHitCheck = new Vector2(transform.position.x, transform.position.y - m_groundCheckPosY * 0.5f) * transform.localScale;

        if (BoxCast.Cast(m_playerHitCheck, m_groundCheckSize, 0f, Vector2.down, 0.1f, m_playerLayer))
        {
            m_alive = false;
            Game.Manager.GetCoin();
            m_animator.SetTrigger("Dead");
        }
    }
}
