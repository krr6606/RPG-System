# RPG System - Getting Started Guide

## Quick Start

### Setting Up Your First Scene

1. **Create a New Scene**
   - File → New Scene
   - Choose "3D" template
   - Save as "MainGame" in Assets/Scenes/

2. **Add Core Systems**
   - Create an empty GameObject named "GameSystems"
   - Add the GameManager component
   - The GameManager will automatically initialize:
     - CombatSystem
     - InventorySystem
     - QuestManager

3. **Set Up the Player**
   - Create a Capsule GameObject (3D Object → Capsule)
   - Rename it to "Player"
   - Tag it as "Player" (Create tag if needed)
   - Add Component → Character Controller
   - Add Component → PlayerController script
   - Configure stats in the Inspector

4. **Create the Environment**
   - Create a Plane for the ground (3D Object → Plane)
   - Scale it up (e.g., 10, 1, 10)
   - Tag it as "Ground"
   - Add a Material for visibility

5. **Add UI**
   - Create UI → Canvas
   - Add UI → Slider for Health Bar
   - Add UI → Slider for Mana Bar
   - Add TextMeshPro elements for Level and Experience
   - Create an empty GameObject under Canvas, add UIManager script
   - Assign UI elements in the Inspector

6. **Add Enemies (Optional)**
   - Create a Capsule GameObject
   - Tag it as "Enemy"
   - Add Component → Nav Mesh Agent
   - Add Component → EnemyController script
   - Bake Navigation (Window → AI → Navigation)

## Character Stats Configuration

### Recommended Starting Stats

**Warrior Build:**
- Strength: 15
- Intelligence: 5
- Dexterity: 10
- Vitality: 15

**Mage Build:**
- Strength: 5
- Intelligence: 15
- Dexterity: 10
- Vitality: 10

**Rogue Build:**
- Strength: 10
- Intelligence: 8
- Dexterity: 15
- Vitality: 12

## Creating Your First Quest

```csharp
// In a MonoBehaviour script
void CreateExampleQuest()
{
    Quest newQuest = new Quest("quest_001", "Kill Goblins", "Defeat 5 goblins terrorizing the village");
    
    QuestObjective objective = new QuestObjective
    {
        description = "Defeat goblins",
        type = QuestObjectiveType.Kill,
        targetName = "Goblin",
        requiredAmount = 5,
        currentAmount = 0
    };
    
    newQuest.objectives.Add(objective);
    
    newQuest.reward = new QuestReward
    {
        experience = 100,
        gold = 50
    };
    
    QuestManager.Instance.AcceptQuest(newQuest);
}
```

## Creating Items

### Example: Health Potion

```csharp
ConsumableItem healthPotion = new ConsumableItem
{
    itemName = "Health Potion",
    description = "Restores 50 HP",
    itemType = Item.ItemType.Consumable,
    maxStackSize = 10,
    value = 25,
    healthRestore = 50f,
    manaRestore = 0f
};

InventorySystem.Instance.AddItem(healthPotion);
```

### Example: Sword Equipment

```csharp
EquipmentItem ironSword = new EquipmentItem
{
    itemName = "Iron Sword",
    description = "A basic iron sword",
    itemType = Item.ItemType.Equipment,
    maxStackSize = 1,
    value = 100,
    equipmentSlot = EquipmentItem.EquipmentSlot.Weapon,
    strengthBonus = 5,
    intelligenceBonus = 0,
    dexterityBonus = 2,
    vitalityBonus = 0
};

InventorySystem.Instance.AddItem(ironSword);
```

## Combat Implementation

### Dealing Damage to Enemies

```csharp
// In your player attack script
void AttackEnemy(GameObject enemy)
{
    EnemyController enemyController = enemy.GetComponent<EnemyController>();
    if (enemyController != null)
    {
        float damage = CombatSystem.Instance.CalculatePhysicalDamage(
            playerStats.AttackPower,
            enemyController.Stats.Armor
        );
        
        enemyController.TakeDamage(damage);
    }
}
```

## Tips and Best Practices

1. **NavMesh for Enemies**
   - Always bake NavMesh before using EnemyController
   - Window → AI → Navigation → Bake

2. **Camera Setup**
   - Attach camera as child of Player
   - Position it behind and above the player
   - Or use Cinemachine for advanced camera control

3. **Testing**
   - Use Debug.Log to verify system initialization
   - Check Console for system messages
   - Test one system at a time

4. **Performance**
   - Limit active enemies in scene
   - Use object pooling for projectiles
   - Optimize NavMesh settings

## Common Issues

### Player Not Moving
- Check if CharacterController is attached
- Verify Input settings in Project Settings
- Ensure ground layer is set correctly

### Enemy Not Chasing
- Make sure NavMesh is baked
- Check if Player has "Player" tag
- Verify detection range is large enough

### UI Not Updating
- Ensure UIManager has reference to UI elements
- Check if Player is found by UIManager
- Verify Canvas is set to Screen Space - Overlay

## Next Steps

1. Create custom enemy types with unique behaviors
2. Design a quest chain with multiple objectives
3. Implement a skill/ability system
4. Add visual and audio feedback
5. Create different weapon types
6. Implement a save/load system
7. Add NPC dialogue system
8. Create boss encounters

## Resources

- Unity Documentation: https://docs.unity3d.com/
- C# Reference: https://docs.microsoft.com/en-us/dotnet/csharp/
- Unity Forums: https://forum.unity.com/

For more information, see the main README.md file.
