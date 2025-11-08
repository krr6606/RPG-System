using System;
using UnityEngine;

public class Entity_Health : MonoBehaviour,IDamagable
{
    Entity_VFX entityVFX;
    private Entity entity;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead = false;

    [Header("On Damage Knockback Settings")]
    [SerializeField] protected Vector2 knockbackForce;
    [SerializeField] protected Vector2 heavyDamageKnockbackForce;
    [SerializeField] protected float knockbackDuration;
    [SerializeField] protected float HeavyDamageKnockbackDuration;
    [Header("On Heavy Damage Settings")]
    [SerializeField] private float heavyDamageThreshold = .3f;// % of max health
    protected virtual void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        currentHealth = maxHealth;
    }
    public virtual void TakeDamage(float damageAmount, Transform damageDealer)
    {
        if (isDead) return;
        Vector2 knockbackDir = CalculateKnockbackDirection(damageAmount, damageDealer);
        float knockbackDuration = CalculateDuration(damageAmount);
        entity?.Knockback(knockbackDuration, knockbackDir);
        entityVFX?.PlayOnDamageVFX();
        ReduceHP(damageAmount);

    }
    protected void ReduceHP(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }
    private Vector2 CalculateKnockbackDirection(float damageAmount, Transform damageDealer)
    {
        int direction = damageDealer.position.x < transform.position.x ? 1 : -1;
        Vector2 knockbackDir = IsHeavyDamage(damageAmount) ? heavyDamageKnockbackForce : knockbackForce;
        knockbackDir.x *= direction;
        return knockbackDir;
    }
    private float CalculateDuration(float damageAmount) => IsHeavyDamage(damageAmount) ? HeavyDamageKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damageAmount)
    {
        return damageAmount > maxHealth * heavyDamageThreshold;
    }
}
