using UnityEngine;
using UnityEngine.AI;

namespace RPGSystem.Character
{
    /// <summary>
    /// Enemy AI controller
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private CharacterStats stats;
        
        [Header("AI Behavior")]
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1.5f;
        
        [Header("Loot")]
        [SerializeField] private int experienceReward = 50;
        
        private NavMeshAgent agent;
        private Transform target;
        private float lastAttackTime;
        private bool isDead;
        
        public CharacterStats Stats => stats;
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            
            if (stats == null)
            {
                stats = new CharacterStats();
                stats.CharacterName = "Enemy";
            }
            
            stats.Initialize();
        }
        
        private void Update()
        {
            if (isDead) return;
            
            FindTarget();
            
            if (target != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                
                if (distanceToTarget <= attackRange)
                {
                    AttackTarget();
                }
                else if (distanceToTarget <= detectionRange)
                {
                    ChaseTarget();
                }
            }
        }
        
        private void FindTarget()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    float distance = Vector3.Distance(transform.position, player.transform.position);
                    if (distance <= detectionRange)
                    {
                        target = player.transform;
                    }
                }
            }
        }
        
        private void ChaseTarget()
        {
            agent.SetDestination(target.position);
        }
        
        private void AttackTarget()
        {
            agent.SetDestination(transform.position);
            
            transform.LookAt(target);
            
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PerformAttack();
                lastAttackTime = Time.time;
            }
        }
        
        private void PerformAttack()
        {
            PlayerController player = target.GetComponent<PlayerController>();
            if (player != null)
            {
                float damage = stats.AttackPower;
                player.TakeDamage(damage);
                Debug.Log($"{stats.CharacterName} attacks for {damage} damage!");
            }
        }
        
        public void TakeDamage(float damage)
        {
            stats.TakeDamage(damage);
            
            if (stats.IsDead && !isDead)
            {
                Die();
            }
        }
        
        private void Die()
        {
            isDead = true;
            agent.enabled = false;
            
            // Award experience to player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                PlayerController player = playerObj.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Stats.AddExperience(experienceReward);
                }
            }
            
            Debug.Log($"{stats.CharacterName} has been defeated!");
            
            // Destroy after a delay
            Destroy(gameObject, 3f);
        }
    }
}
