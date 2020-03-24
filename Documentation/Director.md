# The Director

## Director Components 

The directors are designed to track the progress and flow of each level.

```Context```: Tracks the current level context.  The thought was we would eventually use different level 
context implementation based on the type of level, but I'm not sure other implementations would ever really be needed.

```SetStartPhase```: This method should be used to set a specific first phase for this level.  In the future I'd like to find a smarter way to do this, preferably from the unity inspector or some thing.  

## ILevelContext

The level context provides an abstraction layer between the Director and the specific context.  The director never directly cares about the current level phase, the context does.  

The context should be used as a way to report information back to the director from the level phases themselves.  The director and the level phase shouldn't ever reference each other.

```CurrentPhase```: Tracks the current phase of the level

```IsCompleted```: When true, the director will prepare the next phase when it is ready.  This can't be 
called directly, and should be set to true via the FlagAsComplete() method.

```CompletePhase```:  Calls the PhaseComplete method on the current phase.

```UpdatePhase```: Calls the PhaseUpdate method on the current phase.

```BeginPhase```: Handles instantiating and initializing the next phase.  Also calls PHaseBegin on the new
phase right away.

## ILevelPhase 

A level phase should be thought of as a single, tiny chunk of level logic.  One single phase can literally
just be a single enemy or group of enemies.  There should not be multiple waves of enemies spawned in a single phase.  

These classes can be pretty light weight i'd think, and very little logic needs to exist in them, other than checking if enemies still exist, etc.  It's totally okay to have a ton of these classes because they're so tiny, and keeping them decoupled helps with ease of development.

Examples of what a single level phase should usually be:

### Example 1
**Begin**: Spawn 10 Enemies

**Update**: Wait for those 10 enemies to be destroyed

**Complete**: Start another phase

### Example 2
**Begin**: Spawn 5 larger enemies

**Update**: Wait for those 5 enemies to be destroyed, but also randomly spawn some tiny enemies too (that don't contribute to phase completion)

**Complete**: Start another phase

### Example 3
**Begin**: Show some dialogue 

**Update**: Mark the level phase as complete (the level director won't advance until all dialogue is gone)

**Complete**: Start another phase

The Level phases are super easy to implement.  Simply create a class that implments ILevelPhase.

```PhaseBegin```: This method is used for spawning enemies, starting dialogue, etc.  It is only called once.

```PhaseUpdate```: This method is called every frame if the phase is not completed.  This is used to check the conditions for phase completion, spawn additional enemies, or any other logic specific to this phase.  *It should eventually call context.FlagAsComplete();*

```PhaseComplete```: Can do whatever, but *Should eventually begin the next phase via ```context.BeginPhase()<T>```



