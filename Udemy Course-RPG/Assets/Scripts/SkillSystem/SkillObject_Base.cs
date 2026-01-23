using NUnit;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject onHitVFX;
    [SerializeField] protected LayerMask EnemyMask;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 0.5f;
    
    protected Animator animator;
    protected Rigidbody2D rb;

    protected Entity_Stats entityStat;
    protected DamageScaleData damageScaleData;
    protected ElementType usedELementType;
    protected bool targetGotHit;
    protected Transform lastTarget;
    
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    protected Collider2D[] GetEnemiesAround(Transform transform, float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, EnemyMask);
    }
    protected void DamageEnemiesInRadius(Transform transform, float radius)
    {
        foreach (var target in GetEnemiesAround(transform, radius))
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
                lastTarget = target.transform;
                Instantiate(onHitVFX, target.transform.position, Quaternion.identity);
            }

            usedELementType = attackData.elementType;
        }
    }
    protected Transform TargetTracking()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in GetEnemiesAround(transform, 10))
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
