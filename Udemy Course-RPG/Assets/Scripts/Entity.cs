using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    protected StateMachin stateMachine;
    public Animator animator { get; private set; }

    public Rigidbody2D rb { get; private set; }



    private bool facingRihgt = true;
    public int facingDir { get; private set; } = 1;



    [Header("Collision detection")]
    [SerializeField] private float groundCheckDist;
    [SerializeField] private float wallCheckDist;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primarywallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private bool isKnockback;
    private Coroutine knockbackCoroutine;

    public bool canDash;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachin();
       
    }

    protected virtual void Start()
    {
            
    }
    protected virtual void Update()
    {
        HendleColisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CurrentAinmTriggerCall()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }
    public virtual void EntityDeath()
    {

    }
    public void Knockback(float knockbackTime, Vector2 knockbackForce)
    {
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);
        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(knockbackTime, knockbackForce));
    }
    private IEnumerator KnockbackCoroutine(float knockbackTime, Vector2 knockbackForce)
    {
        isKnockback = true;
        rb.linearVelocity =knockbackForce;
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if(isKnockback) return;
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }
    public void HandleFlip(float xVelocity)
    {
        if ((xVelocity > 0 && !facingRihgt) || (xVelocity < 0 && facingRihgt))
        {
            Flip();
        }
    }
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRihgt = !facingRihgt;
        facingDir *= -1;
    }
    private void HendleColisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, groundLayer);
        bool wallHit1 = Physics2D.Raycast(primarywallCheck.position, Vector2.right * facingDir, wallCheckDist, groundLayer);
        if (secondaryWallCheck != null)
        {
            bool wallHit2 = Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDist, groundLayer);
            wallDetected = wallHit1 && wallHit2;
            canDash = !(wallHit1 || wallHit2);
        }
        else
        {
            wallDetected = wallHit1;
            canDash = !wallHit1;
        }
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDist));
        Gizmos.DrawLine(primarywallCheck.position, primarywallCheck.position + new Vector3(wallCheckDist * facingDir, 0));
        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDist * facingDir, 0));
    }

}
