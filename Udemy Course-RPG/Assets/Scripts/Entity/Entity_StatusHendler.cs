using System.Collections;
using UnityEngine;

public class Entity_StatusHendler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entity_VFX;
    private Entity_Stat entity_Stat;
    private ElementType currentElementType = ElementType.None;

    void Awake()
    {
        entity = GetComponent<Entity>();
        entity_VFX = GetComponent<Entity_VFX>();
        entity_Stat = GetComponent<Entity_Stat>();
    }
    public void ApplyChilledEffect(float duration, float slowAmount)
    {
        float iceResistance = entity_Stat.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);

        StartCoroutine(CilledEffectCoroutine(reducedDuration, slowAmount));
    }
    public void ApplyBurnEffect(float duration, float totalDamage)
    {
        float fireResistance = entity_Stat.GetElementalResistance(ElementType.Fire);
        
        float reducedDamage = totalDamage * (1 - fireResistance);
        StartCoroutine(BurnEffectCoroutine(duration, reducedDamage));
    }
    private IEnumerator BurnEffectCoroutine(float duration, float totalDamage)
    {

        currentElementType = ElementType.Fire;
        entity_VFX.PlayStatusVFX(duration,ElementType.Fire);
        int ticksPerSecond = 2;
        float tickCount = Mathf.RoundToInt(duration * ticksPerSecond);
        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;
        for (int i = 0; i < tickCount; i++)
        {
            entity.GetComponent<Entity_Health>().TakeDamage(damagePerTick, 0, ElementType.Fire, entity.transform);
            yield return new WaitForSeconds(tickInterval);
        }

        currentElementType = ElementType.None;
    }
    private IEnumerator CilledEffectCoroutine(float duration, float slowAmount)
    {
        entity.SlowDownEntity(duration, slowAmount);
        currentElementType = ElementType.Ice;
        entity_VFX.PlayStatusVFX(duration,ElementType.Ice);
        yield return new WaitForSeconds(duration);
        currentElementType = ElementType.None;
    }
    public bool canBeApplied(ElementType elementType)
    {
        return currentElementType == ElementType.None;
    }
}
