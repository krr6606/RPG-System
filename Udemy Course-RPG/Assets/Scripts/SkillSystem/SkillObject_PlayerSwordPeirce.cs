using UnityEngine;

public class SkillObject_PlayerSwordPeirce : SkillObject_PlayerSword
{
    private int amountToPierce;
    override public void SetupSword(Skill_SwordThrow skill_SwordThrowMgr, Vector2 dir)
    {
        base.SetupSword(skill_SwordThrowMgr, dir);
        amountToPierce = skill_SwordThrowMgr.pierceAmount;
    }
    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (amountToPierce <= 0 || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            stopSword(collision);
        }
        else
        {
            amountToPierce--;
        }
        DamageEnemiesInRadius(transform, 0.3f);
    }
}
