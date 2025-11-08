using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    public Enemy_IdleState(Enemy enemy, StateMachin stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        enemy.SetVelocity(0f, enemy.rb.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer <0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
