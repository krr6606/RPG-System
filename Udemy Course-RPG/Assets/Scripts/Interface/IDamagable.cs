using UnityEngine;

public interface IDamagable 
{
    public bool TakeDamage(float damageAmount, Transform damageDealer);
}
