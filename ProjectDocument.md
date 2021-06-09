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

You should replace any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

**Please note that our game was highly ambitious. To accommodate for this, we had to share many different duties across the different roles. This document will show who did what part of the corresponding roles.**

## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**
Being a deckbuilder, the UI is a key element of game feel and flow, as the art on the cards make up the bulk of the player's primary actions.

**Liam:**

*Resource Display* - The in-game UI contains counters for the 3 resources (Mana, Energy, & Matter), an HP counter, and the deck setup which consists of a draw pile, 3 card slots for the player's hand, and a discard pile.

*Playing/Drawing Cards* - The player can right click to draw a card from the draw pile (placing it into the leftmost open hand slot), or if their draw pile is empty, the discard pile is flipped over and placed on the draw pile.
Casting a card places it on top of the discard pile.

*Examining Cards* - At any time when the player mouses over a card in the UI, they can hold R to view a more detailed version of the card that explains exactly what it does and how strong the effect is.
This functionality also extends to markets; the player can mouse over a market in the world and hold R to view a detailed version of the card being sold.

*Notifications* - The player is notified in the UI when their health is low, when they take damage, or when they gain or lose the various resources.
They are also notified when their mouse is over an object that can be examined.

**The following section explains the general process I (Liam) underwent in creating the UI. It's quite long.**

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

**Jackson:** I added the main menu UI and the display health/damage text to the swarmlings.

**Jarod:** See Animations and Visuals. I helped with UI scaling in the main scene, as well as built most of the sprites for cards.


## Movement/Physics

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

**Jackson:** I contributed by adding the collision functions and rigidbody/boxcollider logic for the player, workers, swarmlings, fireballs, leafblades, minibosses, boss, and resource depots.

We used a mix of the Unity physics system and our own scripted physics to modify it when it did not serve our purposes (ignoring collisions and rotations). The game is top down and 2d, so we had to ensure that gravity was disabled.

**Jarod:**
I implemented the 
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

**Liam:** Many of the changes I did would fall under this category, I would say:

*Audio implementation in general* - While Navya procured the audio assets, I added the actual code needed to make them play at proper times. This includes 
when a swarmling/boss/player gets hit,
when a card is drawn or the discard is added back to the draw pile,
and when a card is played (each card type has a unique sound).
This kind of audio is critical for game feel, increasing the satisfaction the player gets from general input and actions.

*UI indicators when resources/HP are gained/lost* - Not only is it important for the player to know exactly how much they've gained/lost of a particular resource, but it's also important for that knowledge to be imparted in such a way that the player notices it immediately.
I added indicators near the resources/HP themselves, that show up whenever the numerical value changes. This way, the player knows exactly how much they gained/lost from a particular action.