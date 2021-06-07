# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**


**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replay any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

**Please note that our game was highly ambitious. To accommodate for this, we had to share many different duties across the different roles. This document will show who did what part of the corresponding roles.**

## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**

**Jackson:** I added the main menu UI and the display health/damage text to the swarmlings.

## Movement/Physics

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

**Jackson:** I contributed by adding the collision functions and rigidbody/boxcollider logic for the player, workers, swarmlings, fireballs, leafblades, minibosses, boss, and resource depots.

We used a mix of the Unity physics system and our own scripted physics to modify it when it did not serve our purposes (ignoring collisions and rotations). The game is top down and 2d, so we had to ensure that gravity was disabled.

## Animation and Visuals

**List your assets including their sources and licenses.**

**Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists.**

**Jackson:** I contributed by providing the programmer art for the Swarmlings, Player (remove later if someone finds something better?), Workers, Boss/Dead-boss, Resource Depots, Fireball projectiles, Leafblade projectiles, and Miniboss. Leafblade image that I created was also used for the temporary ice spike art (remove if it gets replaced). 

We knew we wanted something magical, mechanical, and perhaps organic for the bad guys (and even player) so the swarmlings and boss were designed with the idea of mutated cyborgs. Bosses and swarmlings follow a similar style of green flesh and orange eyes so that the player easily recognizes the connection between the two different enemies.

**Navya:** I contributed to this role by building a tile set map for our game (basically worl-building). I got an asset from the unity store which had the feeling of an "rpg midtown". I felt like that would be the best for our game as it was quite an open-ended theme when we had not decided a story line for our game. I made the map in a way which would allow for the "SAP2D" to calculate colliders easily and kept on making changes on the map as the game progressed because of the colliders not wokring properly.

## Input

**Describe the default input configuration.**

**Add an entry for each platform or input style your project supports.**

**Jackson:** I contributed to the input role by offering my insights into how we will need our input logic to match the capabilities of the player. I created the camera controller that focuses the mouse position.

Keyboard and mouse: WASD movement, mouse+r for hovering over cards for information, mouse for optional camera controller panning, left-click enable optional camera controller, right click draw card, [1,2,3] for using the corresponding card slot.

**Navya:** My main role was input. I implementedd this role by implemting a WASD input for the player and a function which gets the mouse positions. Expect that, this role didn't demand much work in our game.
## Game Logic

**Document what game states and game data you managed and what design patterns you used to complete your task.**

**Jackson:** This was my assigned main role

Created the design and first draft for:
- Player Controller
- Swarmling Controller
- Worker Controller
- Inventory class
- ICard interface and card class mechanics
- Boss Controller
- Game Controller
- Controllers for various projectiles and their game objects
- Worker Commands
- Mini-boss game objects
- Resource Depot Controller

Implemented most of the code for:
- Pathfinding AI of Swarmling Controller (w/ Amy)
- Resource Depot Controller
- Worker Controller
- Game states and tracking of swarmlings, updates to workers for depots, spawning swarmlings at minibosses, and workers signaling swarmlings to their location when nearby
- Main Menu and scene loading
- Inventory class
- The fireball, leafblade, instant heal, and summon worker cards.
- The fireball and leafblade projectile controllers
- Player ADSR movement speed modifier factor for the move speed of the player
- Boss controller (w/ Amy)

Helped with:
- Market controller implementation and loading card info from the game objects Liam made to store their info
- Card design and mechanics

We tried to use the command pattern for the workers, player, and swarmlings, but eventually found that it would be too limiting to do so given how widely differen the classes were. In the end, all we did was have the workers use the command pattern. We probably should have kept the player with the command pattern because the player controller ended up unreasonably long, but we did keep the command pattern for the inventory/deck class.

The main game state was the main menu versus game scene (eventually help screen or pause menu?). Within the game scene, the game controller managed whether the player had hit game over or victory.

I also managed the game data such as spawning timing and logic for swarmlings (they spawn at the miniboss locations) as well as difficulty increase (the game controller has a setting to allow newly-spawned swarmlings to have higher max health over time). 

The game controller that I created also provided the logic for alerting swarmlings to nearby workers and switching their current target. The swarmling controller picks the nearby player as a target first, then a nearby worker, then a random resource depot to patrol to if no enemies are nearby. 

**Navya:** For the game logic, I worked on the market controller which allowed for the user to buy a card from a market (when near) if the user had sufficient funds. Following that, the card was then added to the deck of the user.
The whole logic for a market was pretty simple as each market sold only one card. Hence, we designated particular markets to particular instancs of a card statically before run.

# Sub-Roles

## Audio

**Jackson:** I did nothing for this role.
**Navya:** I downloaded and created audios for the game. The sounds used were for the cards, game over, win and overall game feel. The sounds created were the error, flip a card and suffle because these sounds were not readily available online. 
Implemented a few, rest of the implementation was done by Liam.

**List your assets including their sources and licenses.**
freesounds.org
**Describe the implementation of your audio system.**

**Document the sound style.** 
The sound style was similar to a midtown rpg fanatsy game. Hence, all the magic cards have a loud bancg and fantasy element to it. 
## Gameplay Testing

**Jackson:** Besides testing our builds during dev time, I did not contribute to this role (yet?).

**Add a link to the full results of your gameplay tests.**

**Summarize the key findings from your gameplay tests.**

## Narrative Design

**Jackson:** Gave input on names and card narrative flavor.

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Jackson:** I did nothing for this role.

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**



## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**

**Jackson:** This was my assigned sub role. I implemented:
- The ADSR class to make movement feel smoother. 
- The health/damage display to swarmlings so that the player can see how close they are to death. 
- The two camera controller types and their logic. We had issues with position lock limiting the player view, but the mouse-focus camera was too fast for some players and we could not get lerping to work, so we allowed the user to switch between them.
- The correspondance of colors in the UI text to the game elements (matter currency label should be in the color of matter resource nodes, for example)
- Screen shakes on damage.
