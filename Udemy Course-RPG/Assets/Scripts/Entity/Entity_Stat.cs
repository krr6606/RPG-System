using UnityEngine;

public class Entity_Stat : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;

    public Stat_ResourceGroup statResourceGroup;

    public Stat_MagorGroup majorStats;
    public Stat_OffenceGroup offenceStats;
    public Stat_DefenceGroup defenceStats;

    public float GetPhysicalDamage(out bool isCriticalHit, float scaleFator =1)
    {
        float basePhysicalDamage = offenceStats.damage.GetValue();
        float strengthBonus = majorStats.strength.GetValue() * 2;
        float totalPhysicalDamage = basePhysicalDamage + strengthBonus;

        float baseCriticalChance = offenceStats.critChance.GetValue();
        float bonusCriticalChance = majorStats.agility.GetValue() * 0.3f;
        float totalCriticalChance = baseCriticalChance + bonusCriticalChance;

        float baseCriticalPower = offenceStats.critPower.GetValue();
        float bonusCriticalPower = majorStats.strength.GetValue() * 0.5f;
        float totalCriticalPower = (baseCriticalPower + bonusCriticalPower)/100;
        isCriticalHit = Random.value < (totalCriticalChance / 100f);
        float fianlDamage = isCriticalHit ? basePhysicalDamage * totalCriticalPower : totalPhysicalDamage;

        return fianlDamage * scaleFator;
    }
    public float GetElementalDamage(out ElementType elementType, float scaleFactor = 1)
    {
        float fireDamage = offenceStats.fireDamage.GetValue();
        float iceDamage = offenceStats.iceDamage.GetValue();
        float lightningDamage = offenceStats.lightningDamage.GetValue();
        
        float bonusIntelligenceDamage = majorStats.intelligence.GetValue();

        float highestElementalDamage = fireDamage;
        elementType = ElementType.Fire;
        if(iceDamage > highestElementalDamage)
        {
            highestElementalDamage = iceDamage;
            elementType = ElementType.Ice;
        }
        if(lightningDamage > highestElementalDamage)
        {
            highestElementalDamage = lightningDamage;
            elementType = ElementType.Lightning;
        }
        if (highestElementalDamage == 0)
        {
            elementType = ElementType.None;
            return 0;
        }
        float bonusFireDamage = elementType == ElementType.Fire ? 0 : fireDamage * 0.5f ;
        float bonusIceDamage = elementType == ElementType.Ice ? 0 : iceDamage * 0.5f;      
        float bonusLightningDamage = elementType == ElementType.Lightning ? 0 : lightningDamage * 0.5f;
        float weakerElementsDamage = bonusFireDamage + bonusIceDamage + bonusLightningDamage;
        float finalElementalDamage = highestElementalDamage + bonusIntelligenceDamage + weakerElementsDamage;
        return finalElementalDamage * scaleFactor;
    }
    public float GetElementalResistance(ElementType elementType)
    {
        float baseResistance = 0f;
        float intelligenceBonus = majorStats.intelligence.GetValue() * 0.5f;
        switch (elementType)
        {
            case ElementType.Fire:
                baseResistance = defenceStats.fireResist.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defenceStats.iceResist.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defenceStats.lightningResist.GetValue();
                break;
            default:
                return 0f;
        }
        float finalResistance = baseResistance + intelligenceBonus;
        float ResistanceCap = 75f;
        finalResistance = Mathf.Min(finalResistance, ResistanceCap);
        return finalResistance;
    }
    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmour = defenceStats.armor.GetValue();
        float strengthBonus = majorStats.strength.GetValue();
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
        float finalArmorReduction = offenceStats.armorReduction.GetValue()/100;

        return finalArmorReduction;
    }
    public float GetMaxHP()
    {
        float baseHP = statResourceGroup.maxHealth.GetValue();
        float vitalityBonus = majorStats.vitality.GetValue() * 5;
        float finalMaxHP = baseHP + vitalityBonus;
        return finalMaxHP;
    }
    public float GetEvasion()
    {
        float baseEvasion = defenceStats.evasion.GetValue();
        float agilityBonus = majorStats.agility.GetValue() * 0.5f;
        float totalEvasion = baseEvasion + agilityBonus;
        float evasionCap = 75f;
        float finalEvasion = Mathf.Min(totalEvasion, evasionCap);
        return finalEvasion;
    }
    public Stat GetStatByType(StatType statType)
    {
        var stat = statType switch
        {
            StatType.MaxHealth => statResourceGroup.maxHealth,
            StatType.healthRegen => statResourceGroup.healthRegen,
            StatType.Strangth => majorStats.strength,
            StatType.Agility => majorStats.agility,
            StatType.Intelligence => majorStats.intelligence,
            StatType.Vitality => majorStats.vitality,
            StatType.AtackSpeed => offenceStats.attackSpeed,
            StatType.Damage => offenceStats.damage,
            StatType.CritChance => offenceStats.critChance,
            StatType.CritPower => offenceStats.critPower,
            StatType.ArmorReduction => offenceStats.armorReduction,
            StatType.FireDamage => offenceStats.fireDamage,
            StatType.IceDamage => offenceStats.iceDamage,
            StatType.LightningDamage => offenceStats.lightningDamage,
            StatType.Armor => defenceStats.armor,
            StatType.Evasion => defenceStats.evasion,
            StatType.IceResistance => defenceStats.iceResist,
            StatType.FireResistance => defenceStats.fireResist,
            StatType.LightningResistance => defenceStats.lightningResist,
            _ => null,
        };
        if (stat == null)
        {
            Debug.LogWarning($"StatType {statType} not found in Entity_Stat.");
        }
        return stat;
    }
    [ContextMenu("Apply Default Stat Setup")]
    public void ApplyDefaultSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.LogWarning("Default Stat Setup is not assigned.");
            return;
        }
        statResourceGroup.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        statResourceGroup.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        offenceStats.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offenceStats.damage.SetBaseValue(defaultStatSetup.Damage);
        offenceStats.critChance.SetBaseValue(defaultStatSetup.critChance);
        offenceStats.critPower.SetBaseValue(defaultStatSetup.critPower);
        offenceStats.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);
        offenceStats.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offenceStats.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offenceStats.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defenceStats.armor.SetBaseValue(defaultStatSetup.armor);
        defenceStats.evasion.SetBaseValue(defaultStatSetup.evasion);
        defenceStats.fireResist.SetBaseValue(defaultStatSetup.fireResist);
        defenceStats.iceResist.SetBaseValue(defaultStatSetup.iceResist);
        defenceStats.lightningResist.SetBaseValue(defaultStatSetup.lightningResist);

        majorStats.strength.SetBaseValue(defaultStatSetup.strength);
        majorStats.agility.SetBaseValue(defaultStatSetup.agility);
        majorStats.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        majorStats.vitality.SetBaseValue(defaultStatSetup.vitality);

    }
}
