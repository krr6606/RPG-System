using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachin stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, player.rb.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if(player.movementInput.x == player.facingDir && player.wallDetected)
            return;
        // Add specific logic for updating the idle state here
        if (player.movementInput.x != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }

    }
    public override void Exit()
    {
        base.Exit();
        // Add specific logic for exiting the idle state here
    }
}

