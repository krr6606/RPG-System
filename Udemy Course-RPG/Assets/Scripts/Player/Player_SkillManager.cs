using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{

    public Skill_Dash dashSkill { get; private set; }
    public Skill_Shard shardSkill { get; private set; }
    public Skill_SwordThrow swordThrowSkill { get; private set; }

    private void Awake()
    {
        dashSkill = GetComponentInChildren<Skill_Dash>();
        shardSkill = GetComponentInChildren<Skill_Shard>();
        swordThrowSkill = GetComponentInChildren<Skill_SwordThrow>();
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash:
                return dashSkill;
            case SkillType.TimeEcho:
                Debug.LogWarning("Time Echo skill is not implemented yet.");
                return null;
            case SkillType.TimeShard:
                return shardSkill;
            case SkillType.SwordThrow:
                return swordThrowSkill;
            default:
                Debug.LogWarning("Skill Type not found: " + type);
                return null;
        }
    }
}
