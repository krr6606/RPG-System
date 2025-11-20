using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_StunnedState stunnedState;
    public Enemy_DeadState deadState;
    [Header("Enemy Movement details")]
    public float idleTime = 2f;
    public float moveSpeed = 2.3f;
    [Range(0f,2f)]
    public float moveAnimSpeedMultiplier = 1f;

    [Header("Stunned details")]
    public float stunnedTime = 1f;
    public Vector2 stunnedVelocity = new Vector2(5.5f,5.5f);
    [SerializeField]protected bool CanBeStunned;

    [Header("Enemy Battle details")]
    public float battleMoveSpeed = 3.1f;
    public float attackDistance = 2f;
    public float battleTimeDuration = 4.3f;
    public float minimumDistanceToPlayer = 1f;
    public Vector2 retreatVelocity;
    [Header("Enemy Attack details")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDist;
    public Transform player { get; private set; }

    protected override IEnumerator SlowDownCoroutine(float duration, float slowAmount)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleMoveSpeed = battleMoveSpeed;
        float originalAnimSpeed = animator.speed;
        
        float speedReductionFactor = 1f - slowAmount;
        moveSpeed *= speedReductionFactor;
        battleMoveSpeed *= speedReductionFactor;
        animator.speed *= speedReductionFactor;

        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleMoveSpeed;
        animator.speed = originalAnimSpeed;
    }

    public void EnableCounterWindow(bool enable)=>CanBeStunned = enable;
    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
    }
    public void TryEnterBattleState(Transform playerTransform)
    {
        if(stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            return;
        }
        player = playerTransform;
        stateMachine.ChangeState(battleState);
    }
    private void HendlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }
    public Transform GetPlayerReference()
    {
        if(player == null)
        {
            player = playerDetected().transform;
        }
        return player;
    }
    public RaycastHit2D playerDetected()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDist, playerLayer|groundLayer);
        if (hit2D.collider == null || hit2D.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }
        return hit2D;
    }
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(facingDir * playerCheckDist,playerCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(facingDir * attackDistance, playerCheck.position.y));
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(facingDir * minimumDistanceToPlayer, playerCheck.position.y));
    }
    private void OnEnable()
    {
        Player.OnPlayerDeath += HendlePlayerDeath;
    }
    private void OnDisable()
    {
        Player.OnPlayerDeath -= HendlePlayerDeath;
    }
}

