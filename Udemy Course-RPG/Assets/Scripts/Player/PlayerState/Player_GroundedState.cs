using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();
        if(rb.linearVelocity.y < 0 && player.groundDetected == false)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if(playerInputSet.Player.jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        if (playerInputSet.Player.Attak.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttackState);
        }
        if(playerInputSet.Player.CountAttack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
    }
}
