using System.Collections;
using UnityEngine;

public class Entity_StatusHendler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entity_VFX;
    private Entity_Health entity_Health;
    private Entity_Stat entity_Stat;
    private ElementType currentElementType = ElementType.None;
    [Header("Elemental Status Effects Settings")]
    [SerializeField] private GameObject lightningVFXPrefab;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maxCharge = 1;
    private Coroutine electricCoroutine;

    void Awake()
    {
        entity = GetComponent<Entity>();
        entity_Health = GetComponent<Entity_Health>();
        entity_VFX = GetComponent<Entity_VFX>();
        entity_Stat = GetComponent<Entity_Stat>();
    }
    public void ApplyChillEffect(float duration, float slowAmount)
    {
        float iceResistance = entity_Stat.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);

        StartCoroutine(CillEffectCoroutine(reducedDuration, slowAmount));
    }
    
    public void ApplyBurnEffect(float duration, float totalDamage)
    {
        float fireResistance = entity_Stat.GetElementalResistance(ElementType.Fire);
        
        float reducedDamage = totalDamage * (1 - fireResistance);
        StartCoroutine(BurnEffectCoroutine(duration, reducedDamage));
    }
    public void ApplyElectricEffect(float duration ,float damage, float charge)
    {
        float energyResistance = entity_Stat.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - energyResistance);
        currentCharge += finalCharge;
        if (currentCharge >= maxCharge)
        {
            DoLightningStrike(damage);
            StopElectricEffect();
            return;
        }
        if(electricCoroutine != null)
        {
            StopCoroutine(electricCoroutine);
        }
        electricCoroutine = StartCoroutine(ElectricEffectCoroutine(duration));
    }
    private void StopElectricEffect()
    {
        currentElementType = ElementType.None;
        currentCharge = 0;
        entity_VFX.StopStatusVFX();
    }
    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningVFXPrefab, entity.transform.position, Quaternion.identity);
        entity_Health.ReduceHP(damage);

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
    private IEnumerator CillEffectCoroutine(float duration, float slowAmount)
    {
        entity.SlowDownEntity(duration, slowAmount);
        currentElementType = ElementType.Ice;
        entity_VFX.PlayStatusVFX(duration,ElementType.Ice);
        yield return new WaitForSeconds(duration);
        currentElementType = ElementType.None;
    }
    private IEnumerator ElectricEffectCoroutine(float duration)
    {
        currentElementType = ElementType.Lightning;
        entity_VFX.PlayStatusVFX(duration,ElementType.Lightning);
        yield return new WaitForSeconds(duration);
        StopElectricEffect();
    }
    public bool canBeApplied(ElementType elementType)
    {
        if(elementType ==ElementType.Lightning && currentElementType == ElementType.Lightning)
        {
            return true;
        }
        return currentElementType == ElementType.None;
    }
}
