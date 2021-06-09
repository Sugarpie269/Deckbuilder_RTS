# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**
Technodeck is a deck-builder real-time strategy game in which the player’s objective is to defeat an enemy and its minions in a magical and technologically mechanical themed game setting.

The player, a technomancer, must gather resources to build their own strength to defeat the boss enemy and keep the dark magic at bay. Once the player has acquired upgrades for their ability card deck, they will have the means to finally power down the boss enemy once and for all.

These cards, when played, will allow the player to send damaging projectiles or summon barriers of defense in their battle against the swarmling minions. The player must decide which cards to acquire first, in which the strategy of deck-building will be valuable in gaining power and winning the game.

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**
The player can move around the map using the WASD keys, where W is up, A is left, S is down, and D is right. The keys for 1, 2, and 3 map to the according card slots in the player's hand. The player can use these keys to play the cards in each respective card slot. 

The player can use the Fire2 command (Right click) to draw a card from the deck, or to reload the deck from the discard pile. 

The R key can be used to examine a card when the player's mouse is hovered over that card. This allows the player to learn more about the card's description and capabilities. Additionally, the R key can be used to view a market when the player's mouse is hovered over a building. This allows the player to view what cards are being sold. 

**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replace any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

**Please note that our game was highly ambitious. To accommodate for this, we had to share many different duties across the different roles. This document will show who did what part of the corresponding roles.**

## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**
Being a deckbuilder, the UI is a key element of game feel and flow, as the art on the cards make up the bulk of the player's primary actions.

**Liam:**

(Most of my work here involves GameObjects in the scene hierarchy, so is hard to link meaningful evidence for them. Best to view the project in the Unity editor if needed.)

*Resource Display* - The in-game UI contains counters for the 3 resources (Mana, Energy, & Matter), an HP counter, and the deck setup which consists of a draw pile, 3 card slots for the player's hand, and a discard pile.

*Playing/Drawing Cards* - The player can right click to draw a card from the draw pile (placing it into the leftmost open hand slot), or if their draw pile is empty, the discard pile is flipped over and placed on the draw pile.
Casting a card places it on top of the discard pile.

*Examining Cards* - At any time when the player mouses over a card in the UI, they can hold R to view a more detailed version of the card that explains exactly what it does and how strong the effect is.
This functionality also extends to markets; the player can mouse over a market in the world and hold R to view a detailed version of the card being sold.

*Notifications* - The player is notified in the UI when their health is low, when they take damage, or when they gain or lose the various resources.
They are also notified when their mouse is over an object that can be examined.

*Main Menu and Instructions Scenes* - Jackson implemented the main menu of the game, but I altered it to include a button that takes you to an instructions scene.
Within the instructions are buttons that reveal information explaining the controls, the layout of cards, how to gain resources, the purposes of the markets, and the general player objective (along with a straightfoward story). 
You can start playing at any time from within the instructions scene.
If the player dies in-game, a button appears to take them back to the main menu.

*Credits Scene* - When the player kills the boss, a button appears to take them to a small credits scene where all 5 of our names are listed.
There is a button to return to the main menu as well.

**The following section explains the general process I (Liam) underwent in creating the UI. It's quite long, and I'm unsure if it's necessary, but I'm including it to be safe. I added to this over the course of the project.**

Very nearly everything in the Canvas prefab of our game project was designed and implemented by me. Jarod did come in and organize the Card prefab used in the UI several weeks back, but otherwise the UI is my doing.
The majority of my scripting was done within [PlayerController.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/097c84b74d0517a4735afedfad0746ea5a089940/DeckbuilderRTS/Assets/Resources/Scripts/Player/PlayerController.cs). To start from the beginning:

I added several utilities to the UI: a facedown deck asset, three different card slots to represent the 3 cards in the player's hand, and a faceup discard pile asset. 
I also added most of the numerical counters, e.g. player health and resources available, and any relevant victory/failure text that would appear upon certain conditions being met. 
The only way to lose is to run out of health, so if the game detects the player is at 0 HP, the game over text is displayed. 
I made all of these functions with corresponding [ModifyPlayerX()](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/097c84b74d0517a4735afedfad0746ea5a089940/DeckbuilderRTS/Assets/Resources/Scripts/Player/PlayerController.cs#L298) methods so that others can very easily call said functions to update the player's resources.

Utilizing Jackson's Inventory class, I added functionality for each card slot in the UI to display the proper card stored within. 
At this point in the project, each card was saved as a single sprite, and a new sprite is created and loaded from resources every time, which caused noticeable frame freezes every time a card was played.
I fixed this fairly quickly afterwards to just load the sprites on game launch and then access them each time the relevant card is pulled from the deck instead.

Additional features were added to make the UI more responsive. When the player attempts to play a card from a slot that has no card in it, nothing happens and they are given warning text. 
If they try to draw a card from the draw pile during the cooldown (or when they try to draw but already have a full hand), they are told they cannot draw yet. 
Lastly, if the player's health drops below 25% of their maximum HP, a warning appears to make sure they know they are at risk of dying.
In v0.1.1 and v0.1.2 of the game, these action fails were also accompanied by an error noise, but it was removed because it was found to be too annoying.

Around this time, Jarod finished and implemented a CardTemplate prefab, which handles the displaying of any given card given the correct input parameters (e.g. card title, description, art image, price, etc.).
To go alongside this, I created a [CardInfo struct](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/main/DeckbuilderRTS/Assets/Resources/Scripts/Cards/CardInfo.cs) that would store everything that is needed to fully render such a card, and integrated it into the scripts I had for the UI.
Cards are no longer rendered as a single sprite, but instead are modular so that each aspect of a card can potentially be edited during a game.

However, the cards in the UI are very small, and the vast majority of the information detailed in each card is either impossible to read or useless to the player during most points of action.
To remedy this, I modified this template and made a simplified prefab that displays only the card title, level, and art image.
Now the cards in the UI only display the strictly necessary information to the player, so they can easily tell from a glance what cards they have.

This brought up a new issue: the player should still be able to read the detailed information on a card if they so choose to. 
Ideally, they would be able to mouse over a card and hold down a button to display the detailed version. 
This required a fair big of legwork, primarily involving how to tell if the player's mouse is over a card in the UI. 
I ended up implementing this using the OnPointerEnter & OnPointerExit functions provided in UnityEngine.EventSystems. 
While the player's mouse is over a card, an informational message appears that tells them to hold R to examine said card more closely. 
If they choose to do so, a much larger and fully detailed version of the card will appear on the screen. 
This image takes up a lot of the player's vision, so it is only feasible to display it when the player explicitly asks to do so. 
The image utilizes the previously mentioned CardInfo struct to determine what information to display.

Following this, I added update numbers into the UI whenever the player gains or loses resources (via the aforementioned ModifyPlayerX() functions). 
Now, when the player is hit by an enemy or uses a healing card of some kind, an appropriate number appears in the UI to signify just how much they gained or lost. 
This also applies to the currencies of mana/energy/matter; whenever a worker gains resources for them, a number appears, and whenever the player buys a card from a market, a negative number also appears. 
This isn't as critical to the game infrastructure as the other changes, but is nevertheless important for game feel and responsiveness.

Markets are an important part of the game (they are the only way to acquire new cards for your deck), and so the player needs to be able to tell what markets are selling what card, as well as know when they are and aren't allowed to buy. 
I added functionality for the player being able to mouse over a market and hold the Examine key to display the detailed version of the card the market is selling. 
On this detailed version is the price of the card in mana, energy, and matter. 
If the player purchases the card, it immediately goes to their discard pile, and an appropriate message pops up telling them they bought a card. 
A cooldown is then instantiated, preventing the player from buying cards for the duration. 
If they try to do so (or they try to buy a card when they don't have the resources necessary), an error message appears. 
This error message cannot appear while the successful card purchase message is on screen, to prevent overlap.

At this point, the UI was largely feature complete. 
Last thing that needs to be added is audio effects: playing a sound when the player draws a card, plays a card, and moves their discard back into their deck. 
I was waiting on Navya to provide audio files, however.

Navya eventually procured some audio clips for me to use, so I added functionality within them for the game. 
Each card within its CardInfo struct now also holds an AudioSource to play whenever that card is used (so each card has a unique sound on play to go with their effect). 
I also added sound for drawing a card and replacing the discard into the draw deck, as well as set up game over and victory fanfare when the appropriate events occur.

At this point I created and implemented the Instructions & Credits scenes. The Instructions scene utilizes an [InstructionController.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/main/DeckbuilderRTS/Assets/Resources/Scripts/_GameManagement/InstructionController.cs) script to display the correct information when certain buttons are pressed.

**Jackson:** I added the main menu UI and the display health/damage text to the swarmlings.

**Jarod:** See Animations and Visuals. I helped with UI scaling in the main scene, as well as built most of the sprites for cards.


## Movement/Physics

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

**Jackson:** I contributed by adding the collision functions and rigidbody/boxcollider logic for the player, workers, swarmlings, fireballs, leafblades, minibosses, boss, and resource depots. 

We used a mix of the Unity physics system and our own scripted physics to modify it when it did not serve our purposes (ignoring collisions and rotations). The game is top down and 2d, so we had to ensure that gravity was disabled.

**Liam:** I did nothing for this role.

**Jarod:**
I implemented the 

**Amy:**  
This was originally assigned as my main role. I implemented the player directional movement based on the WASD input keys. I also added movement for the enemy swarmlings, which by default follows a pathfinding movement between two chosen endpoints, or follows a seeking movement towards the player in the case that the player moves within a designated range of the swarmling. This was done with SAP2D as suggested by Jackson, which has built-in pathfinding and obstacle avoiding. This was helpful since we also had obstacles within our map that our sprites would need to maneuver around.

I also helped to implement the physics for projectile movement for the different card attacks along with the others in my team. The projectiles from each card have unique specifications including their damage capability, and have damage effects when they collide with other sprites such as the enemies or the player or the workers. The projectiles are also shot at a specific angle based on where the mouse is located in relation to the player. To do this, we made use of the transform.eulerAngle property to decide which angle the projectile should be.

I also implemented the physics for projectiles shot from the swarmling enemies and boss enemy towards the player. This occurs in the case that the player comes within a certain distance of the enemies, or in the case that the enemies attack the worker sprites.

This relates to the movement concepts that we learned from Exercise 1, as well as the physics and projectile concepts that we learned from Exercise 4, both of which were referenced when building this game.


## Animation and Visuals

**List your assets including their sources and licenses.**
TODO


**Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists.**

** Jarod ** This was my primary role.
I sourced numerous assets for use in the User Interface, Card art, map art, and more (see above). 
After *exhaustive* searches of the free assets in the Unity Asset store, I found several assets that would eventually be used in the live build. I also worked with Liam to help build the UI, with most of my work being the tedium of formatting several canvas elements.
With UI, card design, and asset selection or creation, I contributed to game feel and graphic design, as well as world building, giving character to the entities we build as a team.

***Key assets***
- Sap2d: Specialized 2-dimensional A-Star pathfinding algorithm asset free on the asset store.
- Monster Wizard 2D: Sprites and animations used for the player character
- Slime Asset: sprites and animations used for swarmlings
- 

I was in charge of Card Design, and custom-made the card bases and formats. 
***Specific stuff I made***
- laser bolt assetss/animation sprites - made using adobe after effects and adobe photoshop
- Card Back designs (Blue and Green) traced from source images found via google and bing.
- Card Base designs - made with adobe illustrator
- Game icon and splash screen



**Jackson:** I contributed by providing the programmer art for the Swarmlings, Player (remove later if someone finds something better?), Workers, Boss/Dead-boss, Resource Depots, Fireball projectiles, Leafblade projectiles, and Miniboss. Leafblade image that I created was also used for the temporary ice spike art (remove if it gets replaced). 

We knew we wanted something magical, mechanical, and perhaps organic for the bad guys (and even player) so the swarmlings and boss were designed with the idea of mutated cyborgs. Bosses and swarmlings follow a similar style of green flesh and orange eyes so that the player easily recognizes the connection between the two different enemies.

**Liam:** I did nothing except provide opinions on what looked good and what didn't, for this role.

**Navya:** I contributed to this role by building a tile set map for our game (basically worl-building). I got an asset from the unity store which had the feeling of an "rpg midtown". I felt like that would be the best for our game as it was quite an open-ended theme when we had not decided a story line for our game. I made the map in a way which would allow for the "SAP2D" to calculate colliders easily and kept on making changes on the map as the game progressed because of the colliders not wokring properly.

## Input

**Describe the default input configuration.**

Keyboard and mouse: WASD movement, mouse+r for hovering over cards for information, mouse for optional camera controller panning, left-click enable optional camera controller, right click draw card, [1,2,3] for using the corresponding card slot.

**Add an entry for each platform or input style your project supports.**

Our game only supports the specific keyboard & mouse controls assigned.

**Jackson:** I contributed to the input role by offering my insights into how we will need our input logic to match the capabilities of the player. I created the camera controller that focuses the mouse position.

**Liam:** Input was fairly critical to my main role as it affects how the player interacts with UI. 
However, I didn't do much for this category other than set the card buttons to 1/2/3 and the examine and purchase buttons to R and B.

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
- Inventory class (w/ Liam)
- The fireball, leafblade, instant heal, and summon worker cards.
- The fireball and leafblade projectile controllers
- Player ADSR movement speed modifier factor for the move speed of the player
- Boss controller (w/ Amy)

Helped with:
- Market controller implementation and loading card info from the game objects Liam made to store their info
- Card design and mechanics

We tried to use the command pattern for the workers, player, and swarmlings, but eventually found that it would be too limiting to do so given how widely differen the classes were. In the end, all we did was have the workers use the command pattern. They do so by checking collisions with resource nodes and if they detect a change between resource types, they set their current command to a different resource generation command. We probably should have kept the player with the command pattern because the player controller ended up unreasonably long, but we did keep the command pattern for the inventory/deck class. The [Inventory.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/_GameManagement/Inventory.cs#L8) was written primarily between myself and Liam. I worked on the deck logic end of things, while Liam added functionality to the UI data sharing. The deck is a collection of ICard objects where ICard is the interface for a card class. When the player plays a card from the inventory it calls the OnCardPlayed function of the corresponding hand slot's card.

The main game state was the main menu versus game scene (eventually help screen or pause menu?). Within the game scene, the game controller managed whether the player had hit game over or victory.

I also managed the game data such as spawning timing and logic for swarmlings (they spawn at the miniboss locations) as well as difficulty increase (the game controller has a setting to allow newly-spawned swarmlings to have higher max health over time). 

The game controller that I created also provided the logic for alerting swarmlings to nearby workers and switching their current target. The swarmling controller picks the nearby player as a target first, then a nearby worker, then a random resource depot to patrol to if no enemies are nearby. 

The fireball and other projectile controllers generally fly in the direction they were casted until they hit something or outlive themselves. The other logic pertaining to the projectiles mostly included the hit-detection system and resulting logic, which I implemented.

The card system

You can see my work on the game controller here [GameController.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/_GameManagement/GameController.cs#L1)

My work with the workers can be obserbed at [WorkerController.cs]() [WorkerBasicMatterCommand.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/Entities/Worker/WorkBasicMatterCommand.cs#L1) [WorkerMatterCommand.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/Entities/Worker/WorkMatterCommand.cs#L1) [WorkerEnergyCommand.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/Entities/Worker/WorkEnergyCommand.cs#L1) and [WorkerManaCommand.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/Entities/Worker/WorkManaCommand.cs#L1)

The [BossController.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/Entities/Enemies/BossController.cs#L1) was also primarily written by me, with Amy writing the logic for its basic projectile shoot command.

The [FireballController.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/Cards/Cards%20Casting%20Logic/FireballController.cs#L1) and [FireballCard.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/Cards/FireballCard.cs#L1) were almost completely my work. Their logic was commonly reused by myself and the others to make the other cards and projectiles functional.

**Liam:** I'm not quite sure what category these contributions fall under, so I'm listing them here.

I implemented code for:
- Displaying the information stored within Jackson's Inventory class to the player via the UI (done within [PlayerController.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/main/DeckbuilderRTS/Assets/Resources/Scripts/Player/PlayerController.cs) )
- The [CardInfo struct](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/main/DeckbuilderRTS/Assets/Resources/Scripts/Cards/CardInfo.cs), which stores all necessary information about a card for various scripts to utilize.
- A simplified version of cards to display to the UI, since the fully detailed cards contained too much information to fit on screen at once
- The [Ice Spike card script](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/main/DeckbuilderRTS/Assets/Resources/Scripts/Cards/IceSpikeCard.cs) and [controller script](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/main/DeckbuilderRTS/Assets/Resources/Scripts/Cards/Cards%20Casting%20Logic/IceSpikeController.cs), primarily adapted from Jackson's implementation of Leafblade.
- The [Force Bolt card script](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/main/DeckbuilderRTS/Assets/Resources/Scripts/Cards/ForceBoltCard.cs) and [controller script](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/main/DeckbuilderRTS/Assets/Resources/Scripts/Cards/Cards%20Casting%20Logic/ForceBoltController.cs), primarily adapted from Jackson's implementation of Fireball.
- The entirety of the Instructions scene & Credits scene.

**Navya:** For the game logic, I worked on the market controller which allowed for the user to buy a card from a market (when near) if the user had sufficient funds. Following that, the card was then added to the deck of the user.
The whole logic for a market was pretty simple as each market sold only one card. Hence, we designated particular markets to particular instancs of a card statically before run.

**Amy:**   
I implemented code for the LightningBlast card and controller, which is designed to shoot 4 lightning blasts at the mouse's position after some delay. This is similar to the LaserBeam card for the delay, and similar to the ForceBolt / FireBall / etc cards for the projectile like motions.

# Sub-Roles

## Audio

**Jackson:** I did nothing for this role.

**Liam:** Using the assets found by Navya, I implemented them into the game by simply adding them as AudioSources in various GameObjects, and then calling them with GetComponent<AudioSource>().Play() when needed.
The sound effects I added functionality for include:

- When the player draws a card
- When the player replaces their discard pile into their draw pile
- When the player casts a card (every card has a unique sound effect)
- When the player purchases a card
- When the player gets hit by an attack
- When the player is killed by an attack
- When a swarmling gets hit by an attack
- When a swarmling is killed by an attack
- When the boss is hit by an attack
- When the boss is killed by an attack

**Navya:** I downloaded and created audios for the game. The sounds used were for the cards, game over, win and overall game feel. The sounds created were the error, flip a card and suffle because these sounds were not readily available online. 
Implemented a few, rest of the implementation was done by Liam.

**List your assets including their sources and licenses.**
freesounds.org

### Assets ###
- https://assetstore.unity.com/packages/2d/environments/tiny-rpg-town-environment-88293
- https://assetstore.unity.com/packages/audio/music/action-rpg-music-free-85434

### Audios ###
- https://freesound.org/people/eggdeng/sounds/502658/
(Creative Commons 0 License)
- https://freesound.org/people/VKProduktion/sounds/217502/
(Creative Commons 0 License)
- https://freesound.org/people/f4ngy/sounds/240776/
(Attribution License)
- https://freesound.org/people/JoelAudio/sounds/77691/
(Attribution License)
- https://freesound.org/people/EminYILDIRIM/sounds/563662/
(Attribution License)
- https://freesound.org/people/Milky0519/sounds/506626/
(Creative Commons 0 License)
- https://freesound.org/people/lulyc/sounds/346116/
(Creative Commons 0 License)
- https://freesound.org/people/Deathscyp/sounds/443806/
(Creative Commons 0 License)
- https://freesound.org/people/Timbre/sounds/452540/
(Attribution NonCommercial License)
- https://freesound.org/people/gamebalance/sounds/455228/
(Attribution License)
- https://freesound.org/people/NewEonOrchestra/sounds/170602/
(Creative Commons 0 License)
- https://freesound.org/people/Daleonfire/sounds/376694/
(Creative Commons 0 License)
- https://freesound.org/people/ihitokage/sounds/552038/
(Creative Commons 0 License)
- https://freesound.org/people/nsstudios/sounds/321107/
(Attribution License)
- https://freesound.org/people/humanoide9000/sounds/466133/
(Attribution License)
- https://freesound.org/people/TheZero/sounds/368367/
(Creative Commons 0 License)
- https://freesound.org/people/Euphrosyyn/sounds/380493/
(Creative Commons 0 License)
- https://freesound.org/people/broumbroum/sounds/50543/
(Attribution License)
- https://freesound.org/people/Hoggington/sounds/536603/
(Creative Commons 0 License)
- https://freesound.org/people/tonsil5/sounds/416839/
(Creative Commons 0 License)
- https://freesound.org/people/scorpion67890/sounds/396805/
(Attribution License)  
  
  
  
**Describe the implementation of your audio system.**

**Document the sound style.** 
The sound style was similar to a fantasy rpg game. Hence, all the magic cards have a loud bang and a fantasy element to them. 

## Gameplay Testing

**Jackson:** Besides testing our builds during dev time, I did not contribute to this role (yet?).

**Liam:** This was my subrole, so I gathered several people to test out the game and provide feedback. The results are too long to list here, and so I compiled them within this document (should be open to comment by anyone with the link): https://docs.google.com/document/d/1tpUaciaweEwHjLk7jGraNJTr-MNyhMOIsSD4SJJkF2M/edit?usp=sharing

**Summarize the key findings from your gameplay tests.**

**Liam:** There were 3 versions of the game we tested with outside participants (v0.1.1, v0.1.2, and v0.1.3). Going into testing, I expected runs of the game to last around 5-10 minutes.

However, the first run of v0.1.1 approached around 25 minutes or higher. It was pretty clear that the game was far too difficult and resource gains far too low, so v0.1.2 introduced several sweeping balance changes to remedy this.
The cooldown of drawing a card was halved, and the delay on a worker generating resources from a node was halved (thereby doubling the rate of resource gain). The cost of several cards was decreased to make them faster to access. 

Further testing of v0.1.2 revealed that we made the game a little too easy, especially regarding purchase prices with the faster resource gain, so prices were adjusted again. More importantly, explanatory elements of the UI were enlarged because many players didn't notice them at all.
Based on player experience, we removed the stipulation that the player could not destroy their Summon Worker cards (which are vital to game progression), and instead opted to allow them to be destroyed while providing a market that sold them for free.
We realized at this point that the error noises upon trying to draw a card while on cooldown, trying to play a card from an empty slot, or purchasing a new card on cooldown were too annoying and were often spammed, so we removed these audio effects and instead repurposed the error noise to loop while the player is on low health.
Lastly, to address the easier gameplay, we set an increasing difficulty scale so that swarmlings spawned faster as time went on, capping out at 7 minutes into a game (which should be more than enough time for the player to defeat the boss). v0.1.3 ended up containing a mix of balance and quality of life adjustments.

## Narrative Design

**Jackson:** Gave input on names and card narrative flavor.

**Liam:** I created and revised the majority of the descriptions and flavor text on the cards, opting for a step in the comedic direction when it came to tone. The primary narrative in the Objectives section of the instructions scene was devised by Jarod, though I tweaked and revised it a little to flow better.
I also set up the game over and credits screens to contribute to this approach.

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Jackson:** I did nothing for this role.

**Liam:** I did nothing for this role.

**Amy:**  
I made a press kit for the game on a Wix site (https://technodeckgame.wixsite.com/press). This includes all of the necessary content from DoPressKit() and the sample Press Kit websites provided by the professor. The site has different pages for Home, Factsheet, Description, Videos & Media, Press Releases, Team, and Contact information.

The Home page has some general overview about the mechanics of the game, as well as the storyline behind the purpose and objective of the game. 

The Factsheet page has information about the location and date in which the game was released, as well as some brief contact information and social media links. This includes an email address, Twitch, Facebook, Wix Press Kit, and the Github repository where the code is hosted. 

The Description page has further details on the game. This includes the Gameplay System, which describes how the game is set up and how to play the game. This page also details key features of the game, specifically the cards and the deck-building capability, the resources and potential to purchase new cards, and the markets which allow users to buy new cards. This page also contains details about the inspiration behind the game and other similar games.

The Videos & Media page contains images of the game in play, including images of the latest version and previous versions. This page also features trailers for the game, and screenshots of the instructions pages from the game's UI. 

The Press Releases page details the various version releases of the game, as well as links to Build files. 

The Team page features the people in the team and a brief bio.

The Contact page highlights the contact information and social media links for the game.

**Include links to your presskit materials and trailer.**  
Press Kit: https://technodeckgame.wixsite.com/press
Trailer: https://youtu.be/fUFxNVJltTk 

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**  
I chose specific screenshots and sequences from the game that I thought were the most important to highlight in a short trailer clip and as images in the press kit. 

This includes a brief overview of the game play rules and features that are included in the game UI. The trailer shows the player navigating the map and interacting with the enemy swarmlings and boss. A major part of the game is the deck-building aspect, so a large portion of the trailer shows many of the various cards in action as the player utilizes different attacks against the swarmlings or boss enemies. 

The trailer also includes the sound effects as they would occur in the live gameplay. This is to provide an accurate representation of the game.

Similarly, the press kit has images of various parts of the gameplay experience, which show the player navigating the map, the player utilizing the card attacks against the swarmlings, and the player fighting the boss enemy. 


## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**

**Jackson:** This was my assigned sub role. I implemented:
- The ADSR class to make movement feel smoother. [PlayerADSR](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/Player/PlayerADSR.cs#L1). This was done without a delay phase for simplification.
- The health/damage display to swarmlings so that the player can see how close they are to death. 
- The two camera controller types and their logic. We had issues with position lock limiting the player view, but the mouse-focus camera was too fast for some players and we could not get lerping to work, so we allowed the user to switch between them. [CameraController.cs](https://github.com/Sugarpie269/Deckbuilder_RTS/blob/f9acf6fb83b0d1b00a84f4d92c95ab980b589725/DeckbuilderRTS/Assets/Resources/Scripts/_GameManagement/CameraController.cs#L71)
- The correspondance of colors in the UI text to the game elements (matter currency label should be in the color of matter resource nodes, for example)
- Screen shakes on damage.

**Liam:** Some of my work would fall under this category, I would say:

*Audio implementation in general* - While Navya procured the audio assets, I added the actual code needed to make them play at proper times. 
See the Audio section for what sound effects I specifically implemented.
This kind of audio is critical for game feel, increasing the satisfaction the player gets from general input and actions.

*UI indicators when resources/HP are gained/lost* - Not only is it important for the player to know exactly how much they've gained/lost of a particular resource, but it's also important for that knowledge to be imparted in such a way that the player notices it immediately.
I added indicators near the resources/HP themselves, that show up whenever the numerical value changes. This way, the player knows exactly how much they gained/lost from a particular action.
