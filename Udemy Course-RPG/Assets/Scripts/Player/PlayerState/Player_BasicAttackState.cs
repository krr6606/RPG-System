using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    private float lastTimeAttacked;

    private const int firstComboIndex = 1;
    private int ComboIndex = firstComboIndex;
    private int ComboLimit = 3;
    private bool comboAttackQueued;
    private int attackDir;
    public Player_BasicAttackState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if(ComboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("Combo Limit and Attack Velocity array length do not match. Adjusting Combo Limit to match array length.");
            ComboLimit = player.attackVelocity.Length;
        }
    }
    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed(); 
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();

        attackDir = (player.movementInput.x != 0) ? (int)Mathf.Sign(player.movementInput.x): player.facingDir;

        animator.SetInteger("basicAttackIndex", ComboIndex);
        ApplyAttackVelocity();
    }
    public override void Update()
    {
        base.Update();
        HendleAttackVelocity();
        if (playerInputSet.Player.Attak.WasPressedThisFrame())
        {
            QueueNextAttack();
        }
        if (triggerCalled)
        {
            HendleStateExit();
        }
    }
    public override void Exit()
    {
        base.Exit();
        lastTimeAttacked = Time.time;
        ComboIndex++;
    }
    void HendleStateExit()
    {
        if (comboAttackQueued)
        {
            animator.SetBool(AnimBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }
    private void QueueNextAttack()
    {
        if (ComboIndex < ComboLimit)
        {
            comboAttackQueued = true;
        }
    }
    void ResetComboIndexIfNeeded()
    {
        if (ComboIndex > ComboLimit || Time.time > lastTimeAttacked + player.comboResetTime)
        {
            ComboIndex = firstComboIndex;
        }
    }
    private void HendleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
        if(attackVelocityTimer < 0)
        {
        player.SetVelocity(0, rb.linearVelocity.y);
        }
    }
    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[ComboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
}
