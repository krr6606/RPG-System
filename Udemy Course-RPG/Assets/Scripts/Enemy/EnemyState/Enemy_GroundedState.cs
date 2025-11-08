using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachin stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    override public void Update()
    {
        base.Update();
        if(enemy.playerDetected() == true)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
