using Assets.Source.Components.Base;
using Assets.Source.Components.Enemy;
using Assets.Source.Components.Spawner;
using Assets.Source.Constants;
using Assets.Source.Director.Interfaces;
using UnityEngine;

namespace Assets.Source.Director.Testing.TestLevel
{
    // todo:  I think we might need a naming scheme to easier tell what order these are in, 
    // i was thinking numbers but that will make it harder to add things in between phases
    public class TestLevelPhase1 : ILevelPhase
    {
        private GameObject asteroidSpawner;

        public void PhaseBegin(ILevelContext context)
        {
            GameObject asteroidPrefab = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.Asteroid}");
            
            // Create an asteroid spawner which will generate a random asteroid ever buncha milliseconds
            asteroidSpawner = ComponentBase.InstantiateInLevel("AsteroidSpawner", Vector3.zero, typeof(AutoSpawnerComponent));
            ComponentBase.GetRequiredComponent<AutoSpawnerComponent>(asteroidSpawner).Initialize(500, asteroidPrefab);

            // This method is used to spawn enemies, initialize the phase, etc
            GameObject enemy = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Actors/{GameObjects.ShooterEnemy}");
            
            GameObject inst = ComponentBase.InstantiateInLevel(enemy);
            inst.transform.position = new Vector3(0, .5f, 1); 
        }


        public void PhaseUpdate(ILevelContext context)
        {
            // This method is used for spawning more enemies, or checking if the phase is completed

            // Check to see if any enemies exist 
            if (!ComponentBase.ComponentExists<SimpleEnemyBehavior>())
            {
                // todo: prevent from calling complete phase directly
                context.FlagAsComplete();            
            }
        }

        public void PhaseComplete(ILevelContext context)
        {
            GameObject.Destroy(asteroidSpawner);
            Debug.Log("Phase 1 completed, you killed the guy!");
            context.BeginPhase(new TestLevelPhase2());
        }

    }
}
