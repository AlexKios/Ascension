# Ascension

**Genre:** 2D Action-Platformer  
**Engine:** Unity (2D)  
**Platform:** Mobile (Android / iOS)  
**Course:** Mobile Games, Plovdiv University

---

## Overview

Ascension is a 2D action-platformer built in Unity for the Mobile Games course at Plovdiv University. You play as a lone adventurer journeying across varied landscapes, facing hordes of normal enemies and powerful bosses. Defeat bosses to collect special items that grant you new traversal and combat abilities. Race against the clock to reach—and conquer—the final challenge!

---

## Features

- **World Exploration**  
  Traverse diverse biomes and unlock new areas as you gain abilities.  
- **Enemy Types**  
  - **Normal Enemies:** Basic AI that chases and attacks the player on sight.  
  - **Bosses:** Unique, tougher foes that drop Ability Items upon defeat.  
- **Progression Items & Abilities**  
  - **Dash** – Quick horizontal burst  
  - **Air Dash** – Mid-air burst for aerial maneuvers  
  - **Double Jump** – Gain an extra leap while airborne  
  - **Super Jump** – High-powered leap to reach lofty platforms  
  - **Wall Jump** – Push off walls to scale vertical shafts  
- **Timer & Leaderboard**  
  A built-in timer tracks how long it takes to beat the game. Optimize your run and aim for the fastest completion time!  
- **Mobile-Friendly Controls**  
  On-screen joystick and buttons for movement, jumping, dashing, and pausing  

---

## Installation & Setup

1. **Clone the repository**  
   ```bash
   git clone https://github.com/YourUsername/Ascension.git
2. **Open in Unity**

    Launch Unity Hub (version 2021.3 LTS or later recommended)

    Click Add, navigate to the cloned Ascension folder, and open the project

3. **Configure Build Settings**  
   - File → Build Settings → Select **Android** or **iOS**  
   - Switch Platform  
4. **Run**  
   - Connect your device or use an emulator  
   - Click **Build and Run**  

---

## Controls

| Action          | Mobile Input             |
| --------------- | ------------------------ |
| Move Left/Right | Virtual Joystick (left)  |
| Jump            | “Jump” Button (right)    |
| Dash            | “Dash” Button (right)    |
| Pause           | “Pause” Icon (top-right) |

> **Note:** Abilities (Air Dash, Double Jump, etc.) unlock as you collect boss-dropped items.

---

## Gameplay Loop

1. **Explore** each level, avoiding or fighting normal enemies.  
2. **Defeat Bosses** to obtain Ability Items.  
3. **Unlock New Abilities** and use them to access previously unreachable areas.  
4. **Race the Timer**—every second counts toward your final completion time.  
5. **Final Showdown:** Defeat the last boss to end the game and record your time.

---

## AI Behavior

- **Normal Enemies** patrol a set path until the player enters detection range, then chase and attempt melee attacks.  
- **Boss Enemies** feature scripted patterns, phases, and special attacks. Upon defeat, they drop an Ability Item.

---

## Art & Audio

- **Sprites:** Hand-drawn 2D pixel art assets  
- **Animations:** Frame-based Unity Animator controllers  
- **Sound Effects & Music:** Royalty-free tracks and SFX, as credited in the `/Assets/Audio` folder

---

## Contributing

This project was developed for an academic course and is not accepting outside contributions at this time. Feel free to fork and experiment on your own!

---
