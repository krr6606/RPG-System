using System;
using UnityEngine;
using UnityEngine.UI;
public class Entity_Health : MonoBehaviour,IDamagable
{
    Slider healthBar;
    Entity_VFX entityVFX;
    private Entity entity;
    Entity_Stat entityStat;
    Entity_Health entityHealth;

    [SerializeField] protected float currentHealth;
    [Header("Health Regeneration Settings")]
    [SerializeField] protected bool canRegenerateHealth = false;
    [SerializeField] protected float healthRegenInterval = 1;

    protected bool canTakeDamage = true;
    public bool isDead { get; private set; }
    public float lastDamageTaken { get; protected set; }

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
        entityHealth = GetComponent<Entity_Health>();

    }
    void Start()
    {
        SetupHealth();
    }

    private void SetupHealth()
    {
        if (entityStat == null)
            return;
            currentHealth = entityStat.GetMaxHP();
        UpdateHealthBar();
        InvokeRepeating(nameof(RegegenerateHP), 0, healthRegenInterval);
    }

    public virtual bool TakeDamage(float damageAmount,float elementalDamage,ElementType elementType, Transform damageDealer)
    {
        if (isDead || !canTakeDamage) return false;
        if (AttackEvaded())
        {
            Debug.Log("Attack Evaded!");
            return false;
        }
        Entity_Stat dealerStat = damageDealer.GetComponent<Entity_Stat>();
        float armorReduction = dealerStat != null ? dealerStat.GetArmorReduction() : 0f;

        float mitigation = entityStat != null ? entityStat.GetArmorMitigation(armorReduction) : 0;
        float physicalDamageTaken = damageAmount * (1 - mitigation);

        float elementalResist = entityStat != null ? entityStat.GetElementalResistance(elementType) : 0;
        float elementalDamageTaken = elementalDamage * (1 - elementalResist / 100f);

        TakeKonkback(physicalDamageTaken, damageDealer);

        ReduceHP(physicalDamageTaken + elementalDamageTaken);

        lastDamageTaken = physicalDamageTaken + elementalDamageTaken;
        return true;
    }
    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;
    private void RegegenerateHP()
    {
        if(!canRegenerateHealth || isDead || currentHealth==entityStat.GetMaxHP()) return;
        float regenAmount = entityStat.statResourceGroup.healthRegen.GetValue();
        IncreaseHP(regenAmount);
    }
    public void IncreaseHP(float healAmount)
    {
        if (isDead) return;
        float newHealth = currentHealth + healAmount;
        currentHealth = Mathf.Min(newHealth, entityStat.GetMaxHP());
        UpdateHealthBar();
    }
    public void ReduceHP(float damage)
    {
        entityVFX?.PlayOnDamageVFX();
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public float GetHealthPercentage()
    {
        return currentHealth / entityStat.GetMaxHP();
    }

    public void SetHealthToPercent(float percent)
    {
        currentHealth = entityStat.GetMaxHP() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }

    private bool AttackEvaded()
    {
        if(entityStat == null) 
            return false;
        else
            return UnityEngine.Random.Range(0, 100) < entityStat.GetEvasion();
    }
    protected virtual void Die()
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
    private void TakeKonkback(float damageAmount, Transform damageDealer)
    {
        Vector2 knockbackDir = CalculateKnockbackDirection(damageAmount, damageDealer);
        float knockbackDuration = CalculateDuration(damageAmount);

        entity?.Knockback(knockbackDuration, knockbackDir);
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
        if(entityStat == null) return false;
        return damageAmount > entityStat.GetMaxHP() * heavyDamageThreshold;
    }
}
