# How to add new avatar animations to the text writer

1. Add a new folder under ` Animations > SpriteSheets > Avatars ` for your character if they do not exist
2. Add a sub folder for their mood or state, like "normal", "angry", etc
3. Drag your sprite into this folder, set it up as a sprite sheet (multiple sprites) in unity
4. Open the text writer prefab, click on `Image`
5. Open the animation panel
6. Click the animation selector and "Create new clip"
7. Save the animation in that same folder that you added the sprite sheet into 
8. Add all frames from your sprite sheet
9. Double click the avatar animator controller, located in the root of the `Avatars` folder
10. Hopefully the animator panel shows up
11. Your new animation should just be awkwardly floating there
12. Right click on `Any State` and click the only option "Make Transition"
13. Click on your newly animated animation to add a transition
14. Click on the newly added transition
15. Set the `transition duration` to zero.  I'm not sure this matters for sprite sheets but do it anyway.
16. At the bottom, notice the `conditions` panel.  Click the plus to add a new condition
17. Set the left side (the parameter name) to "animation_id"
18. set the middle (the operator) to "Equals"
19. Set the right side (value) to a number that isn't being used yet.  Or open the `TextAnimatorComponent` since we have to do that anyway, and pick a number one larger than the biggest number in the `AvatarAnimation` enum.
20. Add a new value to the `AvatarAnimation` enum.  The name should be our animation name, the value should be the new number we just added.  
21. You should now be able to reference the animation id via a TextWriterStyle by the enum value (as text)
