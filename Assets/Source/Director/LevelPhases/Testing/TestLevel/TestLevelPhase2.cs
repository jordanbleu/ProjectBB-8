using Assets.Source.Components.Base;
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
            GameObject enemy = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Actors/{GameObjects.Actors.KamikazeEnemy}");
            
            GameObject enemy1 = ComponentBase.InstantiateInLevel(enemy);
            enemy1.transform.position = new Vector3(1, 2f, 0);

            GameObject enemy2 = ComponentBase.InstantiateInLevel(enemy);
            enemy2.transform.position = new Vector3(-1, 3f, 0);
        }
        
        public void PhaseUpdate(ILevelContext context)
        {
            if (!UnityEngine.Object.FindObjectsOfType<KamikazeEnemyBehavior>().Any()) 
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
