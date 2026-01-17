# Football (Soccer) Simulator

![Unity](https://img.shields.io/badge/Unity-2022.3.62f2-black?style=for-the-badge&logo=unity)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Android%20%7C%20macOS-blue?style=for-the-badge)
![Game](https://img.shields.io/badge/Game-Football%20Simulator-green?style=for-the-badge)
![Input](https://img.shields.io/badge/Input-System%20(New)-orange?style=for-the-badge)
![GitHub stars](https://img.shields.io/github/stars/HTANV/Soccer-Unity?style=for-the-badge)
![GitHub last commit](https://img.shields.io/github/last-commit/HTANV/Soccer-Unity?style=for-the-badge)

---

## Overview

**Football (Soccer) Simulator** is a complete, fully playable football game built in Unity.  
It features a full **match engine**, **team selector**, and a powerful **team creator/editor**, allowing players and developers to create, customize, and simulate football matches with AI or manual control.

The project is designed to be **ready-to-play**, **easy to extend**, and **cross-platform**, making it suitable for learning, prototyping, or building a full commercial football game.

---

## Key Features

### Match Engine
- Real-time playable football matches
- Home / Away side selection
- Player-controlled or full AI-vs-AI matches
- Realistic movement, passing, and positioning logic

### Team System
- 7 pre-made teams included
- Team selector available from main menu
- Home / Away team assignment
- AI-controlled teams supported

### Team Editor
- Create new teams or edit existing ones
- Player editor with skill-based attributes
- 10 pre-made kit schemas
- Teams saved in `Resources/Database/` and auto-loaded in menu

### Player Positions & Skill System
Each player position uses a different skill-weight formula:

- GK – Goalkeeper  
- CB – Center Back  
- LB / RB – Left & Right Back  
- DMF – Defensive Midfielder  
- CM – Central Midfielder  
- AMF – Attacking Midfielder  
- LMF / RMF – Left & Right Midfielder  
- LW / RW – Left & Right Winger  
- ST – Striker  

Player **overall rating** is calculated dynamically based on position-specific skill priorities.

---

## Input Support

- Gamepad
- Keyboard & Mouse
- Touch Controls (Mobile UI)
- Built using **Unity New Input System**

---

## Playable Platforms

- Windows (x64)
- Android
- macOS

Playable executable builds can be generated directly from the project.

---

## Technical Details

| Category | Details |
|-------|--------|
| Engine | Unity |
| Unity Version | 2022.3.62f2 |
| Language | C# |
| Input | Unity New Input System |
| Architecture | Modular & Config-driven |
| Platforms | PC, Mobile, macOS |
| Assets | Code, UI, Graphics & Audio included |

---

## Requirements

### Minimum
- **Unity Editor**: 2022.3.62f2
- **OS**: Windows 10 / macOS / Linux
- **RAM**: 8 GB recommended
- **Storage**: ~2 GB free space

### Optional
- Gamepad controller
- Android device for touch testing

---

## Getting Started

1. Clone or download the repository
2. Open project using **Unity Hub**
3. Ensure Unity **2022.3.62f2** is installed
4. Open the Main Menu scene
5. Press **Play** and start a match

---

## Customization

- Create new teams via Team Editor
- Modify player skills & positions
- Edit configs and scenes to build your own football world
- Extend AI, animations, or UI freely

---

## Disclaimer

This project is intended for:
- Educational use
- Prototyping
- Portfolio showcase
- Game development learning

Assets and systems should be reviewed before use in commercial products.

---

## Support

If you need help understanding the systems or extending the project, feel free to explore the codebase and experiment with the tools provided.

---

**Build your own football universe with Football (Soccer) Simulator**