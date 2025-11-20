using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    public PlayerInputSet inputSet { get; private set; }
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    [Header("Movement")]
    public float movementSpeed;
    public float jumpForce;
    public Vector2 wallJumpForce;

    [Range(0, 1)]
    public float wallSlideSlowMultiplier;
    [Range(0, 1)]
    public float inAirMovementMultiplier;
    [Space]
    public float dashDuration;
    public float dashSpeed;

    [Header("Attack")]
    public Vector2[] attackVelocity;
    public float attackVelocityDuration;
    public float comboResetTime = 1f;
    public Vector2 jumpAttackVelocity;
    private Coroutine queueAttackCoroutine;

    public Vector2 movementInput { get; private set; }
    override protected void Awake()
    {
        base.Awake();
        inputSet = new PlayerInputSet();
        idleState = new Player_IdleState(this, stateMachine, "IDLE");
        moveState = new Player_MoveState(this, stateMachine, "MOVE");

        jumpState = new Player_JumpState(this, stateMachine, "JumpFall");
        fallState = new Player_FallState(this, stateMachine, "JumpFall");

        wallSlideState = new Player_WallSlideState(this, stateMachine, "WALLSLIDE");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "JumpFall");

        dashState = new Player_DashState(this, stateMachine, "DASH");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
    }
    void OnEnable()
    {
        inputSet.Enable();
        inputSet.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputSet.Player.Movement.canceled += ctx => movementInput = Vector2.zero;
    }



    void OnDisable()
    {
        inputSet.Disable();
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    protected override IEnumerator SlowDownCoroutine(float duration, float slowAmount)
    {
        float originalMoveSpeed = movementSpeed;
        float originalAnimSpeed = animator.speed;
        float originalDashSpeed = dashSpeed;
        float originalJumpForce = jumpForce;
        Vector2 originalJumpAttackVelocity = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = (Vector2[])attackVelocity.Clone();
        Vector2 orignalWallJumpForce = wallJumpForce;

        float speedReductionFactor = 1f - slowAmount;

        movementSpeed *= speedReductionFactor;
        dashSpeed *= speedReductionFactor;
        jumpForce *= speedReductionFactor;
        wallJumpForce *= speedReductionFactor;
        jumpAttackVelocity *= speedReductionFactor;
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] *= speedReductionFactor;
        }
        animator.speed *= speedReductionFactor;
        yield return new WaitForSeconds(duration);
        movementSpeed = originalMoveSpeed;
        dashSpeed = originalDashSpeed;
        jumpForce = originalJumpForce;
        wallJumpForce = orignalWallJumpForce;
        jumpAttackVelocity = originalJumpAttackVelocity;
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
        animator.speed = originalAnimSpeed;

    }
    public override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }
    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
    public void EnterAttackStateWithDelay()
    {
        if (queueAttackCoroutine != null)
            StopCoroutine(queueAttackCoroutine);
        queueAttackCoroutine = StartCoroutine(EnterAttackStateWithDelayCo());
    }
}
