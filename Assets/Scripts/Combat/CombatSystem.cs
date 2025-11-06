using UnityEngine;

namespace RPGSystem.Combat
{
    /// <summary>
    /// Handles combat mechanics including damage calculation
    /// </summary>
    public class CombatSystem : MonoBehaviour
    {
        [Header("Combat Settings")]
        [SerializeField] private float criticalChance = 0.1f;
        [SerializeField] private float criticalMultiplier = 2f;
        
        public static CombatSystem Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Calculate physical damage
        /// </summary>
        public float CalculatePhysicalDamage(float attackPower, float armor)
        {
            float damage = attackPower;
            
            // Apply armor reduction
            float damageReduction = armor / (armor + 100f);
            damage *= (1f - damageReduction);
            
            // Check for critical hit
            if (Random.value <= criticalChance)
            {
                damage *= criticalMultiplier;
                Debug.Log("Critical Hit!");
            }
            
            return Mathf.Max(1f, damage);
        }
        
        /// <summary>
        /// Calculate magical damage
        /// </summary>
        public float CalculateMagicalDamage(float spellPower, float magicResist)
        {
            float damage = spellPower;
            
            // Apply magic resistance
            float damageReduction = magicResist / (magicResist + 100f);
            damage *= (1f - damageReduction);
            
            // Check for critical hit
            if (Random.value <= criticalChance)
            {
                damage *= criticalMultiplier;
                Debug.Log("Critical Hit!");
            }
            
            return Mathf.Max(1f, damage);
        }
        
        /// <summary>
        /// Apply damage over time effect
        /// </summary>
        public void ApplyDamageOverTime(GameObject target, float damagePerSecond, float duration)
        {
            DamageOverTime dot = target.GetComponent<DamageOverTime>();
            if (dot == null)
            {
                dot = target.AddComponent<DamageOverTime>();
            }
            
            dot.ApplyEffect(damagePerSecond, duration);
        }
    }
    
    /// <summary>
    /// Component for damage over time effects
    /// </summary>
    public class DamageOverTime : MonoBehaviour
    {
        private float damagePerSecond;
        private float remainingDuration;
        private bool isActive;
        
        public void ApplyEffect(float dps, float duration)
        {
            damagePerSecond = dps;
            remainingDuration = duration;
            isActive = true;
        }
        
        private void Update()
        {
            if (!isActive) return;
            
            remainingDuration -= Time.deltaTime;
            
            // Apply damage
            var player = GetComponent<Character.PlayerController>();
            if (player != null)
            {
                player.Stats.TakeDamage(damagePerSecond * Time.deltaTime);
            }
            
            var enemy = GetComponent<Character.EnemyController>();
            if (enemy != null)
            {
                enemy.Stats.TakeDamage(damagePerSecond * Time.deltaTime);
            }
            
            if (remainingDuration <= 0)
            {
                isActive = false;
                Destroy(this);
            }
        }
    }
}
