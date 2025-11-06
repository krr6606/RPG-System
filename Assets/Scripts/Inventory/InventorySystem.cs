using UnityEngine;
using System.Collections.Generic;

namespace RPGSystem.Inventory
{
    /// <summary>
    /// Base class for all items in the game
    /// </summary>
    [System.Serializable]
    public class Item
    {
        public string itemName;
        public string description;
        public Sprite icon;
        public ItemType itemType;
        public int maxStackSize = 1;
        public int value;
        
        public enum ItemType
        {
            Consumable,
            Equipment,
            QuestItem,
            Material
        }
    }
    
    /// <summary>
    /// Equipment item that can be worn
    /// </summary>
    [System.Serializable]
    public class EquipmentItem : Item
    {
        public EquipmentSlot equipmentSlot;
        public int strengthBonus;
        public int intelligenceBonus;
        public int dexterityBonus;
        public int vitalityBonus;
        public float armorBonus;
        public float magicResistBonus;
        
        public enum EquipmentSlot
        {
            Head,
            Chest,
            Legs,
            Feet,
            Hands,
            Weapon,
            Shield,
            Accessory
        }
    }
    
    /// <summary>
    /// Consumable item that can be used
    /// </summary>
    [System.Serializable]
    public class ConsumableItem : Item
    {
        public float healthRestore;
        public float manaRestore;
        public float duration;
    }
    
    /// <summary>
    /// Inventory slot
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public Item item;
        public int quantity;
        
        public InventorySlot(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
        
        public bool IsEmpty => item == null || quantity <= 0;
        
        public bool CanAddItem(Item newItem)
        {
            if (IsEmpty) return true;
            if (item.itemName == newItem.itemName && quantity < item.maxStackSize)
            {
                return true;
            }
            return false;
        }
        
        public void AddItem(Item newItem, int amount)
        {
            if (IsEmpty)
            {
                item = newItem;
                quantity = amount;
            }
            else if (item.itemName == newItem.itemName)
            {
                quantity += amount;
            }
        }
        
        public void RemoveItem(int amount)
        {
            quantity -= amount;
            if (quantity <= 0)
            {
                item = null;
                quantity = 0;
            }
        }
    }
    
    /// <summary>
    /// Player inventory system
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [SerializeField] private int inventorySize = 20;
        
        private List<InventorySlot> slots;
        private Dictionary<EquipmentItem.EquipmentSlot, EquipmentItem> equippedItems;
        
        public static InventorySystem Instance { get; private set; }
        
        public List<InventorySlot> Slots => slots;
        
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
            
            InitializeInventory();
        }
        
        private void InitializeInventory()
        {
            slots = new List<InventorySlot>();
            for (int i = 0; i < inventorySize; i++)
            {
                slots.Add(new InventorySlot(null, 0));
            }
            
            equippedItems = new Dictionary<EquipmentItem.EquipmentSlot, EquipmentItem>();
        }
        
        public bool AddItem(Item item, int quantity = 1)
        {
            // Try to stack with existing items
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.item.itemName == item.itemName)
                {
                    if (slot.quantity + quantity <= item.maxStackSize)
                    {
                        slot.AddItem(item, quantity);
                        Debug.Log($"Added {quantity} {item.itemName} to inventory");
                        return true;
                    }
                }
            }
            
            // Find empty slot
            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                {
                    slot.AddItem(item, quantity);
                    Debug.Log($"Added {quantity} {item.itemName} to inventory");
                    return true;
                }
            }
            
            Debug.Log("Inventory is full!");
            return false;
        }
        
        public bool RemoveItem(Item item, int quantity = 1)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.item.itemName == item.itemName)
                {
                    if (slot.quantity >= quantity)
                    {
                        slot.RemoveItem(quantity);
                        Debug.Log($"Removed {quantity} {item.itemName} from inventory");
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        public bool UseItem(Item item)
        {
            if (item is ConsumableItem consumable)
            {
                var player = FindObjectOfType<Character.PlayerController>();
                if (player != null)
                {
                    player.Stats.Heal(consumable.healthRestore);
                    player.Stats.RestoreMana(consumable.manaRestore);
                    
                    RemoveItem(item, 1);
                    Debug.Log($"Used {item.itemName}");
                    return true;
                }
            }
            
            return false;
        }
        
        public void EquipItem(EquipmentItem equipment)
        {
            // Unequip current item in slot if any
            if (equippedItems.ContainsKey(equipment.equipmentSlot))
            {
                UnequipItem(equipment.equipmentSlot);
            }
            
            equippedItems[equipment.equipmentSlot] = equipment;
            
            // Apply stat bonuses
            var player = FindObjectOfType<Character.PlayerController>();
            if (player != null)
            {
                player.Stats.Strength += equipment.strengthBonus;
                player.Stats.Intelligence += equipment.intelligenceBonus;
                player.Stats.Dexterity += equipment.dexterityBonus;
                player.Stats.Vitality += equipment.vitalityBonus;
                player.Stats.Armor += equipment.armorBonus;
                player.Stats.MagicResist += equipment.magicResistBonus;
                player.Stats.CalculateSecondaryStats();
            }
            
            Debug.Log($"Equipped {equipment.itemName}");
        }
        
        public void UnequipItem(EquipmentItem.EquipmentSlot slot)
        {
            if (equippedItems.ContainsKey(slot))
            {
                EquipmentItem equipment = equippedItems[slot];
                
                // Remove stat bonuses
                var player = FindObjectOfType<Character.PlayerController>();
                if (player != null)
                {
                    player.Stats.Strength -= equipment.strengthBonus;
                    player.Stats.Intelligence -= equipment.intelligenceBonus;
                    player.Stats.Dexterity -= equipment.dexterityBonus;
                    player.Stats.Vitality -= equipment.vitalityBonus;
                    player.Stats.Armor -= equipment.armorBonus;
                    player.Stats.MagicResist -= equipment.magicResistBonus;
                    player.Stats.CalculateSecondaryStats();
                }
                
                equippedItems.Remove(slot);
                Debug.Log($"Unequipped {equipment.itemName}");
            }
        }
        
        public int GetItemCount(Item item)
        {
            int count = 0;
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.item.itemName == item.itemName)
                {
                    count += slot.quantity;
                }
            }
            return count;
        }
    }
}
