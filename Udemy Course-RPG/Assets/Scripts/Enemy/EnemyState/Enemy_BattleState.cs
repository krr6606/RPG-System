using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
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
            enemy.SetVelocity(enemy.retreatVelocity.x * -facingDirToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(facingDirToPlayer());
        }



    }
    public override void Update()
    {
        base.Update();
        if(enemy.playerDetected() == true)
        {
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
            enemy.SetVelocity(enemy.battleMoveSpeed * facingDirToPlayer(), rb.linearVelocity.y);
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


            return player.position.x > enemy.transform.position.x ? 1 : -1;

    }
}
