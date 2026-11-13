# The First Pact — Unity 6

**The First Pact** is a personal turn-based fantasy RPG prototype developed in **Unity 6 (C#)**, inspired by *Dragon Quest Monsters: Joker*.

The project features strategic 3v3 monster battles, exploration, enemy AI, and interactive dialogue systems.

Beyond the gameplay itself, I developed custom editor tools, data-driven interfaces, dynamic camera systems, and stylized rendering effects to explore different aspects of **Game Programming**.

## Features

### Gameplay Systems
- **3v3 Turn-Based Combat** — Turn management, action resolution, abilities, targeting, animations, and combat feedback.
- **Player Controller** — Character movement, exploration, and interactions with the game world.
- **Dynamic Camera** — Context-sensitive camera transitions and framing during gameplay and combat.
- **Monster AI** — A state machine with five states: Wander, Suspicious, Alerted, Chase, and Engaging.
- **Monster Spawning** — Configurable spawn zones with weighted probabilities for different monster types.
- **Dialogue System** — Interactive NPC conversations with character portraits and dialogue sequences.

### Custom Editor Tools
- **MonsterData** — Configurable monster statistics, visual profiles, abilities, visual prefabs, animations, and audio.
- **AbilityData** — Customizable abilities with effects, resource costs, targeting, accuracy, VFX, SFX, and dynamic descriptions.
- **DialogueData** — Dialogue creation and editing with configurable speakers, portraits, text, and display positions.

### UI & Rendering
- **Dynamic UI** — Combat interfaces automatically populated and updated using monster and ability data.
- **Adaptive Controls** — Keyboard and gamepad support with UI prompts that automatically adapt to the active input device.
- **Menus & Settings** — Main menu, audio settings, display options, and language selection.
- **Custom Shaders** — Stylized rendering developed with Shader Graph, including outlines, posterization, color adjustments, and screen-space texture effects.

## Technical Information

| | |
|---|---|
| **Engine** | Unity 6 (URP) |
| **Language** | C# |
| **Platform** | Windows PC |
| **Development** | Solo project |
| **Status** | Playable prototype |

## Play the Game

The Windows demo is available on itch.io:

**[Download The First Pact on itch.io](https://samuel-selami.itch.io/)**

For a more detailed breakdown of the technical systems, editor tools, and shaders, visit my portfolio:

**[Game Programming Portfolio](https://sites.google.com/view/samuel-selami-portfolio)**

## Assets & Credits

Game programming, systems development, UI integration, and custom rendering effects by Samuel Selami.

Third-party assets used in this prototype:

- **Monster Models & Animations:** RPG Monster BUNDLE Polyart by Dungeon Mason.
- **Environment Assets:** POLYGON Meadow Forest – Nature Biomes and POLYGON Fantasy Kingdom by Synty.
- **Character Portraits:** Generative AI.

Third-party assets remain the property of their respective creators.

## About

This project was developed as part of my transition from **Game Art to Game Programming**.
