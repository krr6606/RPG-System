using UnityEngine;
using System.Collections.Generic;

namespace RPGSystem.Quest
{
    /// <summary>
    /// Quest objective types
    /// </summary>
    public enum QuestObjectiveType
    {
        Kill,
        Collect,
        Talk,
        Reach,
        Escort
    }
    
    /// <summary>
    /// Quest status
    /// </summary>
    public enum QuestStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Failed
    }
    
    /// <summary>
    /// Individual quest objective
    /// </summary>
    [System.Serializable]
    public class QuestObjective
    {
        public string description;
        public QuestObjectiveType type;
        public string targetName;
        public int requiredAmount;
        public int currentAmount;
        
        public bool IsCompleted => currentAmount >= requiredAmount;
        
        public void UpdateProgress(int amount)
        {
            currentAmount = Mathf.Min(currentAmount + amount, requiredAmount);
        }
    }
    
    /// <summary>
    /// Quest reward
    /// </summary>
    [System.Serializable]
    public class QuestReward
    {
        public int experience;
        public int gold;
        public List<Inventory.Item> items;
    }
    
    /// <summary>
    /// Quest data
    /// </summary>
    [System.Serializable]
    public class Quest
    {
        public string questId;
        public string questName;
        public string description;
        public QuestStatus status;
        public List<QuestObjective> objectives;
        public QuestReward reward;
        
        public bool IsCompleted => status == QuestStatus.Completed;
        public bool AllObjectivesCompleted
        {
            get
            {
                foreach (var objective in objectives)
                {
                    if (!objective.IsCompleted)
                        return false;
                }
                return true;
            }
        }
        
        public Quest(string id, string name, string desc)
        {
            questId = id;
            questName = name;
            description = desc;
            status = QuestStatus.NotStarted;
            objectives = new List<QuestObjective>();
            reward = new QuestReward();
        }
        
        public void StartQuest()
        {
            status = QuestStatus.InProgress;
            Debug.Log($"Quest started: {questName}");
        }
        
        public void CompleteQuest()
        {
            if (AllObjectivesCompleted)
            {
                status = QuestStatus.Completed;
                Debug.Log($"Quest completed: {questName}");
            }
        }
        
        public void FailQuest()
        {
            status = QuestStatus.Failed;
            Debug.Log($"Quest failed: {questName}");
        }
        
        public void UpdateObjective(string targetName, int amount)
        {
            foreach (var objective in objectives)
            {
                if (objective.targetName == targetName)
                {
                    objective.UpdateProgress(amount);
                    
                    if (objective.IsCompleted)
                    {
                        Debug.Log($"Objective completed: {objective.description}");
                    }
                    
                    if (AllObjectivesCompleted)
                    {
                        CompleteQuest();
                    }
                    break;
                }
            }
        }
    }
    
    /// <summary>
    /// Quest manager system
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        [Header("Quest Settings")]
        [SerializeField] private List<Quest> availableQuests;
        
        private List<Quest> activeQuests;
        private List<Quest> completedQuests;
        
        public static QuestManager Instance { get; private set; }
        
        public List<Quest> ActiveQuests => activeQuests;
        public List<Quest> CompletedQuests => completedQuests;
        
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
            
            activeQuests = new List<Quest>();
            completedQuests = new List<Quest>();
            
            if (availableQuests == null)
            {
                availableQuests = new List<Quest>();
            }
        }
        
        public void AcceptQuest(Quest quest)
        {
            if (quest.status == QuestStatus.NotStarted)
            {
                quest.StartQuest();
                activeQuests.Add(quest);
                Debug.Log($"Accepted quest: {quest.questName}");
            }
        }
        
        public void CompleteQuest(Quest quest)
        {
            if (quest.AllObjectivesCompleted)
            {
                quest.CompleteQuest();
                activeQuests.Remove(quest);
                completedQuests.Add(quest);
                
                GiveRewards(quest.reward);
                Debug.Log($"Quest {quest.questName} completed! Rewards given.");
            }
        }
        
        public void AbandonQuest(Quest quest)
        {
            activeQuests.Remove(quest);
            quest.status = QuestStatus.NotStarted;
            
            // Reset objectives
            foreach (var objective in quest.objectives)
            {
                objective.currentAmount = 0;
            }
            
            Debug.Log($"Abandoned quest: {quest.questName}");
        }
        
        public void UpdateQuestProgress(string targetName, int amount)
        {
            foreach (var quest in activeQuests)
            {
                quest.UpdateObjective(targetName, amount);
            }
        }
        
        private void GiveRewards(QuestReward reward)
        {
            var player = FindObjectOfType<Character.PlayerController>();
            if (player != null)
            {
                // Give experience
                if (reward.experience > 0)
                {
                    player.Stats.AddExperience(reward.experience);
                    Debug.Log($"Gained {reward.experience} experience");
                }
                
                // Give items
                if (reward.items != null && reward.items.Count > 0)
                {
                    var inventory = Inventory.InventorySystem.Instance;
                    if (inventory != null)
                    {
                        foreach (var item in reward.items)
                        {
                            inventory.AddItem(item);
                        }
                    }
                }
                
                // Give gold (if you have a currency system)
                if (reward.gold > 0)
                {
                    Debug.Log($"Gained {reward.gold} gold");
                }
            }
        }
        
        public Quest GetQuestById(string questId)
        {
            foreach (var quest in availableQuests)
            {
                if (quest.questId == questId)
                    return quest;
            }
            
            foreach (var quest in activeQuests)
            {
                if (quest.questId == questId)
                    return quest;
            }
            
            return null;
        }
        
        public bool IsQuestActive(string questId)
        {
            foreach (var quest in activeQuests)
            {
                if (quest.questId == questId)
                    return true;
            }
            return false;
        }
        
        public bool IsQuestCompleted(string questId)
        {
            foreach (var quest in completedQuests)
            {
                if (quest.questId == questId)
                    return true;
            }
            return false;
        }
    }
}
