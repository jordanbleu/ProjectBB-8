using Assets.Source.Components.Base;
using Assets.Source.Components.Spawner;
using Assets.Source.Components.Timer;
using Assets.Source.Constants;
using Assets.Source.Director.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Director.LevelPhases.Demo
{
    public class DP_200_Asteroids : ILevelPhase
    {
        bool timerReached = false;

        private GameObject asteroidSpawner;
        private GameObject intervalTimerObject;


        public void PhaseBegin(ILevelContext context)
        {
            GameObject asteroidPrefab = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.Projectiles.Asteroid}");

            // Create an asteroid spawner which will generate a random asteroid every half second
            asteroidSpawner = ComponentBase.InstantiateInLevel("AsteroidSpawner", Vector3.zero, typeof(AutoSpawnerComponent));
            ComponentBase.GetRequiredComponent<AutoSpawnerComponent>(asteroidSpawner).Initialize(500, asteroidPrefab);


            // A formation will spawn after a while 
            intervalTimerObject = new GameObject("FormationSpawnTimer");

            intervalTimerObject.AddComponent<IntervalTimerComponent>();
            IntervalTimerComponent timer = ComponentBase.GetRequiredComponent<IntervalTimerComponent>(intervalTimerObject);
            timer.SetInterval(15000); // 15 seconds
            timer.OnIntervalReached.AddListener(TimerDone);
            timer.SelfDestruct = true;
            timer.IsActive = true;

        }

        private void TimerDone()
        {
            timerReached = true;
        }

        public void PhaseComplete(ILevelContext context)
        {
            GameObject.Destroy(asteroidSpawner);
        }

        public void PhaseUpdate(ILevelContext context)
        {
            if (timerReached)
            {
                Debug.Log("done");
                context.FlagAsComplete();
            }
        }
    }
}
