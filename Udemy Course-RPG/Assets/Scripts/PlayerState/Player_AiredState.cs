using UnityEngine;

public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        // Add specific logic for entering the aired state here
    }
    public override void Update()
    {
        base.Update();
        // Add specific logic for updating the aired state here
        if(player.movementInput.x != 0)
        {
            player.SetVelocity(player.movementInput.x * (player.movementSpeed * player.inAirMovementMultiplier) , player.rb.linearVelocity.y);
        }
        if(playerInputSet.Player.Attak.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);
        }
    }
}
