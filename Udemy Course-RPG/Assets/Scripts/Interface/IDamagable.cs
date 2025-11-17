using UnityEngine;

public interface IDamagable 
{
    public bool TakeDamage(float damageAmount, float elementalDamage, ElementType elementType, Transform damageDealer);
}
