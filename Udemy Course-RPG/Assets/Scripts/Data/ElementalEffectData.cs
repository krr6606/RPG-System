using System;
using UnityEngine;
[Serializable]
public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowAmount;
    
    public float burnDuration;
    public float burnTotalDamage;

    public float electricDuration;
    public float electricDamage;
    public float electricCharge;

    public ElementalEffectData(Entity_Stats entity_Stat,DamageScaleData damageScaleData)
    {
        chillDuration = damageScaleData.chillDuration;
        chillSlowAmount = damageScaleData.chillSlowAmountScale;

        burnTotalDamage = entity_Stat.offenceStats.fireDamage.GetValue() * damageScaleData.elementalDamageScale;
        burnDuration = damageScaleData.burnDuration;

        electricDamage = entity_Stat.offenceStats.lightningDamage.GetValue() * damageScaleData.elementalDamageScale;
        electricDuration = damageScaleData.electricDuration;
        electricCharge = damageScaleData.electricCharge;
    }
}
