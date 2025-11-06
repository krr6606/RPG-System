# RPG System Architecture

## System Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         Unity 6 RPG System                       │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                        Game Manager Layer                        │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │ GameManager  │  │  UIManager   │  │ InputHandler │         │
│  │  (Singleton) │  │  (Singleton) │  │              │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      Core Systems Layer                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │ CombatSystem │  │  Inventory   │  │QuestManager  │         │
│  │  (Singleton) │  │   System     │  │ (Singleton)  │         │
│  │              │  │  (Singleton) │  │              │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                     Character Layer                              │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │   Player     │  │    Enemy     │  │ CharacterStats│         │
│  │  Controller  │  │  Controller  │  │   (Shared)    │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                       Unity Components                           │
│  CharacterController │ NavMeshAgent │ Colliders │ UI Elements   │
└─────────────────────────────────────────────────────────────────┘
```

## Data Flow

### Player Action Flow
```
Player Input → PlayerController → CharacterStats → Combat/Inventory/Quest
                                                            ↓
                                                      Update Systems
                                                            ↓
                                                       UIManager
                                                            ↓
                                                     Display Update
```

### Enemy AI Flow
```
Update Loop → Find Target → Calculate Distance → Choose Action
                                                       ↓
                                    ┌─────────────────┴─────────────┐
                                    ▼                               ▼
                              Chase Target                    Attack Target
                                    │                               │
                                    └───────────┬───────────────────┘
                                                ▼
                                        Update NavMesh
```

### Combat Flow
```
Attack Triggered → Calculate Damage → Apply Modifiers → Check Critical
                                                              ↓
                                                    Apply to CharacterStats
                                                              ↓
                                                      Check if Dead
                                                              ↓
                                            ┌─────────────────┴────────────┐
                                            ▼                              ▼
                                       Handle Death                  Update UI
                                            │
                                            ▼
                                     Award Experience/Loot
```

### Inventory Flow
```
Add Item → Check Stack → Find Slot → Update Inventory
                                          ↓
                              ┌───────────┴──────────┐
                              ▼                      ▼
                         Use Item               Equip Item
                              │                      │
                              ▼                      ▼
                      Apply Effects           Update Stats
                              │                      │
                              └──────────┬───────────┘
                                         ▼
                                     Update UI
```

### Quest Flow
```
Accept Quest → Add to Active → Track Progress → Check Objectives
                                                       ↓
                                              All Complete?
                                                       ↓
                                    ┌──────────────────┴────────────────┐
                                    ▼                                   ▼
                              Complete Quest                        Continue
                                    │
                                    ▼
                            Award Rewards → Update Stats → Update UI
```

## Class Hierarchy

```
MonoBehaviour
├── GameManager
├── UIManager
├── CombatSystem
├── InventorySystem
├── QuestManager
├── PlayerController
│   └── Uses: CharacterStats
├── EnemyController
│   └── Uses: CharacterStats
└── DamageOverTime

Serializable Classes (Data)
├── CharacterStats
├── Item
│   ├── EquipmentItem
│   └── ConsumableItem
├── InventorySlot
├── Quest
├── QuestObjective
└── QuestReward

Static Classes
└── RPGUtilities
```

## Component Dependencies

### PlayerController Dependencies
```
Required:
- CharacterController (Unity Component)
- CharacterStats (Custom)

Interacts With:
- CombatSystem (for damage calculation)
- InventorySystem (for item usage)
- UIManager (for display updates)
```

### EnemyController Dependencies
```
Required:
- NavMeshAgent (Unity Component)
- CharacterStats (Custom)
- Baked NavMesh (Scene Setup)

Interacts With:
- CombatSystem (for damage calculation)
- QuestManager (for quest updates)
- PlayerController (for targeting)
```

### InventorySystem Dependencies
```
Interacts With:
- Item classes (data)
- CharacterStats (for equipment bonuses)
- UIManager (for display)
- QuestManager (for quest items)
```

### QuestManager Dependencies
```
Interacts With:
- Quest classes (data)
- CharacterStats (for rewards)
- InventorySystem (for item rewards)
- UIManager (for quest display)
```

## Singleton Pattern Implementation

All manager classes follow this pattern:

```csharp
public class ManagerName : MonoBehaviour
{
    public static ManagerName Instance { get; private set; }
    
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
}
```

## Key Namespaces

```
RPGSystem
├── Character
│   ├── CharacterStats
│   ├── PlayerController
│   └── EnemyController
├── Combat
│   ├── CombatSystem
│   └── DamageOverTime
├── Inventory
│   ├── Item
│   ├── EquipmentItem
│   ├── ConsumableItem
│   ├── InventorySlot
│   └── InventorySystem
├── Quest
│   ├── Quest
│   ├── QuestObjective
│   ├── QuestReward
│   └── QuestManager
├── UI
│   └── UIManager
└── Utilities
    └── RPGUtilities
```

## Communication Patterns

### Event-Based (Future Enhancement)
```csharp
// Example pattern for future implementation
public static event Action<float> OnHealthChanged;
public static event Action<int> OnLevelUp;
public static event Action<Quest> OnQuestComplete;
```

### Direct Reference
```csharp
// Current pattern
CombatSystem.Instance.CalculatePhysicalDamage(atk, armor);
InventorySystem.Instance.AddItem(item);
QuestManager.Instance.UpdateQuestProgress(target, amount);
```

### Component Communication
```csharp
// Through GetComponent
PlayerController player = target.GetComponent<PlayerController>();
EnemyController enemy = target.GetComponent<EnemyController>();
```

## Execution Order

1. **Awake Phase**
   - GameManager initializes
   - All Singleton managers establish Instance
   - DontDestroyOnLoad applied to managers

2. **Start Phase**
   - Characters initialize stats
   - UI finds and references elements
   - Systems complete initialization

3. **Update Phase**
   - PlayerController handles input/movement
   - EnemyController runs AI logic
   - UIManager updates display
   - GameManager tracks time

4. **Event Response**
   - Damage dealt → Stats updated → UI refreshes
   - Enemy dies → Quest progress → Rewards given
   - Level up → Stats recalculated → UI updates

## Performance Considerations

### Efficient Patterns
- ✓ Singleton prevents duplicate managers
- ✓ DontDestroyOnLoad prevents recreation
- ✓ GetComponent cached in Awake when possible
- ✓ NavMesh used for pathfinding (native optimization)
- ✓ Stats calculated on change, not every frame

### Optimization Opportunities
- Object pooling for damage numbers
- Coroutines for periodic checks
- Event system to reduce polling
- Spatial hashing for enemy detection
- LOD system for distant enemies

## Scene Setup Requirements

### Minimum Scene Setup
1. GameSystems GameObject + GameManager
2. Player GameObject + PlayerController + CharacterController
3. Canvas + UIManager + UI Elements
4. Baked NavMesh (for enemies)

### Recommended Scene Setup
1. GameSystems (with GameManager)
2. Player (fully configured)
3. UI Canvas (complete UI)
4. Environment (ground, obstacles)
5. NavMesh (baked)
6. Enemies (spawned or placed)
7. Lighting
8. Camera (as child of player or Cinemachine)

## Extension Points

### Adding New Features
1. **New Stats**: Extend CharacterStats
2. **New Items**: Inherit from Item
3. **New Quests**: Use Quest system
4. **New Enemies**: Extend EnemyController
5. **New UI**: Add to UIManager
6. **New Systems**: Follow Singleton pattern

### Integration Points
- Save/Load hooks in GameManager
- Event callbacks throughout systems
- Virtual methods for inheritance
- Interface opportunities for polymorphism

## Summary

This architecture provides:
- ✓ Modular, maintainable code
- ✓ Clear separation of concerns
- ✓ Singleton pattern for global access
- ✓ Component-based design
- ✓ Extensible framework
- ✓ Unity-friendly implementation
- ✓ Performance-conscious design
- ✓ Well-documented codebase
