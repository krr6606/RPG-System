using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10f;
    Entity_VFX entityVFX;
    Entity_Stat entityStat;

    public DamageScaleData basicAttackScale;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask targetLayer;
    [Header("Status Effect Settings")]
    [SerializeField] private float chillDuration = 2f;
    [SerializeField] private float chillSlowAmount = 0.3f;
    [SerializeField] private float burnDuration = 3f;
    [SerializeField] private float electricChargeBulldUp = 0.4f;
    [Range(0f, 1f)]
    [SerializeField] private float burnDamageScale = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float electricDamageScale = 1.4f;
    
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

            ElementalEffectData elementalEffectData = new ElementalEffectData(entityStat, basicAttackScale);

            bool isCrit = false; 
            float elementalDamage = entityStat.GetElementalDamage(out ElementType elementType);
            bool targetGoHit = damagable.TakeDamage(entityStat.GetPhysicalDamage(out isCrit), elementalDamage,elementType, transform);
            if(elementType != ElementType.None)
            {
                target.GetComponent<Entity_StatusHendler>()?.ApplyStatusEffect(elementType, elementalEffectData);
            }
            if (!targetGoHit) return;

            entityVFX.CreateOnHitVFX(target.transform, isCrit,elementType);
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
