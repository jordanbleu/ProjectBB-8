using Assets.Source.Components.AI;
using Assets.Source.Components.Audio;
using Assets.Source.Components.Base;
using Assets.Source.Components.Enemy;
using Assets.Source.Components.Spawner;
using Assets.Source.Components.Timer;
using Assets.Source.Constants;
using Assets.Source.Director.Interfaces;
using UnityEngine;

namespace Assets.Source.Director.Testing.TestLevel
{
    // todo:  I think we might need a naming scheme to easier tell what order these are in, 
    // i was thinking numbers but that will make it harder to add things in between phases
    public class TestLevelPhase1 : ILevelPhase
    {
        private bool intervalReached = false;

        private GameObject asteroidSpawner;
        private GameObject intervalTimerObject;

        private GameObject flyingVFormationPrefab;
        private GameObject flyingVObject;
        
        public void PhaseBegin(ILevelContext context)
        {
            flyingVFormationPrefab = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Actors/Formations/{GameObjects.Formations.FlyingV}");

            GameObject asteroidPrefab = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.Projectiles.Asteroid}");
            
            // Create an asteroid spawner which will generate a random asteroid ever buncha milliseconds
            asteroidSpawner = ComponentBase.InstantiateInLevel("AsteroidSpawner", Vector3.zero, typeof(AutoSpawnerComponent));
            ComponentBase.GetRequiredComponent<AutoSpawnerComponent>(asteroidSpawner).Initialize(500, asteroidPrefab);

            // This method is used to spawn enemies, initialize the phase, etc
            GameObject enemy = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Actors/Enemies/Shooter_AI");
            
            GameObject inst = ComponentBase.InstantiateInLevel(enemy);
            inst.transform.position = new Vector3(-2, 3f, 3);
            GameObject inst2 = ComponentBase.InstantiateInLevel(enemy);
            inst2.transform.position = new Vector3(2, 3, 4);

            // A formation will spawn after a while 
            intervalTimerObject = new GameObject("FormationSpawnTimer");
            
            intervalTimerObject.AddComponent<IntervalTimerComponent>();
            IntervalTimerComponent timer = ComponentBase.GetRequiredComponent<IntervalTimerComponent>(intervalTimerObject);            
            timer.SetInterval(3000);
            timer.OnIntervalReached.AddListener(SpawnFormation);
            timer.SelfDestruct = true;
            timer.IsActive = true;

            // Play some dank tunes
            ComponentBase.GetMusicPlayer().Loop(MusicPlayerComponent.Song.Prototype);

        }


        public void PhaseUpdate(ILevelContext context)
        {
            // This method is used for spawning more enemies, or checking if the phase is completed

            if (intervalReached)
            {
                // Check to see if any enemies exist 
                if (!ComponentBase.ComponentExists<ShooterEnemyAIBehavior>() && flyingVObject == null)
                {
                    // todo: prevent from calling complete phase directly
                    context.FlagAsComplete();
                }
            }
        }

        public void PhaseComplete(ILevelContext context)
        {
            GameObject.Destroy(asteroidSpawner);
            Debug.Log("Phase 1 completed, you killed the guy!");
            context.BeginPhase(new TestLevelPhase2());
        }

        // After the interval has completed, spawn a grunt formation
        private void SpawnFormation() 
        {
            intervalReached = true;
            flyingVObject = ComponentBase.InstantiateInLevel(flyingVFormationPrefab);

        }

    }
}
