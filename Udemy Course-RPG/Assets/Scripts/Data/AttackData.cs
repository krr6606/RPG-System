using UnityEngine;
using System;
[Serializable]
public class AttackData
{
    public float pyhsicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType elementType;

    public ElementalEffectData effectData;

    public AttackData(Entity_Stats entity_Stat, DamageScaleData damageScaleData)
    {
        pyhsicalDamage = entity_Stat.GetPhysicalDamage(out isCrit, damageScaleData.physicalDamageScale);
        elementalDamage = entity_Stat.GetElementalDamage(out elementType ,damageScaleData.elementalDamageScale);

        effectData = new ElementalEffectData(entity_Stat, damageScaleData);
    }
}
