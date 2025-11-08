using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (player.wallDetected)
        {
                       stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
