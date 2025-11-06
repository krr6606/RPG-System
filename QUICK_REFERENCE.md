# Quick Reference - RPG System

## Essential Commands

### Creating a Player
```csharp
GameObject player = new GameObject("Player");
player.tag = "Player";
player.AddComponent<CharacterController>();
PlayerController pc = player.AddComponent<PlayerController>();
```

### Creating an Enemy
```csharp
GameObject enemy = new GameObject("Enemy");
enemy.tag = "Enemy";
enemy.AddComponent<NavMeshAgent>();
EnemyController ec = enemy.AddComponent<EnemyController>();
```

### Adding Items to Inventory
```csharp
// Health Potion
ConsumableItem potion = new ConsumableItem {
    itemName = "Health Potion",
    healthRestore = 50f
};
InventorySystem.Instance.AddItem(potion);

// Weapon
EquipmentItem sword = new EquipmentItem {
    itemName = "Iron Sword",
    equipmentSlot = EquipmentItem.EquipmentSlot.Weapon,
    strengthBonus = 5
};
InventorySystem.Instance.AddItem(sword);
```

### Creating a Quest
```csharp
Quest quest = new Quest("q1", "Kill Enemies", "Defeat 10 enemies");
quest.objectives.Add(new QuestObjective {
    description = "Defeat enemies",
    type = QuestObjectiveType.Kill,
    targetName = "Enemy",
    requiredAmount = 10
});
quest.reward = new QuestReward { experience = 100, gold = 50 };
QuestManager.Instance.AcceptQuest(quest);
```

### Dealing Damage
```csharp
// Physical damage
float damage = CombatSystem.Instance.CalculatePhysicalDamage(
    attackPower, targetArmor
);
enemyController.TakeDamage(damage);

// Magic damage
float magicDmg = CombatSystem.Instance.CalculateMagicalDamage(
    spellPower, targetMagicResist
);
enemyController.Stats.TakeMagicDamage(magicDmg);
```

### Updating Quest Progress
```csharp
// When enemy dies
QuestManager.Instance.UpdateQuestProgress("Enemy", 1);

// When item collected
QuestManager.Instance.UpdateQuestProgress("GoldCoin", 5);
```

## Common Patterns

### Singleton Access
```csharp
GameManager.Instance.TogglePause();
CombatSystem.Instance.CalculatePhysicalDamage(atk, armor);
InventorySystem.Instance.AddItem(item);
QuestManager.Instance.AcceptQuest(quest);
UIManager.Instance.ShowNotification("Message");
```

### Stat Modification
```csharp
CharacterStats stats = player.Stats;
stats.Strength += 5;
stats.CalculateSecondaryStats();  // Recalculate derived stats
```

### Equipment Management
```csharp
// Equip
InventorySystem.Instance.EquipItem(equipment);

// Unequip
InventorySystem.Instance.UnequipItem(EquipmentItem.EquipmentSlot.Weapon);
```

## Inspector Setup Checklist

### Player GameObject
- [ ] Tag: "Player"
- [ ] Layer: Player (8)
- [ ] Component: CharacterController
- [ ] Component: PlayerController
- [ ] Configure: Move Speed, Sprint Speed, Jump Height

### Enemy GameObject
- [ ] Tag: "Enemy"
- [ ] Layer: Enemy (9)
- [ ] Component: NavMeshAgent
- [ ] Component: EnemyController
- [ ] Configure: Detection Range, Attack Range, Experience Reward

### Canvas (UI)
- [ ] Component: Canvas (Screen Space - Overlay)
- [ ] Child: Health Slider
- [ ] Child: Mana Slider
- [ ] Child: Level Text (TextMeshPro)
- [ ] Child: Experience Text (TextMeshPro)
- [ ] Component: UIManager (on child GameObject)

### Game Systems
- [ ] Empty GameObject: "GameSystems"
- [ ] Component: GameManager

## Common Values

### Character Stats (Beginner)
```
Level: 1
Strength: 10
Intelligence: 10
Dexterity: 10
Vitality: 10
```

### Movement
```
Move Speed: 5.0
Sprint Speed: 8.0
Rotation Speed: 10.0
Jump Height: 1.5
Gravity: -9.81
```

### Combat
```
Critical Chance: 0.1 (10%)
Critical Multiplier: 2.0 (200%)
```

### Enemy AI
```
Detection Range: 10.0
Attack Range: 2.0
Attack Cooldown: 1.5
```

## Keyboard Shortcuts

| Key | Function |
|-----|----------|
| W/S | Forward/Backward |
| A/D | Left/Right |
| Shift | Sprint |
| Space | Jump |
| Mouse 0 | Attack |
| Q | Ability |
| I | Inventory |
| L | Quest Log |
| ESC | Pause |

## Layer Setup (Edit → Project Settings → Tags and Layers)

```
Layer 0: Default
Layer 1: TransparentFX
Layer 2: Ignore Raycast
Layer 5: UI
Layer 8: Player
Layer 9: Enemy
Layer 10: NPC
Layer 11: Ground
```

## Tag Setup (Edit → Project Settings → Tags and Layers)

```
- Player
- Enemy
- NPC
- Item
- QuestGiver
```

## Required Packages

```json
{
  "com.unity.ugui": "1.0.0",
  "com.unity.textmeshpro": "3.0.6",
  "com.unity.ai.navigation": "1.1.5"
}
```

## Scene Setup Order

1. Create Plane (Ground)
2. Create Capsule (Player)
   - Add CharacterController
   - Add PlayerController
   - Tag as "Player"
3. Create Canvas (UI)
   - Add UI elements
   - Add UIManager
4. Create Empty GameObject (GameSystems)
   - Add GameManager
5. Bake NavMesh (Window → AI → Navigation)
6. Create Capsule (Enemy)
   - Add NavMeshAgent
   - Add EnemyController
   - Tag as "Enemy"
7. Add Camera as child of Player

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Player not moving | Check CharacterController component exists |
| Enemy not chasing | Bake NavMesh, check Player tag |
| UI not updating | Assign UI elements in UIManager Inspector |
| Stats not updating | Call CalculateSecondaryStats() after changes |
| Inventory full | Increase inventorySize in InventorySystem |

## Performance Tips

- Limit NavMesh Agent count to < 20 per scene
- Use object pooling for projectiles
- Disable NavMesh updates when far from player
- Update UI only when values change
- Use coroutines for periodic checks

## Extension Points

### Add New Stat
1. Add field to CharacterStats
2. Update CalculateSecondaryStats()
3. Update level-up rewards

### Add New Item Type
1. Create class inheriting from Item
2. Add ItemType enum value
3. Implement in UseItem() or EquipItem()

### Add New Quest Objective
1. Add QuestObjectiveType enum value
2. Update Quest.UpdateObjective()
3. Create trigger logic in game

### Add New Enemy Behavior
1. Extend EnemyController
2. Override Update() or Add new state
3. Implement custom AI logic

## File Paths

```
Scripts/Character/CharacterStats.cs
Scripts/Character/PlayerController.cs
Scripts/Character/EnemyController.cs
Scripts/Combat/CombatSystem.cs
Scripts/Inventory/InventorySystem.cs
Scripts/Quest/QuestSystem.cs
Scripts/UI/UIManager.cs
Scripts/GameManager.cs
Scripts/Utilities/RPGUtilities.cs
```

## Support

For detailed information:
- See README.md for overview
- See GETTING_STARTED.md for setup guide
- See API_REFERENCE.md for complete API docs
- See IMPLEMENTATION_SUMMARY.md for what was created
