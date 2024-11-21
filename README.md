# Solus

![image](https://github.com/user-attachments/assets/aab0d25e-8e7d-4bd6-a8d6-4db5b87a0d63)

A challenging top-down 2D survival game built with Unity and C# where players must survive against increasingly difficult waves of enemies while collecting powerups and exploring an infinitely generating world.

![image](https://github.com/user-attachments/assets/a2413758-dfdd-4a38-babc-ccb12fc6094d)

## Features

- **Infinite World Generation**: Procedurally generated map that creates new terrain as the player explores
- **Progressive Difficulty**: Enemy level difficulty increases every minute up to 5 times
  - Enemy attack damage increase (+20 per level)
  - Enemy health increase (+20 per level)
  - Enemy speed increase (+0.05 per level)
- **Power-up System**: Collect powerups from defeated enemies:
  - Health boost (+20 per heart up to a max of 10 hearts)
  - Attack damage increase (+10 up to a max of 50 damage)
  - Movement speed enhancement (.1 up to a max of 1 speed)
- **Scoring System**:
  - 10 points per second survived
  - 50 points per enemy defeated
- **Assets**:
  - Sound designs
    - Enemy hit, Enemy hurt, Player death, Player hurt made using https://sfbgames.itch.io/chiptone
    - Dash/Dodge 1st note taken from UI Goku, Enemy and Player attack taken from online swoosh sound effect, Power Up sound taken from https://uppbeat.io/sfx/tag/power-up 
  - Original player sprite designs 
  - Custom made UI elements
  - Custom made powerup sprites
  - Modified enemy animations and colors (credits: AceWay & Darkeyed https://finalgatestudios.itch.io/undead-sprite-pack)
  - Tilemap sprite designs from (https://opengameart.org/content/lpc-terrain-repack) 

## Installation

1. Clone the repository
```bash
git clone https://github.com/AlexLenh7/CS-583-Solus.git
```

2. Requirements:
- Unity 2022.3.44f1
- Visual Studio 2019 or later 

3. Opening the Project:
- Launch Unity Hub
- Click "Add" and browse to the cloned repository
- Select the project folder
- Open with Unity 2022.3.44f1

## How to Play

1. **Main Menu**: Start a new game, read the controls, or quit
  
2. **Gameplay**:
   - WASD: 4 directional movement 
   - Space: Dash/Dodge
   - Left Click: Attack
   - Collect powerups from defeated enemies
   - (Secret mechanic) Dash attack: While dashing left click to perform a dash attack
    
3. **Death Screen**: View final score, restart or go back to the menu

## Technical Challenges

### Animation System
One of the major challenges was implementing a robust animation system that could:
- Smoothly transition between different states (idle, walking, attacking)
- Correctly orient character sprites based on movement direction
- Synchronize animations with gameplay actions

### Combat System
Implementing precise hitbox detection and damage calculation required:
- Creating efficient collision detection systems
- Managing attack cooldowns
- Balancing damage values and enemy health

### Procedural Generation
The infinite map generation system presented several challenges:
- Ensuring seamless chunk transitions
- Optimizing performance by destroying far away chunks
- Loads new chunks based on player position

### Power-up System
Balancing the power-up system required:
- Implementing caps on stat increases
- Creating a fair distribution system
- Ensuring powerups remain meaningful throughout gameplay

## Future Improvements

- Dynamic Lighting
- Additional enemy types
- Additional animations for 8 directional attack/movement
- More power-up varieties
- Environmental hazards
- Boss battles
- Save system for high scores
- Additional weapon types
