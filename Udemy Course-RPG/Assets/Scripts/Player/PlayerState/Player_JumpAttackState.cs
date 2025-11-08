using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private bool touchGround;
    public Player_JumpAttackState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.linearVelocity.x + player.jumpAttackVelocity.x * player.facingDir, rb.linearVelocity.y + player.jumpAttackVelocity.y);
        touchGround = false;
    }
    public override void Update()
    {
        base.Update();

        if (player.groundDetected && touchGround == false)
        {
            touchGround = true;
            player.SetVelocity(0, player.rb.linearVelocity.y);
            animator.SetTrigger("jumpAttackTrigger");

        }

        if (triggerCalled && player.groundDetected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);

        }
    }
}
