# Unity-Moob-Generator

Unity version: 2022.2.19
<br><br>Unity-Moob-Generator - https://youtu.be/bxZQAV2WIrM     (19/06/2023)

<code>This system meets the following assumptions:</code><br>

Part I:
<br>-Creation of several amoobs (min. 3 max. 5, controlled from the inspector)
<br>-Randomly in the range of 2 to 6 seconds (controlled from the inspector) generate a new moob
<br>-Maximum should be no more than 30 moobs at a time (setting also controlled from inspector)
<br>-Moobs move on a limited board (no pathfinding) with speed also inspector-controlled
<br>-If 2 Moobs touch each other, they lose 1 life point each (out of the starting 3)


<code>Script description:</code>                
<br>-AgentGeneratorScript: A script responsible for generating moobs in the game. It creates moobs based on a prefab, defines generation intervals, controls the number of active moobs, and manages their attributes such as name, health, and speed. It also allows assigning random colors to the moobs.

<br>-AgentManager: A script that manages the moobs in the game. It stores references to all the moobs and allows adding and removing moobs from the list. It also provides the functionality to select all moobs in the scene and deselect them.

<br>-AgentScript: A script representing an individual moob in the game. It controls their movement, health, display of health bar and name. It also handles moobs's reactions to collisions, moob death, and plays explosion effect and sound upon death.

<br>-DecalDestroyer: A script responsible for removing decals (e.g., bullet marks) from the game after a certain time. When the decal's lifespan expires, the script removes it from the scene.

<br>-LookAtCamera: A script that makes an object rotate towards the camera, ensuring it always faces the camera. It is used to rotate text in the UI of moobs to make it readable for the player.

<br>-NoCollisionFloor: A script that disables collision with the floor for moobs, preventing them from losing health upon contact with the ground. It works by temporarily disabling the collider of the floor when the moob moves downwards.

<br>-NumberPrinter: A script that displays numbers from 1 to 100 in a text window. It assigns different colors to numbers divisible by 3, divisible by 5, and the remaining numbers. It also allows toggling the visibility of the number window.

<br>-SelectionController: A script handling the moob selection controller. Upon button click, it enables selecting or deselecting all moobs in the scene.
