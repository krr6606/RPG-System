using UnityEngine;

public abstract class PlayerState : EntityState

{
    protected Player player;
    protected Player_SkillManager skillManager;
    protected PlayerInputSet playerInputSet;

 

    public PlayerState(Player player, StateMachin stateMachine, string animBoolName): base(stateMachine, animBoolName)
    {

        this.player = player;

        animator = player.animator;
        rb = player.rb;
        playerInputSet = player.inputSet;
        entityStat = player.entityStat;
        skillManager = player.skillManager;
    }

        // 언제든 스킬을 쓸 수 있는 상태
        public override void Update()
    {
        base.Update();
        if (playerInputSet.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dashSkill.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }
        // 궁극기
        if (playerInputSet.Player.UltimateSpell.WasPressedThisFrame() && skillManager.Skill_DomainExpansion.canUseSkill())
        {
            // 영역 전개 즉시 발동일 경우
            if (skillManager.Skill_DomainExpansion.InstantDomain())
            {
                skillManager.Skill_DomainExpansion.CreateDomain();
            }
            else
            {
                stateMachine.ChangeState(player.ultimateAbilityState);
            }
            skillManager.Skill_DomainExpansion.SetSkillOnCooldown();

        }
    }



    private bool CanDash()
    {
        if(skillManager.dashSkill.canUseSkill() == false)
            return false;
        else if (player.wallDetected)
            return false;
        else if (stateMachine.currentState == player.dashState)
            return false;
        else if(stateMachine.currentState == player.dashState ||stateMachine.currentState == player.ultimateAbilityState)
            return false;

    return true;
    }
    public override void UpdateAinmationParameters()
    {
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }
}
