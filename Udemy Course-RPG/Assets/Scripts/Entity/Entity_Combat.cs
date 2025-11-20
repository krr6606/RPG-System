using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10f;
    Entity_VFX entityVFX;
    Entity_Stat entityStat;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask targetLayer;
    [Header("Status Effect Settings")]
    [SerializeField] private float chillDuration = 2f;
    [SerializeField] private float chillSlowAmount = 0.3f;
    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entityStat = GetComponent<Entity_Stat>();
    }
    public void performAttack()
    {
        
        foreach (var target in GetDetectedTargets())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable ==null) continue;

            bool isCrit = false; 
            float elementalDamage = entityStat.GetElementalDamage(out ElementType elementType);
            bool targetGoHit = damagable.TakeDamage(entityStat.GetPhysicalDamage(out isCrit), elementalDamage,elementType, transform);
            if(elementType != ElementType.None)
            {
                ApplyStatusEffect(target.transform, elementType);
            }
            if (!targetGoHit) return;
            entityVFX.UpdateOnVfxColor(elementType);
            entityVFX.CreateOnHitVFX(target.transform, isCrit);
        }
    }
    public void ApplyStatusEffect(Transform target,ElementType elementType)
    {
        Entity_StatusHendler statusHendler = target.GetComponent<Entity_StatusHendler>();
        if(statusHendler == null) return;
        switch(elementType)
        {
            case ElementType.Ice:
                if(statusHendler.canBeApplied(elementType))
                {
                    statusHendler.ApplyChilledEffect(chillDuration, chillSlowAmount);
                }
                break;
                // Add other element types here
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
