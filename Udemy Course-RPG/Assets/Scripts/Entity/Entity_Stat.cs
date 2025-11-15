using UnityEngine;

public class Entity_Stat : MonoBehaviour
{
    public Stat MaxHP;

    public Stat_MagorGroup majorStats;
    public Stat_OffenceGroup offenceStats;
    public Stat_DefenceGroup defenceStats;

    public float GetPhysicalDamage(out bool isCriticalHit)
    {
        float basePhysicalDamage = offenceStats.damage.GetBaseValue();
        float strengthBonus = majorStats.strength.GetBaseValue() * 2;
        float totalPhysicalDamage = basePhysicalDamage + strengthBonus;

        float baseCriticalChance = offenceStats.critChance.GetBaseValue();
        float bonusCriticalChance = majorStats.agility.GetBaseValue() * 0.3f;
        float totalCriticalChance = baseCriticalChance + bonusCriticalChance;

        float baseCriticalPower = offenceStats.critPower.GetBaseValue();
        float bonusCriticalPower = majorStats.strength.GetBaseValue() * 0.5f;
        float totalCriticalPower = (baseCriticalPower + bonusCriticalPower)/100;
        isCriticalHit = Random.value < (totalCriticalChance / 100f);
        float fianlDamage = isCriticalHit ? basePhysicalDamage * totalCriticalPower : totalPhysicalDamage;

        return fianlDamage;
    }
    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmour = defenceStats.armor.GetBaseValue();
        float strengthBonus = majorStats.strength.GetBaseValue();
        float totalArmour = baseArmour + strengthBonus;

        float reductionMutliplier = Mathf.Clamp01(1 - armorReduction);
        float effectiveArmour = totalArmour * reductionMutliplier;

        float armourMitigation = totalArmour / (effectiveArmour + 100); // Example formula for mitigation (e.g 100 armour = 50% mitigation)

        float mitigationCap = 0.9f;
        armourMitigation = Mathf.Min(armourMitigation, mitigationCap);
        return armourMitigation;
    }
    public float GetArmorReduction()
    {
        float finalArmorReduction = offenceStats.armorReduction.GetBaseValue()/100;

        return finalArmorReduction;
    }
    public float GetMaxHP()
    {
        float baseHP = MaxHP.GetBaseValue();
        float vitalityBonus = majorStats.vitality.GetBaseValue() * 5;
        float finalMaxHP = baseHP + vitalityBonus;
        return finalMaxHP;
    }
    public float GetEvasion()
    {
        float baseEvasion = defenceStats.evasion.GetBaseValue();
        float agilityBonus = majorStats.agility.GetBaseValue() * 0.5f;
        float totalEvasion = baseEvasion + agilityBonus;
        float evasionCap = 75f;
        float finalEvasion = Mathf.Min(totalEvasion, evasionCap);
        return finalEvasion;
    }
}
