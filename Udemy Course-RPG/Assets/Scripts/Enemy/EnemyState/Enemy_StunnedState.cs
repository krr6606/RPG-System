using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    Enemy_VFX enemyVFX;
    public Enemy_StunnedState(Enemy enemy, StateMachin stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        DisableCounterWindow();
        stateTimer = enemy.stunnedTime;
        rb.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facingDir, enemy.stunnedVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer < 0f)
        {
           stateMachine.ChangeState(enemy.idleState);
        }
    }
    private void DisableCounterWindow()
    {
        enemyVFX.EnableCounterWindowVFX(false);
        enemy.EnableCounterWindow(false);
    }
}
