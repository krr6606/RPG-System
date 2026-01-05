using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private UI ui;
    public static event Action OnPlayerDeath;
    public PlayerInputSet inputSet { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX playerVFX { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHendler statusHendler { get; private set; }

    #region States variables
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
    public Player_SwordThrowState swordThrowState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    #endregion

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
    public Vector2 mousePositionInput { get; private set; }
    override protected void Awake()
    {
        base.Awake();
        skillManager = GetComponent<Player_SkillManager>();
        playerVFX = GetComponent<Player_VFX>();
        ui = FindAnyObjectByType<UI>();
        inputSet = new PlayerInputSet();
        health = GetComponent<Entity_Health>();
        statusHendler = GetComponent<Entity_StatusHendler>();

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
        swordThrowState = new Player_SwordThrowState(this, stateMachine, "swordThrow");
        deadState = new Player_DeadState(this, stateMachine, "dead");
    }
    void OnEnable()
    {
        inputSet.Enable();
        
        inputSet.Player.Mouse.performed += ctx => mousePositionInput = ctx.ReadValue<Vector2>();
        inputSet.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputSet.Player.Movement.canceled += ctx => movementInput = Vector2.zero;

        inputSet.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
        inputSet.Player.Spell.performed += ctx => skillManager.shardSkill.TryUseSkill();

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
    public void TeleportToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
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
