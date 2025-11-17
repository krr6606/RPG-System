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
            if (!targetGoHit) return;
            entityVFX.CreateOnHitVFX(target.transform, isCrit);
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
