using NUnit;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask EnemyMask;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 0.5f;

    protected Entity_Stat entityStat;
    protected DamageScaleData damageScaleData;
    protected ElementType usedELementType;
    protected Collider2D[] EnemiesAround(Transform transform, float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, EnemyMask);
    }
    protected void DamageEnemiesInRadius(Transform transform, float radius)
    {
        foreach (var enemy in EnemiesAround(transform, radius))
        {
            IDamagable damagable = enemy.GetComponent<IDamagable>();
            if(damagable == null)
                continue;
            float pyhsicalDamage = entityStat.GetPhysicalDamage(out bool isCrit,damageScaleData.physicalDamageScale);
            float elementalDamage = entityStat.GetElementalDamage(out ElementType elementType, damageScaleData.elementalDamageScale);

            damagable.TakeDamage(pyhsicalDamage, elementalDamage, elementType,transform);
            if(elementType != ElementType.None)
            {
                ElementalEffectData elementalEffectData = new ElementalEffectData(entityStat, damageScaleData);
                enemy.GetComponent<Entity_StatusHendler>()?.ApplyStatusEffect(elementType, elementalEffectData);
            }
            usedELementType = elementType;
        }
    }
    protected Transform TargetTracking()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in EnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
            return target;
    }
    protected virtual void OnDrawGizmosSelected()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
