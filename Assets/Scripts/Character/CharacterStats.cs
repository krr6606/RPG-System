using UnityEngine;

namespace RPGSystem.Character
{
    /// <summary>
    /// Base class for character statistics
    /// </summary>
    [System.Serializable]
    public class CharacterStats
    {
        [Header("Basic Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private int level = 1;
        [SerializeField] private int experience = 0;
        
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;
        
        [Header("Mana")]
        [SerializeField] private float maxMana = 50f;
        [SerializeField] private float currentMana;
        
        [Header("Primary Stats")]
        [SerializeField] private int strength = 10;
        [SerializeField] private int intelligence = 10;
        [SerializeField] private int dexterity = 10;
        [SerializeField] private int vitality = 10;
        
        [Header("Secondary Stats")]
        [SerializeField] private float armor = 0f;
        [SerializeField] private float magicResist = 0f;
        [SerializeField] private float attackPower;
        [SerializeField] private float spellPower;
        
        public string CharacterName { get => characterName; set => characterName = value; }
        public int Level { get => level; private set => level = value; }
        public int Experience { get => experience; private set => experience = value; }
        
        public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
        public float CurrentHealth { get => currentHealth; private set => currentHealth = value; }
        
        public float MaxMana { get => maxMana; private set => maxMana = value; }
        public float CurrentMana { get => currentMana; private set => currentMana = value; }
        
        public int Strength { get => strength; set => strength = value; }
        public int Intelligence { get => intelligence; set => intelligence = value; }
        public int Dexterity { get => dexterity; set => dexterity = value; }
        public int Vitality { get => vitality; set => vitality = value; }
        
        public float Armor { get => armor; set => armor = value; }
        public float MagicResist { get => magicResist; set => magicResist = value; }
        public float AttackPower { get => attackPower; private set => attackPower = value; }
        public float SpellPower { get => spellPower; private set => spellPower = value; }
        
        public bool IsDead => currentHealth <= 0;
        
        public CharacterStats()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            currentHealth = maxHealth;
            currentMana = maxMana;
            CalculateSecondaryStats();
        }
        
        public void CalculateSecondaryStats()
        {
            // Calculate attack power based on strength and dexterity
            attackPower = (strength * 2f) + (dexterity * 1.5f);
            
            // Calculate spell power based on intelligence
            spellPower = intelligence * 3f;
            
            // Max health increases with vitality
            maxHealth = 100f + (vitality * 10f);
            
            // Max mana increases with intelligence
            maxMana = 50f + (intelligence * 5f);
        }
        
        public void TakeDamage(float damage)
        {
            float actualDamage = Mathf.Max(0, damage - armor);
            currentHealth = Mathf.Max(0, currentHealth - actualDamage);
            
            if (IsDead)
            {
                OnDeath();
            }
        }
        
        public void TakeMagicDamage(float damage)
        {
            float actualDamage = Mathf.Max(0, damage - magicResist);
            currentHealth = Mathf.Max(0, currentHealth - actualDamage);
            
            if (IsDead)
            {
                OnDeath();
            }
        }
        
        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        }
        
        public void RestoreMana(float amount)
        {
            currentMana = Mathf.Min(maxMana, currentMana + amount);
        }
        
        public bool UseMana(float amount)
        {
            if (currentMana >= amount)
            {
                currentMana -= amount;
                return true;
            }
            return false;
        }
        
        public void AddExperience(int amount)
        {
            experience += amount;
            CheckLevelUp();
        }
        
        private void CheckLevelUp()
        {
            int expNeeded = GetExperienceForNextLevel();
            
            while (experience >= expNeeded)
            {
                LevelUp();
                expNeeded = GetExperienceForNextLevel();
            }
        }
        
        private void LevelUp()
        {
            level++;
            
            // Increase stats on level up
            strength += 2;
            intelligence += 2;
            dexterity += 2;
            vitality += 2;
            
            CalculateSecondaryStats();
            
            // Restore health and mana on level up
            currentHealth = maxHealth;
            currentMana = maxMana;
            
            Debug.Log($"{characterName} leveled up to {level}!");
        }
        
        private int GetExperienceForNextLevel()
        {
            return level * 100;
        }
        
        protected virtual void OnDeath()
        {
            Debug.Log($"{characterName} has died!");
        }
    }
}
