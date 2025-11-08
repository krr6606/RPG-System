using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        HendleWallSlide();
        if (playerInputSet.Player.jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }

        if(!player.wallDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            if(player.facingDir != player.movementInput.x)
            player.Flip();
        }

    }
    private void HendleWallSlide()
    {
        if(player.movementInput.y < 0)
        {
            player.SetVelocity(player.movementInput.x,rb.linearVelocity.y);
        }
        else
        {
            player.SetVelocity(player.movementInput.x,rb.linearVelocity.y * player.wallSlideSlowMultiplier);
        }
    }
}
