using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{

    public Skill_Dash dashSkill { get; private set; }
    public Skill_Shard shardSkill { get; private set; }
    public Skill_SwordThrow swordThrowSkill { get; private set; }
    public Skill_TimeEcho timeEchoSkill { get; private set; }
    public Skill_DomainExpansion Skill_DomainExpansion { get; private set; }

    private Skill_Base[] allSkills;
    private void Awake()
    {
        dashSkill = GetComponentInChildren<Skill_Dash>();
        shardSkill = GetComponentInChildren<Skill_Shard>();
        swordThrowSkill = GetComponentInChildren<Skill_SwordThrow>();
        timeEchoSkill = GetComponentInChildren<Skill_TimeEcho>();
        allSkills = GetComponentsInChildren<Skill_Base>();
        Skill_DomainExpansion = GetComponentInChildren<Skill_DomainExpansion>();
    }

    public void ReduceAllSkillCooldowns(float reductionAmount)
    {
        foreach (var skill in allSkills)
        {
            // Assuming Skill_Base has a method to reduce cooldown
            skill.ReduceCooldownBy(reductionAmount);
        }
    }
    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash:
                return dashSkill;
                return null;
            case SkillType.TimeShard:
                return shardSkill;
            case SkillType.SwordThrow:
                return swordThrowSkill;
            case SkillType.TimeEcho:
                return timeEchoSkill;
            case SkillType.DomainExpansion:
                return Skill_DomainExpansion;
            default:
                Debug.LogWarning("Skill Type not found: " + type);
                return null;
        }
    }
}
