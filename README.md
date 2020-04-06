# ProjectBB-8

```
     _-=- _    
    ´´´-` ``   
   (   (O)  )  
   []=__○_ []  
 ´  |    °   ` 
´-´  ° _---_  `
|     ´ □=□  `|
|   ° |  -   ||
`     | □=□  ´´
  `--_____-- ´ 

Team:
Jordan Bleu
Nick Pientka

```

## Concept

The game is a space shooter with a super overly dramatic / cheesy 80s style badass story.  See the  ```/Documentation/StoryOverview.md``` file for a bit more details.


## Gameplay

* Space shooter style, enemies come from top of screen (galaga style)
* Multiple weapons types
* Upgradable Ship (?)
* Side missions

## Links
* **GitHub**: https://github.com/jordanbleu/ProjectBB-8
* **Trello**: https://trello.com/b/i2WahbPS/project-bb-8

## Development

* Use the trello board to figure out what to do
* Pull a card over to your column
* Create a branch off of master named whatever you want 
* Make code change, commit, push, create pull request
* Move trello card over to "done"

___

## Unity Project Structure

#### Animations

Animation files (but **not** sprite sheets) go inside the here.  There should be a folder added for each animator that contains the actual animations and the animator controller.  Sprite sheets will be added to the sprites folder.

#### Rendering

This folder likely won't ever need to be touched at all.  It contains the LWRP (LightWeight Render Pipeline) data assets, which get applied to scenes automatically if you create them.

#### Resources

> Everything in this folder will be included in the game build.

This folder contains resources that can be loaded in code on the fly.  This includes things such as prefabs, static files, etc.  It can also be used to load things such as textures but i'm not sure why we'd need to do that.

Resources contains several sub-folders:

* Prefabs: Prefabs should be added to categorized folders no more than 1 layer deep

Example:

*Resources > Prefabs > **Projectiles > Asteroid***

Exceptions can be made to this rule but the idea is to limit how deep the folder structure goes so we can limit the amount of magic strings in code we need to access these files.

#### Scenes

All scenes should be saved in this folder.  All scenes that are not going to be included in the final build can be placed in the "testingScenes" folder.

#### Source

This is where all the code goes (Except unit tests).  We will come back to this.

#### Sprites

All sprites and sprite sheets should go inside the sprites folder.  Any non-production ready sprite should be placed inside the "Testing" folder so nobody gets confused and uses a placeholder sprite.

#### Unit Tests

This folder contains code for unit tests specifically.  This folder shows as a second project in visual studio because of it's separate Assembly Definition file (.asmdef).

There's also a **_testResoruces** folder here.  This can be used for static files that are part of unit tests.

#### ProjectBB-8 file

This is an **Assembly Definition** file.  These are similar to .csproj files but they only compile bits and pieces of the code base.  This is how unity is able to compile so insanely fast.  

See:
https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html

Eventually we may wish to look into breakout out our project into separate assemblies to make it build quicker but as of right now, i'd avoid touching this file.  If we add any external references, unity will automatically add their references to this file for us.

___

## Code Standards

### Folder / Namespace structure

> Every single piece of game code should exist in the "source" folder.

It's actually extremely difficult to add code outside of this folder because the assembly definition is set up so that visual studio mounts this folder as its root in the project. 

Things outside this folder will not build.  

### Code Structure

> Abstractions go inside the base folders, implementations go in the root of the folder

Folders Allowed:

* **Base**: abstract classes
* **Constants**: Static constant classes related to this group of code
* **Interfaces**: Interfaces only
* **Factory**: Factory classes

Any other classes should be instantiatable implementations.  We don't need sub-folders to organize these implementations any further.  If there's still confusion, either add a new folder to the root of the source / components folder or make the names better.  

Components follow the same rule except their folders are within the components folder.  The components folder itself additionally follows this rule as well.


> Namespaces should follow this same structure

Namespaces should follow the exact same structure as the folders.

> Every class which inherits from ```ComponentBase``` needs to go inside the *Components* folder

(more on this later)

> Classes which are not components can be added to folders in the Source folder, and should be unit tested if possible

Non-Unity specific code can be added outside the Components folder.  Test driven development becomes possible here and should be used whenever we can.  

> Extension classes should go in the "Extensions" folder and be named in an obvious way

Extension classes should be named like ______Extensions.  This gets weird though.  If you're extending the string class you should name it StringExtensions, but for Vector3 / Vector2 its okay to just put the code inside VectorExtensions because Vector3 is a sub class of Vector2.

> Avoid Magic Strings and Magic Numbers

For things like UI and dialogue, **NEVER USE HARD CODED STRINGS**.  Strings such as these need to be added to strings xmls in case we ever wish to localize our game.  

For references, etc. prefer to use a private constant string, even if the string is only used once.  The memory footprint is the same, and it keeps things readable.

Same thing with integers or floats.  I'd rather see a constant at the top saying PLAYER_START_HEALTH so i don't need to sift through code to see what the player start health is.

### Components

> Every single "component" class should be somewhere within the "Components" folder, and everything within the "Components" folder should be a component class

A **component class** is a class that can be dragged and dropped onto game objects in Unity.  Having all of these classes in this folder makes it beautifully obvious what can be dragged onto objects in unity.

The "base folders" rule still applies to the component classes too though.  We can have a "base" folder inside a component class folder, or whatever, as long as we're not dinguses who try to drag a base class onto an object.

In our case, these classes will almost always inherit from ```ComponentBase```.  Component Base is a class that sits between our components and the Unity "MonoBehaviour" class in the inheritance structure. 

**Random Pro tip**: Be careful creating component scripts via unity (rather than visual studio).  Unity doesn't add namespaces, so you'll have to add one manually.

#### ComponentBase

```ComponentBase``` gives us a few main benefits:
 
* It allows us to write global helper methods.  These include things like .```GetRequiredComponent<T>``` which automatically performs null checking if we expect a component to exist on an object.
* It gives us the ability to control every single component on a global level.  Things such as global exception handling or game speed manipulation are now much easier.  
* Overriding values for Step, create, etc. is much easier than remembering the arbitrary method names "Update", "Activate", etc.

Basically, every single component should inherit from ComponentBase.  The one exception we have so far is for the ```SystemObjectBehavior```, which can't because it gets created *from* ComponentBase which causes infinite recursion.

```ComponentBase``` has a few overridable methods: 

* **Construct**: This is the closest thing to a constructor we are allowed to use in Unity.  Here we set up references, etc.  At this point in code, the structure of everything is set up but things aren't rendered yet. (Unity's equivalent of Awake())
* **Create**: This happas after Construct, when the object is ready to start doing things.  Here you can put code such as initializing positions, etc. (Unity's equivalent of Start())
* **Step**: This is the main game loop.  This code is called every single frame.  (Unity's equivalent of Update())
* **Destroy**: This code runs just before an object is destroyed.  (Unity's equivalent of OnDestroy())
* **Activate**: This code runs when an object is activated (Unity's equivalent of OnEnable())

Another important job of the ComponentBase is keeping track of the ```SystemObject```.

#### SystemObject / SystemObjectBehavior

The system object is an important object, and it should be an object that always exists no matter what.  It never needs to be manually added to the unity hierarchy because it gets created by ```ComponentBase``` whenever one is created.  

The system object is simply a prefab that gets created.  But it contains our InputManager which tracks user inputs automatically.  The InputManager can be accessed from any ComponentBase via the ```InputManager``` property.  If the System Object needs to be accessed directly, you can use the ```FindOrCreateSystemObject()``` method.

#### InputManager

> Do not ever use unity's input system on its own.  It is stupid and dumb.

When checking inputs, you should set up key bindings, and is the ComponentBase's ```InputManager``` property, along with one of the following methods:

* ```IsKeyDown()```: Check if a key is currently held down this frame
* ```IsKeyHit()```: Checks if a key is currently pressed this frame but wasn't pressed last frame (aka, key was PRESSED)
* ```IsKeyReleased```: Checks if a key is currrently up, but was down last frame
* ```GetAxis```: This will return a raw input value.  For buttons, this simply returns 1 for pressed or 0 for not pressed.  However for things like controller joysticks, this will returna value *between* 0 and 1.

### Code Style

> The first thing in any component should be the inspector variables

These are the most exposed variables and thus should be the easiest to find. 

Additionally...  

> Inspector variables should never ever be public

Public fields goes against one of the main pillars of object oriented programming (encapsulation) and I hate that Unity encourages that.  If the inspector variable will be used only within the current component, do this:

```C#
  [SerializeField]
  private string testStringus;
```

If an inspector value should be exposed to the public or protected, do this:

```C#
  [SerializeField]
  private string _publicInspectorString;
  public string PublicInspectorString
  {
      get => _publicInspectorString; 
      set => _publicInspectorString = value; 
  }
```

This is an ugly giant block of code right?  That leaves us to the next thing:

> Avoid inspector values unless they are useful

It'd be nice if our code base was smart enought to set itself up without us.  If a reference can be grabbed programatically, don't use inspector variables.  Utilize code such as ```GetRequiredComponent```, ```GetRequiredObject```, etc.

The preference is to have prefabs be completely usable with zero configuration on the inspector.  This will save us time setting up tons of levels.

> Summary Tags

Pretty much every class should have a summary tag.  Public methods on classes generally should as well.  

Private members don't need summary tags, but a comment isn't bad.  I should be able to figure out a class' purpose without looking at code.  

> When in doubt, just follow the .NET standard

Just google it.








