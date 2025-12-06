using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;

    public void RemoveSkillPoint(int cost)
    {
        skillPoints -= cost;
    }
    public void AddSkillPoint(int cost) {
        skillPoints += cost;
    }
    public bool HasEnoughPoints(int cost)
    {
        return skillPoints >= cost;
    }
}
