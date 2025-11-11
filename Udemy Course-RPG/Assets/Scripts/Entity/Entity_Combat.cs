using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10f;
    Entity_VFX entityVFX;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask targetLayer;
    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
    }
    public void performAttack()
    {
        
        foreach (var target in GetDetectedTargets())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable ==null) continue;
            damagable.TakeDamage(damage, transform);

            entityVFX.CreateOnDoingDamageVFX(target.transform);
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
