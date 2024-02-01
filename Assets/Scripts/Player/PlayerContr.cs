using UnityEngine;
public enum EPlayerStates
{
    IDLE,
    WALKING,
    JUMPING,
    FALLING,
    KILLED,
    DEAD,
}
public class PlayerContr : MonoBehaviour
{
    public GameObject m_fireball;
    public Vector2 m_fireballforce;


    public EPlayerStates CurrentState => m_currentState;
    public float HorizontalDir => m_horizontalDirection;

    [SerializeField]
    private float m_movementSpeed = 8.0f;
    [SerializeField]
    private float m_jumpForce = 8.0f;

    [SerializeField]
    private float m_reboundForce = 3.0f;

    [SerializeField]
    private float m_horizontalDirection;

    [SerializeField]
    private float m_jumpTime;
    private float m_jumpCounter;
    private Rigidbody2D m_rb;
    private bool m_isGrounded;
    private Vector2 m_groundCheckSize;
    private Vector2 m_groundCheckPos;
    private float m_groundCheckPosY;

    [SerializeField]
    private LayerMask m_groundAndEnemyLayer;
    private RaycastHit2D m_hit;
    private EPlayerStates m_currentState;
    private SpriteRenderer m_sprite;

    [SerializeField]
    private Animator m_animator;

    public static PlayerContr Player { get; private set; }

    private void Awake()
    {
        if (Player != null)
        {
            Destroy(gameObject);
            return;
        }
        Player = this;
    }

    void Start()
    {
        m_sprite = GetComponentInChildren<SpriteRenderer>();
        BoxCollider2D tmp = GetComponent<BoxCollider2D>();
        m_groundCheckPosY = tmp.size.y;
        m_groundCheckSize = new Vector2(tmp.size.x, 0.5f);

        m_rb = GetComponent<Rigidbody2D>();

        m_animator.SetTrigger("Idle");
        m_animator.SetFloat("Death", 1);
    }

    void Update()
    {
        m_horizontalDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject fb = Instantiate(m_fireball, transform.position, Quaternion.identity);
            Rigidbody2D fbrb = fb.GetComponent<Rigidbody2D>();
            fbrb.AddForce(m_fireballforce * 100);
        }

        m_groundCheckPos = new Vector2(transform.position.x, transform.position.y - m_groundCheckPosY * 0.5f) * transform.localScale;
        m_hit = BoxCast.Cast(m_groundCheckPos, m_groundCheckSize, 0f, Vector2.down, 0.1f, m_groundAndEnemyLayer);

        m_isGrounded = m_hit.collider;

        if (m_currentState != EPlayerStates.DEAD)
        {

            if (m_horizontalDirection > 0)
                m_sprite.flipX = false;
            else if (m_horizontalDirection < 0)
                m_sprite.flipX = true;

            Move();
        }

        m_currentState = CheckState(m_currentState);
    }

    private EPlayerStates CheckState(EPlayerStates _currentState)
    {
        switch (_currentState)
        {
            case EPlayerStates.IDLE:
                {
                    m_animator.SetTrigger("Idle");

                    if (CheckEnemy())
                        return EPlayerStates.KILLED;

                    if (m_rb.velocity.x != 0)
                    {
                        m_animator.ResetTrigger("Idle");
                        return EPlayerStates.WALKING;
                    }

                    if (CheckJump())
                    {
                        Jump();
                        m_animator.ResetTrigger("Idle");
                        return EPlayerStates.JUMPING;
                    }

                    if (!m_isGrounded)
                    {
                        m_animator.ResetTrigger("Idle");
                        return EPlayerStates.FALLING;
                    }

                    return EPlayerStates.IDLE;
                }
            case EPlayerStates.WALKING:
                {
                    m_animator.SetTrigger("Running");

                    if (CheckEnemy())
                    {
                        m_animator.ResetTrigger("Running");
                        return EPlayerStates.KILLED;
                    }

                    if (CheckJump())
                    {
                        Jump();
                        m_animator.ResetTrigger("Running");
                        return EPlayerStates.JUMPING;
                    }

                    if (m_rb.velocity.x != 0)
                    {
                        return EPlayerStates.WALKING;
                    }

                    m_animator.ResetTrigger("Running");
                    return EPlayerStates.IDLE;
                }
            case EPlayerStates.JUMPING:
                {
                    m_animator.SetTrigger("Jump");

                    if (CheckEnemy())
                        return EPlayerStates.KILLED;

                    return Jump();
                }
            case EPlayerStates.FALLING:
                {
                    if (m_isGrounded && !CheckEnemyKill())
                    {
                        if (m_rb.velocity.x != 0)
                            return EPlayerStates.WALKING;

                        return EPlayerStates.IDLE;
                    }

                    return EPlayerStates.FALLING;
                }
            case EPlayerStates.KILLED:
                {
                    Audio.Manager.StopBGM();
                    Audio.Manager.PlaySound(ESounds.DEATH);
                    Game.Manager.ChangeMainMenu();
                    m_animator.SetFloat("Death", 0);
                    return EPlayerStates.DEAD;
                }
            case EPlayerStates.DEAD:
                {
                    return EPlayerStates.DEAD;
                }

            default:
                return EPlayerStates.IDLE;
        }
    }
    private bool CheckEnemy()
    {
        if (m_hit.collider)
        {
            return m_hit.collider.CompareTag("Enemy");
        }
        return false;
    }

    private EPlayerStates Jump()
    {
        if (m_jumpCounter > 0)
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpForce);
            m_jumpCounter -= Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                m_animator.ResetTrigger("Jump");
                return EPlayerStates.FALLING;
            }
            return EPlayerStates.JUMPING;
        }

        m_animator.ResetTrigger("Jump");
        return EPlayerStates.FALLING;
    }

    private bool CheckEnemyKill()
    {
        if (m_hit.collider.CompareTag("Enemy"))
        {
            m_hit.collider.GetComponent<EnemyController>().Kill();
            m_rb.velocity = Vector2.up * m_reboundForce;
            Audio.Manager.PlaySound(ESounds.KILLENEMY);
            return true;
        }

        return false;
    }

    private void Move()
    {
        m_rb.velocity = new Vector2(m_horizontalDirection * m_movementSpeed, m_rb.velocity.y);
    }

    private bool CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_jumpCounter = m_jumpTime;
            Audio.Manager.PlaySound(ESounds.JUMP);
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.collider.CompareTag("Enemy"))
        {
            if (m_currentState != EPlayerStates.FALLING)
                m_currentState = EPlayerStates.KILLED;
            else
            {
                _collision.collider.GetComponent<EnemyController>().Kill();
                m_rb.velocity = Vector2.up * m_reboundForce;
                Audio.Manager.PlaySound(ESounds.KILLENEMY);
            }
        }

        if (_collision.collider.CompareTag("DeathZone"))
                m_currentState = EPlayerStates.KILLED;

        if (_collision.collider.CompareTag("Block"))
        {
            if (m_currentState == EPlayerStates.JUMPING)
            {
                m_currentState = EPlayerStates.FALLING;
            }
        }
    }
}
