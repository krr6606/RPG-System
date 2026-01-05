using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    public Player_SwordThrowState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.skillManager.swordThrowSkill.EnableDots(true);
    }
    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.linearVelocity.y);
        player.HandleFlip(dirToMouse.x);
        player.skillManager.swordThrowSkill.PredictTrajectory(dirToMouse);

        if (playerInputSet.Player.Attak.WasPressedThisFrame())
        {
            animator.SetBool("swordThrowPerformed", true);
            player.skillManager.swordThrowSkill.ConfirmTrajectory(dirToMouse);
            player.skillManager.swordThrowSkill.EnableDots(false);
        }
        if (playerInputSet.Player.RangeAttack.WasReleasedThisFrame() || triggerCalled)
        {
            player.skillManager.swordThrowSkill.EnableDots(false);
            stateMachine.ChangeState(player.idleState);

        }
    }
    override public void Exit()
    {
        base.Exit();
        player.skillManager.swordThrowSkill.EnableDots(false);
        animator.SetBool("swordThrowPerformed", false);
    }
    private Vector2 DirectionToMouse()
    {
        Vector2 mouseWorldPosition = player.transform.position;
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(player.mousePositionInput);

        Vector2 direction = worldMousePosition - mouseWorldPosition;
        return direction.normalized;
    }
}
