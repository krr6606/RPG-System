using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale;
    private int dashDir;
    public Player_DashState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        skillManager.dashSkill.OnStartEffect();
        player.playerVFX.StartImageEchoEffect(player.dashDuration);
        dashDir = (player.movementInput.x != 0) ? (int)Mathf.Sign(player.movementInput.x) : player.facingDir; ;
        stateTimer = player.dashDuration;
        originalGravityScale = player.rb.gravityScale;
        rb.gravityScale = 0;
        player.health.SetCanTakeDamage(false);
    }
    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        player.SetVelocity(player.dashSpeed * dashDir, 0);
        if (stateTimer < 0 || !player.canDash)
        {
            if(player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
    public override void Exit()
    {
        base.Exit();

        skillManager.dashSkill.OnEndEffect();

        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
        player.health.SetCanTakeDamage(true);
    }
    private void CancelDashIfNeeded()
    {
        if(player.wallDetected )
        {
            if(player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
