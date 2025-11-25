using UnityEngine;

public abstract class PlayerState : EntityState

{
    protected Player player;
    protected PlayerInputSet playerInputSet;

 

    public PlayerState(Player player, StateMachin stateMachine, string animBoolName): base(stateMachine, animBoolName)
    {

        this.player = player;

        animator = player.animator;
        rb = player.rb;
        playerInputSet = player.inputSet;
        entityStat = player.entityStat;
    }

        public override void Update()
    {
        base.Update();
        if (playerInputSet.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }



    private bool CanDash()
    {
        if(player.wallDetected)
            return false;
        if(stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
    public override void UpdateAinmationParameters()
    {
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }
}
