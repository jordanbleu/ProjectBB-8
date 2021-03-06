﻿using Assets.Source.Components.AI;
using Assets.Source.Components.Base;
using Assets.Source.Constants;
using Assets.Source.Director.Interfaces;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Director.Testing.TestLevel
{
    class TestLevelPhase2 : ILevelPhase
    {
        public void PhaseBegin(ILevelContext context)
        {
            GameObject enemy = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Actors/Enemies{GameObjects.Actors.Kamikaze}");
            
            GameObject enemy1 = ComponentBase.InstantiateInLevel(enemy);
            enemy1.transform.position = new Vector3(1, 2f, 0);

            GameObject enemy2 = ComponentBase.InstantiateInLevel(enemy);
            enemy2.transform.position = new Vector3(-1, 3f, 0);

            for (int i = 0; i < 5; i++) {
                var inst = ComponentBase.InstantiateInLevel(enemy);
                inst.transform.position = new Vector3(i * 0.5f, i * 0.5f);
            }

        }
        
        public void PhaseUpdate(ILevelContext context)
        {
            if (!Object.FindObjectsOfType<SuicideEnemyAIBehavior>().Any()) 
            {
                context.FlagAsComplete();
            }
        }


        public void PhaseComplete(ILevelContext context)
        {
            Debug.Log("Phase 2 complteed.  You are a god among mortals.");
            context.BeginPhase(new TestLevelPhase3());
        }

    }
}
