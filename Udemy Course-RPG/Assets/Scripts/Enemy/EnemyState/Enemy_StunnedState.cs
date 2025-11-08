using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    public Enemy_StunnedState(Enemy enemy, StateMachin stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}
