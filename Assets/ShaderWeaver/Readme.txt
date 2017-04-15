-------------------------------------------------------
        Shader Weaver - Easy and funny to create
             Copyright Reserved by Jackie Lo
                    Version 1.01
-------------------------------------------------------

Thank you for purchasing Shader Weaver!

If you have any questions, suggestions, comments or feature requests,
please email JackieLo@aliyun.com


---------------------------------------
 Support, documentation, and tutorials
---------------------------------------
Home:
www.shaderweaver.com

Tutorials can be found here:
www.youtube.com/channel/UCoNy1syiYxAuI5Co4vxNkAA 


--------------------
Description
--------------------
Shader Weaver is a node-based shader creation tool specializing in 2d/UI/Sprite effects,
giving you the artistic freedom to enhance cards/icons/sprites in a visual and intuitive way.
Distinctive nodes and workflow makes it easy to create impressive 2d effects and save huge time.
Use handles/gizmos to make effects rather then input tedious numbers.
Draw masks and create remap textures with convenient tools inside Shader Weaver.Graphics tablet is supported.
Shader knowledge or code is NOT required.
Support both Unity Free and Pro.


--------------------
Features
--------------------
-Growing Samples
A pack of sample effects including shaders and textures to study and use freely.

-Intuitive interface 
Clean and intuitive user interface. 

-Mask Texture Creation
Draw masks to divide areas for individual sub-effects.

-UV Distortion
A visual way to distort uv corrdinates.

-UV Remapping
A unique way to make path along effects and object surrounding effects.

-Simple Operation
Use handles/gizmos like what you are used to do.

-Preview
Nice width/height corresponding preview.

-Hot keys
Comfortable hot keys speed up editing and drawing.

-Play Mode
Edit and update in play mode.

-Copy Paste
Support copy and paste. Reuse nodes from other Shader Weaver project.

-Depth
Depth Sorting.

-Visual Modes
View textures' individual rgba channel and choose what to see by setting layermask.

-Sync
No extra files to sync over version control system. All Shader Weaver data are stored in .shader file.  


--------------------
 Quick Start
--------------------
(1)Open editor		: Window -> Shader Weaver
(2)Place nodes 		: Drag nodes from left dock to main canvas
(3)Connect nodes	: Data flows from left to right, drag wires from node ports to create connections.
(4)Edit				: Edit (assign textures, draw masks, create remap texture...)
(5)Save				: Click Save button on the top dock,and outcome will show up on the top-right corner.


--------------------
 Intro to nodes
--------------------
Root:	
This is the main node.Assign base texture here.Choose type between Default/Sprite/UI/Text.

Mask:
Draw mask to masking sub-effects.Mask node's child node effects will ONLY show in the masked area.

Color:
Show textures. Depth is for display order, highest depth is shown on the top.

UV:
Use rgba channel of this node'texture to relocate parent's uv coordinates. Use mainly for distortion.Here are some example effects,
floating flags,flowing water. 

Alpha:
Use one of the rgba channel in this node'texture to do detailed aplha animation for parent nodes.

Remap:
Supply UV corrdinates to the parent node. Drag mode is to lay parental effects on one side of the shape.
Line mode is to make parental effects follow the path you created. In the texture we made,
Red supply  the horizontal corrdinates to parent nodes,Green supply  the vertical corrdinates.
Asix x:R[0,1]=U[0,1]	
Asix y:G[0,1]=V[0,1]


--------------------
 Hotkeys
--------------------
[All windows]
Drag canvas:		Alt + Left Mouse Button 
Scale canvas:		Scrollwheel
Open project:		Alt + O
Save project:		Alt + S (before saved)
Update project:		Alt + S (after saved)
Copy:				Ctrl + C
Paste:				Ctrl + V
Undo:				Ctrl + Z
Redo:				Ctrl + Y

[Main window]
Break connections:	Alt/Ctrl + Click node port
Delete selection:	Delete

[Edit windows]
Along x/y asix:		Shift + operation

[Mask window]
Brush/Eraser size:	'[' and ']' 
Opacity:			'-' and '=' 
Tolerance:			'[' and ']' 
Invert: Ctrl + I

[Remap window - Drag]
Deviation:			'[' and ']' 
Delete all:			Delete				

[Remap window - Line]
Size:				'[' and ']' 
Delete all:			Delete	
		

-----------------------------
 Texture Import Settings
-----------------------------
The following settings will be set to texture automatically when it is used in Shader Weaver.
Textures:
Read/Write Enable -> true
Generate Mip Map -> false

Sprites:
Mesh Type -> Full Rect
