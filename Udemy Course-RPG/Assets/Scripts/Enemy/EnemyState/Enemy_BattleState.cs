using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private Transform lastTarget;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachin stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        UpdateLastTimeInBattle();

            player ??= enemy.GetPlayerReference();

        if(ShouldRetreat())
        {
            enemy.SetVelocity((enemy.retreatVelocity.x * enemy.activeSlowMultiplier) * -facingDirToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(facingDirToPlayer());
        }



    }
    public override void Update()
    {
        base.Update();
        if(enemy.playerDetected() == true)
        {
            UpdateTargetIfNeeded();
            UpdateLastTimeInBattle();
        }
        if(BattleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }
        if (isInAttackRange && enemy.playerDetected())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            enemy.SetVelocity(enemy.GetBattleMoveSpeed() * facingDirToPlayer(), rb.linearVelocity.y);
        }
    }
    private void UpdateTargetIfNeeded()
    {
        Transform newTarget = enemy.playerDetected().transform;
        if(newTarget != lastTarget )
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }
    private void UpdateLastTimeInBattle()
    {
            lastTimeWasInBattle = Time.time;
    }
    private bool ShouldRetreat()
    {
        return DistanceToPlayer() < enemy.minimumDistanceToPlayer;
    }
    private bool BattleTimeIsOver()
    {
        return Time.time > enemy.battleTimeDuration + lastTimeWasInBattle;
    }
    private bool isInAttackRange => DistanceToPlayer() < enemy.attackDistance;
    private float DistanceToPlayer()
    {
        if (player == null)
        {
            return Mathf.Infinity;
        }
        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
    private int facingDirToPlayer()
    {
        if(player == null || enemy == null)
            return 0;
        else
            return player.position.x > enemy.transform.position.x ? 1 : -1;

    }
}
