using UnityEngine;
using System;
[CreateAssetMenu(fileName = "Skill Data", menuName = "ScriptableObjects/Skill Data - ", order = 1)]

public class Skill_DataSO : ScriptableObject
{

    [Header("Skill Info")]
    public string Name;
    [TextArea]
    public string Description;
    public Sprite Icon;

    [Header("unlock & upgrade")]
    public bool unlockedByDefault;
    public int cost;
    public UpgradeData upgradeData;
    public SkillType skillType;
 
}
[Serializable]
public class UpgradeData 
{
    public SkillUpgradeType upgradeType;
    public float cooldownTime;
    public DamageScaleData damageScaleData;
}