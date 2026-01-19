using Unity.VisualScripting;
using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject echoPrefab;
    [SerializeField] private int echoDuration;

    [Header("Attack Setting")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = 0.3f;

    [Header("Heal Wisp Settings")]
    [SerializeField] private float damageHealPercentage = 0.2f;
    [SerializeField] private float cooldownReducedInSeconds;

    public float GetDamageHealPercentage()
    {
        if (ShouldBeWisp()== false)
        {
            return 0;
        }
        return damageHealPercentage;
    }

    public float GetCooldownReducedInSeconds()
    {
        if (skillUpgradeType == SkillUpgradeType.TimeEcho_CooldownWisp)
        {
            return 0;
        }
        return cooldownReducedInSeconds;
    }
    public bool CanRemoveNegativeEffects()
    {
        return skillUpgradeType == SkillUpgradeType.TimeEcho_CleanseWisp;
    }
    public bool ShouldBeWisp()
    {
        return skillUpgradeType == SkillUpgradeType.TimeEcho_HealWisp
            || skillUpgradeType == SkillUpgradeType.TimeEcho_CooldownWisp
            || skillUpgradeType == SkillUpgradeType.TimeEcho_CleanseWisp;

    }
    public float GetDuplicateChance()
    {
        if(skillUpgradeType != SkillUpgradeType.TimeEcho_ChanceToDuplicate)
        {
            return 0;
        }
        return duplicateChance;
    }

    public int GetMaxAttack()
    {
        if(skillUpgradeType == SkillUpgradeType.TimeEcho_SingleAttack ||skillUpgradeType == SkillUpgradeType.TimeEcho_ChanceToDuplicate)
        {
            return 1;
        }
        if(skillUpgradeType == SkillUpgradeType.TimeEcho_MultiAttack)
        {

            return maxAttacks;
        }

        return 0;
    }
    public override void TryUseSkill()
    {
        if (canUseSkill() == false)
            return;

        CreateTimeEcho();
    }
    public float GetEchoDuration()
    {
        return echoDuration;
    }
    public void CreateTimeEcho(Vector3? targetPos  = null)
    {
        Vector3 pos = targetPos ?? transform.position;
        GameObject echo = Instantiate(echoPrefab, pos, Quaternion.identity);
        echo.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);
    }
}
