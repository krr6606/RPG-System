using System;
using UnityEngine;
using UnityEngine.UI;
public class Entity_Health : MonoBehaviour,IDamagable
{
    Slider healthBar;
    Entity_VFX entityVFX;
    private Entity entity;
    Entity_Stat entityStat;

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
        entityStat = GetComponent<Entity_Stat>();
        healthBar = GetComponentInChildren<Slider>();
        if(entityStat == null)
        {
            Debug.LogError("Entity_Stat component missing on " + gameObject.name);
        }

    }
    void Start()
    {
        currentHealth = entityStat.GetMaxHP();
        UpdateHealthBar();
    }
    public virtual bool TakeDamage(float damageAmount, Transform damageDealer)
    {
        if (isDead) return false;
        if (AttackEvaded())
        {
            Debug.Log("Attack Evaded!");
            return false;
        }
        Entity_Stat dealerStat = damageDealer.GetComponent<Entity_Stat>();
        float armorReduction = dealerStat != null ? dealerStat.GetArmorReduction() : 0f;

        float mitigation = entityStat.GetArmorMitigation(armorReduction);
        damageAmount *= (1 - mitigation); 

        Vector2 knockbackDir = CalculateKnockbackDirection(damageAmount, damageDealer);
        float knockbackDuration = CalculateDuration(damageAmount);

        entity?.Knockback(knockbackDuration, knockbackDir);
        entityVFX?.PlayOnDamageVFX();

        ReduceHP(damageAmount);
        Debug.Log("Damage Taken: " + damageAmount);
        return true;
    }
    protected void ReduceHP(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private bool AttackEvaded()
    {
        return UnityEngine.Random.Range(0,100) < entityStat.GetEvasion();
    }
    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }
    private void UpdateHealthBar()
    {
        if (healthBar == null)
        {
            Debug.LogWarning("Health bar not assigned in " + gameObject.name);
            return;
        }

            healthBar.value = currentHealth / entityStat.GetMaxHP();

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
        return damageAmount > entityStat.GetMaxHP() * heavyDamageThreshold;
    }
}
