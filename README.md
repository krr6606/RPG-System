# RPG-System
A comprehensive RPG game system built with Unity 6

## Overview
This project is a complete Role-Playing Game (RPG) system designed for Unity 6, featuring core RPG mechanics including character management, combat, inventory, and quest systems.

## Features

### Character System
- **CharacterStats**: Comprehensive stat management system
  - Health and Mana systems
  - Primary stats: Strength, Intelligence, Dexterity, Vitality
  - Secondary stats: Armor, Magic Resistance, Attack Power, Spell Power
  - Level progression and experience system
  - Automatic stat calculation based on level

- **PlayerController**: Fully-featured player movement and control
  - WASD/Arrow key movement
  - Mouse-based camera rotation
  - Sprint functionality (Left Shift)
  - Jump mechanics
  - Attack and ability systems

- **EnemyController**: AI-driven enemy behavior
  - NavMesh-based pathfinding
  - Detection and chase mechanics
  - Attack behavior with cooldown
  - Experience rewards on defeat

### Combat System
- Physical and magical damage calculation
- Armor and magic resistance mechanics
- Critical hit system
- Damage over time (DoT) effects
- Damage reduction formulas

### Inventory System
- Flexible inventory with configurable slots
- Item types: Consumables, Equipment, Quest Items, Materials
- Stackable items
- Equipment system with stat bonuses
- Equipment slots: Head, Chest, Legs, Feet, Hands, Weapon, Shield, Accessory
- Item usage and consumption

### Quest System
- Quest management framework
- Quest objective types: Kill, Collect, Talk, Reach, Escort
- Quest status tracking: Not Started, In Progress, Completed, Failed
- Reward system (Experience, Gold, Items)
- Multiple objectives per quest
- Progress tracking and completion detection

### UI System
- Health and Mana bars
- Level and experience display
- Inventory panel (Toggle with 'I')
- Quest log (Toggle with 'L')
- Notification system

### Game Management
- Centralized game manager
- Pause/Resume functionality (ESC key)
- Scene management
- Save/Load system hooks
- Game time tracking

## Project Structure
```
RPG-System/
├── Assets/
│   ├── Scripts/
│   │   ├── Character/
│   │   │   ├── CharacterStats.cs
│   │   │   ├── PlayerController.cs
│   │   │   └── EnemyController.cs
│   │   ├── Combat/
│   │   │   └── CombatSystem.cs
│   │   ├── Inventory/
│   │   │   └── InventorySystem.cs
│   │   ├── Quest/
│   │   │   └── QuestSystem.cs
│   │   ├── UI/
│   │   │   └── UIManager.cs
│   │   └── GameManager.cs
│   ├── Prefabs/
│   ├── Scenes/
│   ├── Materials/
│   └── Audio/
├── ProjectSettings/
│   ├── ProjectSettings.asset
│   ├── AudioManager.asset
│   ├── DynamicsManager.asset
│   ├── InputManager.asset
│   └── TagManager.asset
├── Packages/
│   └── manifest.json
└── README.md
```

## Getting Started

### Prerequisites
- Unity 6 or later
- Basic understanding of Unity and C#

### Setup
1. Clone this repository
2. Open the project in Unity 6
3. Open any scene or create a new one
4. Add the GameManager prefab to your scene
5. Create a player GameObject and attach the PlayerController script
6. Configure character stats in the Inspector

### Controls
- **WASD/Arrow Keys**: Move character
- **Mouse**: Rotate camera
- **Left Shift**: Sprint
- **Space**: Jump
- **Left Mouse Button**: Attack
- **Q**: Use ability
- **I**: Toggle inventory
- **L**: Toggle quest log
- **ESC**: Pause/Resume game

## System Requirements

### Input System
The project uses Unity's legacy Input Manager with the following axes configured:
- Horizontal (A/D, Left/Right arrows)
- Vertical (W/S, Up/Down arrows)
- Mouse X/Y for camera control
- Fire1 (Left Ctrl/Mouse 0)
- Jump (Space)

### Tags
Required tags:
- Player
- Enemy
- NPC
- Item
- QuestGiver

### Layers
Configured layers:
- Player (Layer 8)
- Enemy (Layer 9)
- NPC (Layer 10)
- Ground (Layer 11)

## Core Systems Documentation

### Character Stats System
The CharacterStats class provides a complete RPG stat system:
- Automatic secondary stat calculation
- Level-up rewards
- Experience-to-level formulas
- Health/Mana management

### Combat System
Physical Damage Formula:
```
Damage Reduction = Armor / (Armor + 100)
Final Damage = Attack Power * (1 - Damage Reduction)
```

Magic Damage Formula:
```
Damage Reduction = Magic Resist / (Magic Resist + 100)
Final Damage = Spell Power * (1 - Damage Reduction)
```

Critical Hit:
- 10% base chance
- 2x damage multiplier

### Experience System
Experience required for next level:
```
XP Required = Current Level * 100
```

Level up bonuses:
- +2 to all primary stats
- Full health and mana restore

## Extending the System

### Adding New Item Types
1. Create a new class inheriting from `Item`
2. Add custom properties
3. Implement usage logic in `InventorySystem.UseItem()`

### Adding New Quest Objectives
1. Add new type to `QuestObjectiveType` enum
2. Implement logic in `Quest.UpdateObjective()`
3. Create quest completion triggers

### Adding New Enemy Types
1. Create new GameObject with EnemyController
2. Configure NavMeshAgent
3. Set custom stats and behavior parameters

## Dependencies
- Unity UI (com.unity.ugui)
- TextMeshPro (com.unity.textmeshpro)
- Unity AI Navigation (for NavMesh)
- Timeline
- Visual Scripting

## Version
- Unity Version: 6.0 or later
- Project Version: 0.1.0

## License
This project is intended for educational and development purposes.

## Contributing
This is a study project for RPG game elements. Feel free to fork and extend the systems for your own use.

## Future Enhancements
- Save/Load system implementation
- Dialogue system
- Crafting system
- Skill tree
- Multiplayer support
- Advanced AI behaviors
- Sound effects and music integration
- Visual effects for abilities
- Mini-map system
- Trading system with NPCs
