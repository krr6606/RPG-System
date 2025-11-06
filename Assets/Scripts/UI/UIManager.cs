using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPGSystem.UI
{
    /// <summary>
    /// Main UI manager for the RPG system
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Player UI")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider manaBar;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI experienceText;
        
        [Header("Inventory UI")]
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private bool inventoryOpen = false;
        
        [Header("Quest UI")]
        [SerializeField] private GameObject questPanel;
        [SerializeField] private TextMeshProUGUI questLogText;
        
        private Character.PlayerController player;
        
        public static UIManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            player = FindObjectOfType<Character.PlayerController>();
            
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(false);
            }
            
            if (questPanel != null)
            {
                questPanel.SetActive(false);
            }
        }
        
        private void Update()
        {
            if (player != null)
            {
                UpdatePlayerUI();
            }
            
            HandleUIInput();
        }
        
        private void UpdatePlayerUI()
        {
            if (healthBar != null)
            {
                healthBar.maxValue = player.Stats.MaxHealth;
                healthBar.value = player.Stats.CurrentHealth;
            }
            
            if (manaBar != null)
            {
                manaBar.maxValue = player.Stats.MaxMana;
                manaBar.value = player.Stats.CurrentMana;
            }
            
            if (levelText != null)
            {
                levelText.text = $"Level: {player.Stats.Level}";
            }
            
            if (experienceText != null)
            {
                experienceText.text = $"EXP: {player.Stats.Experience}";
            }
        }
        
        private void HandleUIInput()
        {
            // Toggle inventory
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
            
            // Toggle quest log
            if (Input.GetKeyDown(KeyCode.L))
            {
                ToggleQuestLog();
            }
        }
        
        public void ToggleInventory()
        {
            if (inventoryPanel != null)
            {
                inventoryOpen = !inventoryOpen;
                inventoryPanel.SetActive(inventoryOpen);
                
                if (inventoryOpen)
                {
                    UpdateInventoryDisplay();
                }
            }
        }
        
        public void ToggleQuestLog()
        {
            if (questPanel != null)
            {
                bool isActive = questPanel.activeSelf;
                questPanel.SetActive(!isActive);
                
                if (!isActive)
                {
                    UpdateQuestLogDisplay();
                }
            }
        }
        
        private void UpdateInventoryDisplay()
        {
            // This would be expanded with actual inventory UI elements
            Debug.Log("Inventory opened");
        }
        
        private void UpdateQuestLogDisplay()
        {
            if (questLogText != null && Quest.QuestManager.Instance != null)
            {
                string questLog = "Active Quests:\n\n";
                
                foreach (var quest in Quest.QuestManager.Instance.ActiveQuests)
                {
                    questLog += $"{quest.questName}\n";
                    questLog += $"{quest.description}\n";
                    
                    foreach (var objective in quest.objectives)
                    {
                        string status = objective.IsCompleted ? "[X]" : "[ ]";
                        questLog += $"{status} {objective.description} ({objective.currentAmount}/{objective.requiredAmount})\n";
                    }
                    
                    questLog += "\n";
                }
                
                questLogText.text = questLog;
            }
        }
        
        public void ShowDamageText(Vector3 position, float damage)
        {
            // This would create floating damage text at the position
            Debug.Log($"Damage: {damage} at {position}");
        }
        
        public void ShowNotification(string message)
        {
            Debug.Log($"Notification: {message}");
        }
    }
}
