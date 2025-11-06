# API Reference - RPG System

## Character System

### CharacterStats

Main class for managing character statistics.

#### Properties

| Property | Type | Access | Description |
|----------|------|--------|-------------|
| CharacterName | string | Get/Set | Name of the character |
| Level | int | Get | Current level |
| Experience | int | Get | Current experience points |
| MaxHealth | float | Get | Maximum health points |
| CurrentHealth | float | Get | Current health points |
| MaxMana | float | Get | Maximum mana points |
| CurrentMana | float | Get | Current mana points |
| Strength | int | Get/Set | Strength stat |
| Intelligence | int | Get/Set | Intelligence stat |
| Dexterity | int | Get/Set | Dexterity stat |
| Vitality | int | Get/Set | Vitality stat |
| Armor | float | Get/Set | Armor value |
| MagicResist | float | Get/Set | Magic resistance value |
| AttackPower | float | Get | Calculated attack power |
| SpellPower | float | Get | Calculated spell power |
| IsDead | bool | Get | True if health is 0 or below |

#### Methods

```csharp
void Initialize()
```
Initializes stats to default values.

```csharp
void CalculateSecondaryStats()
```
Recalculates attack power, spell power, max health, and max mana.

```csharp
void TakeDamage(float damage)
```
Applies physical damage reduced by armor.

```csharp
void TakeMagicDamage(float damage)
```
Applies magic damage reduced by magic resistance.

```csharp
void Heal(float amount)
```
Restores health up to maximum.

```csharp
void RestoreMana(float amount)
```
Restores mana up to maximum.

```csharp
bool UseMana(float amount)
```
Attempts to consume mana. Returns true if successful.

```csharp
void AddExperience(int amount)
```
Adds experience and automatically handles level ups.

### PlayerController

Controls player movement and input.

#### Inspector Fields

| Field | Type | Description |
|-------|------|-------------|
| moveSpeed | float | Base movement speed |
| sprintSpeed | float | Movement speed while sprinting |
| rotationSpeed | float | Camera rotation sensitivity |
| gravity | float | Gravity force |
| jumpHeight | float | Jump height |

#### Properties

```csharp
CharacterStats Stats { get; }
```
Access to player's character stats.

#### Methods

```csharp
void TakeDamage(float damage)
```
Deals damage to the player and handles death.

### EnemyController

AI controller for enemy characters.

#### Inspector Fields

| Field | Type | Description |
|-------|------|-------------|
| detectionRange | float | Range at which enemy detects player |
| attackRange | float | Range at which enemy attacks |
| attackCooldown | float | Time between attacks |
| experienceReward | int | Experience given when defeated |

#### Properties

```csharp
CharacterStats Stats { get; }
```
Access to enemy's character stats.

#### Methods

```csharp
void TakeDamage(float damage)
```
Deals damage to the enemy and handles death.

## Combat System

### CombatSystem

Singleton that handles combat calculations.

#### Inspector Fields

| Field | Type | Description |
|-------|------|-------------|
| criticalChance | float | Base critical hit chance (0-1) |
| criticalMultiplier | float | Damage multiplier for crits |

#### Methods

```csharp
float CalculatePhysicalDamage(float attackPower, float armor)
```
Calculates physical damage with armor reduction and crit chance.

```csharp
float CalculateMagicalDamage(float spellPower, float magicResist)
```
Calculates magical damage with magic resistance and crit chance.

```csharp
void ApplyDamageOverTime(GameObject target, float damagePerSecond, float duration)
```
Applies DoT effect to target.

## Inventory System

### Item

Base class for all items.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| itemName | string | Name of the item |
| description | string | Item description |
| icon | Sprite | Item icon |
| itemType | ItemType | Type of item |
| maxStackSize | int | Maximum stack size |
| value | int | Gold value |

### EquipmentItem

Equipment that can be worn.

#### Additional Properties

| Property | Type | Description |
|----------|------|-------------|
| equipmentSlot | EquipmentSlot | Which slot it occupies |
| strengthBonus | int | Strength bonus |
| intelligenceBonus | int | Intelligence bonus |
| dexterityBonus | int | Dexterity bonus |
| vitalityBonus | int | Vitality bonus |
| armorBonus | float | Armor bonus |
| magicResistBonus | float | Magic resistance bonus |

### ConsumableItem

Items that can be consumed.

#### Additional Properties

| Property | Type | Description |
|----------|------|-------------|
| healthRestore | float | Health restored |
| manaRestore | float | Mana restored |
| duration | float | Effect duration |

### InventorySystem

Singleton managing player inventory.

#### Properties

```csharp
List<InventorySlot> Slots { get; }
```
Access to all inventory slots.

#### Methods

```csharp
bool AddItem(Item item, int quantity = 1)
```
Adds item to inventory. Returns false if inventory full.

```csharp
bool RemoveItem(Item item, int quantity = 1)
```
Removes item from inventory. Returns false if not found.

```csharp
bool UseItem(Item item)
```
Uses a consumable item. Returns true if successful.

```csharp
void EquipItem(EquipmentItem equipment)
```
Equips an equipment item.

```csharp
void UnequipItem(EquipmentSlot slot)
```
Unequips item from specified slot.

```csharp
int GetItemCount(Item item)
```
Returns total count of item in inventory.

## Quest System

### Quest

Represents a quest.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| questId | string | Unique identifier |
| questName | string | Display name |
| description | string | Quest description |
| status | QuestStatus | Current status |
| objectives | List<QuestObjective> | List of objectives |
| reward | QuestReward | Quest rewards |

#### Methods

```csharp
void StartQuest()
```
Marks quest as in progress.

```csharp
void CompleteQuest()
```
Marks quest as completed if all objectives done.

```csharp
void FailQuest()
```
Marks quest as failed.

```csharp
void UpdateObjective(string targetName, int amount)
```
Updates progress on objective.

### QuestManager

Singleton managing all quests.

#### Properties

```csharp
List<Quest> ActiveQuests { get; }
List<Quest> CompletedQuests { get; }
```

#### Methods

```csharp
void AcceptQuest(Quest quest)
```
Accepts and starts a quest.

```csharp
void CompleteQuest(Quest quest)
```
Completes quest and gives rewards.

```csharp
void AbandonQuest(Quest quest)
```
Abandons active quest.

```csharp
void UpdateQuestProgress(string targetName, int amount)
```
Updates all relevant quest objectives.

```csharp
Quest GetQuestById(string questId)
```
Retrieves quest by ID.

```csharp
bool IsQuestActive(string questId)
bool IsQuestCompleted(string questId)
```
Check quest status.

## UI System

### UIManager

Singleton managing game UI.

#### Methods

```csharp
void ToggleInventory()
```
Shows/hides inventory panel.

```csharp
void ToggleQuestLog()
```
Shows/hides quest log.

```csharp
void ShowDamageText(Vector3 position, float damage)
```
Displays damage number at position.

```csharp
void ShowNotification(string message)
```
Shows notification message.

## Game Management

### GameManager

Singleton managing game state.

#### Properties

```csharp
bool IsPaused { get; }
float GameTime { get; }
```

#### Methods

```csharp
void TogglePause()
```
Pauses/unpauses game.

```csharp
void LoadScene(string sceneName)
```
Loads specified scene.

```csharp
void RestartGame()
```
Restarts current scene.

```csharp
void QuitGame()
```
Quits application.

```csharp
void SaveGame()
void LoadGame()
```
Save/load hooks (to be implemented).

## Utilities

### RPGUtilities

Static utility methods.

```csharp
float CalculateDamageWithArmor(float baseDamage, float armor)
bool IsCriticalHit(float critChance)
int GetExperienceForLevel(int level)
float Distance2D(Vector3 a, Vector3 b)
Vector3 GetRandomPositionInRadius(Vector3 center, float radius)
float GetPercentage(float current, float max)
string FormatTime(float seconds)
bool IsInRange(Vector3 position, Vector3 target, float range)
```

## Events (Future Enhancement)

The system can be extended with events:

```csharp
// Example event patterns
public static event Action<float> OnPlayerHealthChanged;
public static event Action<int> OnPlayerLevelUp;
public static event Action<Quest> OnQuestCompleted;
public static event Action<Item> OnItemAcquired;
```

## Constants

### Formulas

**Armor Reduction:**
```
reduction = armor / (armor + 100)
```

**Experience for Level:**
```
exp = level * 100
```

**Secondary Stats:**
```
attackPower = (strength * 2) + (dexterity * 1.5)
spellPower = intelligence * 3
maxHealth = 100 + (vitality * 10)
maxMana = 50 + (intelligence * 5)
```
