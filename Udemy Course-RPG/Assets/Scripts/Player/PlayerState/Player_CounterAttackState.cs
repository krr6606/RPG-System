using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat playerCombat;
    bool Countered;
    public Player_CounterAttackState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        playerCombat = player.GetComponent<Player_Combat>();
    }
    override public void Enter()
    {
        base.Enter();
        stateTimer = playerCombat.GetCounterRecoveryDuration(); 
        Countered = playerCombat.CounterAttackPerformed();
        animator.SetBool("counterAttackPerformed", Countered);

    }
    override public void Update()
    {
        base.Update();
        player.SetVelocity(0, rb.linearVelocity.y);
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (stateTimer < 0f&& !Countered)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
