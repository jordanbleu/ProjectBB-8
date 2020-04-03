﻿using Assets.Source.Components.Base;
using Assets.Source.Components.Enemy;
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
            GameObject enemy = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Actors/{GameObjects.ShooterEnemy}");
            
            GameObject enemy1 = ComponentBase.InstantiateLevelPrefab(enemy);
            enemy1.transform.position = new Vector3(0, .5f, 1);

            GameObject enemy2 = ComponentBase.InstantiateLevelPrefab(enemy);
            enemy2.transform.position = new Vector3(0, .5f, 1);
        }
        
        public void PhaseUpdate(ILevelContext context)
        {
            if (!UnityEngine.Object.FindObjectsOfType<SimpleEnemyBehavior>().Any()) 
            {
                context.FlagAsComplete();
            }
        }


        public void PhaseComplete(ILevelContext context)
        {
            Debug.Log("Phase 2 complteed.  You are a god among men.");
            context.BeginPhase(new TestLevelPhase3());
        }

    }
}