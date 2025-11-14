# Documentation of Procedural Programmation

> To access file code: Open the "Components>ProceduralGeneration" folder.

- [Introduction](#introduction)
- [DungeonParts](#DungeonParts)
- [RandomMapGenerator](#RandomMapGenerator)

## How it works
The ProceduralGridGenerator contains the main algortithm.
<div align="center">
  <img src="READ_ME_Images/HowItWorks/PGGenerator.png" alt="Logo" width="500" height="500">
</div>

You have to create a ScriptableObject in order to make your program.
You can choose existing Procedural Program via Assets/Components/Procedural Generation

You can change the Script by Drag and Drop here depending on your procedural generation method. 
<div align="center">
  <img src="READ_ME_Images/HowItWorks/PGG_Menu.png" alt="Logo" width="500" height="500">
</div>

## Dungeons Parts
### • Simple Room Placement
  This algorithms generates random square into a square with defined length and width,
  and make the paths between rooms.

> TO DO: Link the room between them.

### • Binary Space Partition
  The core idea of BSP is to break down a complex space (for example, a 2D or 3D environment) into smaller, more manageable regions. This division is done recursively by choosing a plane (in 3D) or a line (in 2D) that splits the space into two parts:

Code Made by Yona Rutkowski

## • Random Map Generator
### • Cellular Automata
  Generation of random pixels in the map, and according to the 8 pixels all around, change the pixel. 
<div align="center">
  <img src="READ_ME_Images/cellular_exemple.png" alt="Logo" width="500" height="500">
<div\>

• Noise
Definition:
Procedural noise is a mathematical technique used to generate natural-looking randomness in computer graphics, simulations, and procedural content generation. Unlike pure random values, noise functions produce smooth, continuous variations, making them ideal for creating textures, terrains, clouds, and other organic patterns.

Principle:
The idea behind procedural noise is to generate pseudo-random values based on spatial coordinates (e.g., x, y, z). These values are computed using deterministic mathematical functions, meaning the same input will always produce the same output.
The result is a continuous field of values that change gradually over space — giving a natural, non-repetitive look.


Binary Space Partition :
Basically:
The core idea of BSP is to break down a complex space (for example, a 2D or 3D environment) into smaller, more manageable regions. This division is done recursively by choosing a plane (in 3D) or a line (in 2D) that splits the space into two parts:
