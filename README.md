[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-f059dc9a6f8d3a56e377f745f24479a46679e63a5d9fe6f495e02850cd0d8118.svg)](https://classroom.github.com/online_ide?assignment_repo_id=449987&assignment_repo_type=GroupAssignmentRepo)


**The University of Melbourne**
# COMP30019 – Graphics and Interaction

## Table of contents
* [Technologies](#technologies)
* [Explanation of the game](#explanation-of-the-game)
* [How To Use It](#How-To-Use-It)
* [Design of Objects and Entities](#Design-of-Objects-and-Entities)
* [Graphics Pipeline and camera motion](#Graphics-Pipeline-and-camera-motion)
* [The procedural generation technique](#The-procedural-generation-technique)
* [Shaders](#Custom-shaders)
* [Particle system](#The-marking-particle-system)
* [Evaluation Techniques](#Evaluation-Techniques)
* [How We Improve](#How-We-Improve)
* [Graphic](#Graphic)
* [Reference](#Reference)
* [Contributions](#Contributions)


## Technologies
Project is created with:
* Unity 2021.1.13f1
* Ipsum version: 2.33
* Ament library version: 999

## Explanation of the game
Our game is a fps game, which has entered the gun battle and sword battle from the first person perspective. Considering the fact that the player experiences the action through the eyes of the protagonist and controls the player character in a three-dimensional space, we developed an aim system for targeting the enemy, and two sets of weapons for the player to switch, in order to improve experiences. 

The game was set under an imaginative and futuristic sci fi background, having concepts such as advanced science and technology. Under our background, human beings are living in an era when robots play an important role in our life. However one day, a disaster happened, robots were attacked by a computer virus, named "Korona". The entire social system was destroyed. The origin of this virus is caused by an unconscious system update. The protagonist has now accepted the task of implementing the source code so that scientists can investigate this problem. It will be a tough trip, collecting the source code while fighting with a former partner.

## How To Use It
On the main scene, players can choose to play standard games or go to tutorials by clicking on the texts on the left of the screen, or change the difficulty by clicking on the button on the right of the screen.

In game, the player can press Esc at any time to pause the game and exit the main scene. Similarly, players can go back to the menu or exit the game on our gameOver scene.

## Design of Objects and Entities
The design of objects follows the background setting of our game. All objects we chose are sci-fi themes. We selected a robot model that looks horrific, so players feel more pressure. 

The animation of the sword was first filmed in real life by one of our group members, with a stick in his hand, then manually motion captured as .anim files.

We used cloister fonts in the UI, showing our controlled character is using an ancient, loyal system, explaining why it is not infected by "Korona", so it produced a feeling of darkness and thickness. 


## Graphics Pipeline and camera motion
Due to the fact that our game is being restricted under first person perspective, any camera motion becomes sensitive to the player. In order to make our players feel more immersive in the battle, we generated the vibration when the player was hit by an enemy, which made the combat damage more obvious and the interaction with the player look better. 


## The procedural generation technique
The bullet hole made by firing the handgun is procedurally generated. The script responsible for generation can be located in Asserts/Scripts/Environment/GeneratedBulletHole.cs there is also the prefab in Asserts/Prefab. The bullet hole is a generated 3D mesh with corresponding texture aimed to make every bullet holes look distinct while being realistic. The mesh can be seen as the assembly of three parts: Central floor, ascend,and descend. 

The central floor height is set uniformly. Lex r be the radius from the central point. The height of  ascend part is r * r - 1,  and the height of descend part is descendSteepness * (|r| - 1 - descendWidth) ^ 2. Three parts are assembled using the function Max(Min(Ascend, Descend), CentralFloor). PerlinNoise is added on top of the height to add the “vein” feeling to the mesh and it is controlled by parameter nose frequency and amplitude. All the parameters involved making the mesh are randomly generated in a range obtained through thorough testing. The texture will be generated based on the mesh. The pixel with greater height will be darker and less transparent. The Fade Rendering Mode is applied to ensure the bullet hole will blend the color of hit material to make it more realistic.

## Custom shaders
location: *Asserts/Shader/OutlineShader.shader* 

It makes the object have a self-defined color outline, which can flash at a certain speed and can only be displayed when aiming. All the functions can be controlled by changing the parameters of the shader. It is implemented by increasing the rendering object’s volume (object.xyz), when the shader is set to show the outline and multiply the outline color and the object texture as the final color.

location: *Asserts/Shader/RefractionInvisibleShader.shader*

It makes the objects render between refraction and reflection with the time. When the object is rendered in a reflective way, it appears invisible. The shader simulates refraction by adding a texture as the normal map, and then grabs the background texture as the texture of the object itself when it is reflected. The object switches between reflection and refraction by changing the object’s uv map. The displacement of the uv map is determined by multiplying the reflection of the normal map’s xy and the time. When the displacement of the uv map is 0, the object is rendered as reflection, rendering the background texture as the object’s texture.

## The marking particle system
location: *Assets/Scripts/WeaponParticleProjectile.cs*

This particle system is used to make the homing projectile chase its target. 
It will get it’s target game object on creation, and loop through all particles, setting their position closer to the target’s position, according to given speed and deltaTime. If no target is set at the time of creation, the particle system will work like a normal system, and all particles will just follow the emission direction and move at their starting speed. No matter having a target or not, the particle will do damage to the enemy on collision, as well as staggering it.


## Evaluation Techniques
We used questionnaire techniques for the querying method and cooperative evaluation techniques for the observational method. We had ten participants in total, and there were five participants in each technique. All the participants were friends or former classmates of our team members. All participants are in the same age range as us (20-25) and all of them are intense gamers who have sufficient gaming experience. Most of them (9/10) had a Chinese cultural background. We acknowledge that we could do better in evaluation if we could have participants from a wider range of age, gaming experience, and cultural background. In this way, it is possible that our game could be enjoyed by more audiences. But unfortunately, this is a compromise that we must make in this situation. 

The questionnaire produced by google form is sent to participants separately along with the game, and they are required to provide feedback after the game is finished. This observation is accomplished by sending games to participants, asking them to play the game while live streaming through Discord, and asking them how they feel about certain aspects of the game when playing the game. From the result, all 10 participants successfully completed the first level of normal difficulty and of course the tutorial, except one participant refused to play the tutorial but went directly to the first level. 

Because the evaluation is given along with the improvement of the game, each participant played a slightly different version of the game, and the improvements made in their feedback and response will be demonstrated in the next section. 

## How We Improve
### Enemy
At first, we developed only one kind of enemy, and we found that it would make players bored. Therefore, we have made some minor changes to the appearance of the enemy using our shader, so that the types of the enemy look more diversified.

Further, when we looked at the users questionnaires, they pointed out that hit feedback did not make them feel involved. Therefore, we have improved our hit feedback, so that the enemy moves backward when being hit, and when the player is hurt, the player's camera will vibrate. After these adjustments, players will feel more realistic and intense in battle.

In addition, we only applied sound effects to the enemy only when it was hit, but when we were observing participants playing our game, we noticed that sometimes they could not find out the enemy's location at first until they got hit. We found that our sound effects were not enough for players to collect information. As a result, we applied sound on the enemy when they moved. 

### Graphic
We placed a fox as moscot, however few participants think that does not fit into our setting and graphic genre, so we deleted this little fox.

Also we added an electric sheep to tutorial scene, as one participant asked "Why player can not die in tutorial? Is it a dream of an android?".

## Reference
[Armor Cyber X91 By Oscar creativo](https://skfb.ly/oowSy) by OSCAR CREATIVO is [licensed](http://creativecommons.org/licenses/by/4.0/) under Creative Commons Attribution.

[(Sci fi) Level design](https://skfb.ly/68vxR) by Pedram Ashoori is [licensed](http://creativecommons.org/licenses/by/4.0/) under Creative Commons Attribution.

[Level5 Zone1 (Level Design 2002)](https://skfb.ly/6tCxy) by hansolocambo is [licensed](http://creativecommons.org/licenses/by/4.0/) under Creative Commons Attribution.

[Sphere Bot](https://skfb.ly/QsWV) by 3DHaupt is [licensed](http://creativecommons.org/licenses/by-nc-sa/4.0/) under CC Attribution-NonCommercial-ShareAlike.

[Explosion Pack 2](https://assetstore.unity.com/packages/vfx/particles/fire-explosions/explosion-pack-2-59883#description) by 3DimensionLine is licensed under Single Entity Use

[HD Heavy Gun](https://assetstore.unity.com/packages/3d/props/weapons/hd-heavy-gun-57734)

[HiTech SciFi Energy Cell](https://assetstore.unity.com/packages/3d/environments/sci-fi/hitech-scifi-energy-cell-154526)

[LowPoly Cyber Ninja Sword](https://assetstore.unity.com/packages/3d/props/weapons/lowpoly-cyber-ninja-sword-129464)

[Sci Fi Chip](https://assetstore.unity.com/packages/3d/props/electronics/sci-fi-chip-162878)

[Sci Fi Futuristic Hand Gun](https://assetstore.unity.com/packages/3d/props/guns/sci-fi-futuristic-hand-gun-90249)

[Sci-fi Guns SFX Pack](https://assetstore.unity.com/packages/audio/sound-fx/sci-fi-guns-sfx-pack-181144 )

[Menu Sci Fi background](https://assetstore.unity.com/packages/3d/environments/3d-free-modular-kit-85732)

https://github.com/przemyslawzaworski/Unity3D-CG-programming/blob/master/outline.shader

https://www.youtube.com/watch?v=943P0dGR4rQ

http://tinkering.ee/unity/asset-unity-refractive-shader/

## Contributions

Jiachen Li: 
* Procedural generation
* Scene manager
* Enemy behavior
* Pathfinding
* Model

Yanting Mu: 
* Player movement (part of)
* Player weapon
* Player interaction
* Player navigation
* UI (part of)
* Level design

Lingyuan Jin: 
* Custom Shader
* Custom Object Material and texture
* Participant live stream observation 
* Participant feedback collection

Jieting He:
* Sound effect + Music
* Menu design
* Video
* Questionnaire