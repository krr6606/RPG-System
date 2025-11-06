using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    [Header("Enemy Movement details")]
    public float idleTime = 2f;
    public float moveSpeed = 2.3f;
    [Range(0f,2f)]
    public float moveAnimSpeedMultiplier = 1f;
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

}
