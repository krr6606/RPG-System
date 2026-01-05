using NUnit;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject onHitVFX;
    [SerializeField] protected LayerMask EnemyMask;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 0.5f;

    protected Entity_Stat entityStat;
    protected DamageScaleData damageScaleData;
    protected ElementType usedELementType;
    protected bool targetGotHit; 
    protected Collider2D[] EnemiesAround(Transform transform, float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, EnemyMask);
    }
    protected void DamageEnemiesInRadius(Transform transform, float radius)
    {
        foreach (var target in EnemiesAround(transform, radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if(damagable == null)
                continue;
            AttackData attackData = entityStat.AttackData(damageScaleData);



            targetGotHit = damagable.TakeDamage(attackData.pyhsicalDamage, attackData.elementalDamage, attackData.elementType,transform);
            if (attackData.elementType != ElementType.None)
            {
                target.GetComponent<Entity_StatusHendler>()?.ApplyStatusEffect(attackData.elementType, attackData.effectData);
            }
            if (targetGotHit)
            {
                Instantiate(onHitVFX, target.transform.position, Quaternion.identity);
            }

            usedELementType = attackData.elementType;
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
