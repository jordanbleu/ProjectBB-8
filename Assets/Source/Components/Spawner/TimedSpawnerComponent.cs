using Assets.Source.Components.Base;
using Assets.Source.Components.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Spawner
{
    /// <summary>
    /// Used to spawn any objects on limited timed intervals.  
    /// 
    /// The items will be spawned in the order specified, and the interval will be reset each time.
    /// So setting a time of 2000 milliseconds means that the object will be spawned 2 seconds after the last
    /// thing was spawned.
    /// </summary>
    public class TimedSpawnerComponent : ComponentBase
    {
        [SerializeField]
        [Tooltip("Specify the objects to spawn, in order.  The milliseconds are the time to spawn AFTER the last object was spawned.")]
        private List<Spawn> Spawns;

        private IntervalTimerComponent intervalTimerComponent;

        private Queue<Spawn> spawnQueue;
        private Spawn currentSpawn;

        public override void ComponentAwake()
        {
            intervalTimerComponent = GetRequiredComponent<IntervalTimerComponent>();
            intervalTimerComponent.OnIntervalReached.AddListener(TimerComplete);
            spawnQueue = new Queue<Spawn>(Spawns);

            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            PrepareNextSpawn();
            base.ComponentStart();
        }


        public override void ComponentUpdate()
        {

            base.ComponentUpdate();
        }

        private void TimerComplete()
        {
            if (currentSpawn.SpawnObject == null) 
            {
                throw new UnityException("Looks like your forgot to drag an object to spawn in one of the 'Spawns' for your TimedObjectSpawner");
            }

            InstantiateInLevel(currentSpawn.SpawnObject, currentSpawn.Position);    
            PrepareNextSpawn();
        }

        private void PrepareNextSpawn() 
        {
            if (!spawnQueue.Any())
            {
                Destroy(gameObject);
            }
            else
            { 
                currentSpawn = spawnQueue.Dequeue();
                intervalTimerComponent.SetInterval(currentSpawn.Milliseconds);
            }
        }


        [Serializable]
        public struct Spawn {
            public int Milliseconds;
            public GameObject SpawnObject;
            public Vector2 Position;
        }

    }
}
