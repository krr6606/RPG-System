using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachin stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        if (enemy.wallDetected || !enemy.groundDetected)
        {
            enemy.Flip();
        }
    }
    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.linearVelocity.y);
        if (enemy.wallDetected || !enemy.groundDetected)
        {
            stateMachine.ChangeState(enemy.idleState);

        }
    }

}
