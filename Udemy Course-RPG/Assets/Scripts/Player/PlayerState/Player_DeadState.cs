using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    override public void Enter()
    {
        base.Enter();
        player.inputSet.Disable();
        rb.simulated = false;
    }
}
