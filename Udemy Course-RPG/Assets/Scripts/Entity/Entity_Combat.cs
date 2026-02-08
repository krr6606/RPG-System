using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;

    Entity_VFX entityVFX;
    Entity_Stats entityStat;

    public DamageScaleData basicAttackScale;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask targetLayer;

    
    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entityStat = GetComponent<Entity_Stats>();
    }
    public void performAttack()
    {

        foreach (var target in GetDetectedTargets())
        {
            if (!target.TryGetComponent<IDamagable>(out var damagable)) continue;

            AttackData attackData = entityStat.AttackData(basicAttackScale);


            bool targetGoHit = damagable.TakeDamage(attackData.pyhsicalDamage, attackData.elementalDamage,attackData.elementType, transform);
            if(attackData.elementType != ElementType.None)
            {
                target.GetComponent<Entity_StatusHendler>()?.ApplyStatusEffect(attackData.elementType, attackData.effectData);
            }
            if (!targetGoHit) return;
            OnDoingPhysicalDamage?.Invoke(attackData.pyhsicalDamage);
            entityVFX.CreateOnHitVFX(target.transform, attackData.isCrit, attackData.elementType);
        }
    }

    protected Collider2D[] GetDetectedTargets()
    {
      return  Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, targetLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
