using Assets.Source.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Spawner
{
    /// <summary>
    /// Used to spawn prefabs intermittently in random positions.  
    /// </summary>
    public class AutoSpawnerComponent : DelayedUpdateBaseComponent
    {
        [SerializeField]
        private GameObject spawnObject;

        [SerializeField]
        private int delay;

        [SerializeField]
        private Vector2 minPosition = new Vector2(-1.5f, 3f);

        [SerializeField]
        private Vector2 maxPosition = new Vector2(1.5f, 4f);

        public void Initialize(int spawnDelay, GameObject objectToSpawn, Vector2? minPosition = null, Vector2? maxPosition = null) 
        {
            spawnObject = objectToSpawn;
            delay = spawnDelay;

            if (minPosition != null)
            {
                this.minPosition = minPosition.Value;
            }

            if (maxPosition != null)
            {
                this.maxPosition = maxPosition.Value;
            }

        }

        public override void ComponentUpdate()
        {
            FrameTimeDelay = delay;
            base.ComponentUpdate();
        }

        public override void DelayedUpdate()
        {
            if (spawnObject != null) 
            {
                float x = UnityEngine.Random.Range(minPosition.x, maxPosition.x);
                float y = UnityEngine.Random.Range(maxPosition.y, maxPosition.y);
                InstantiateLevelPrefab(spawnObject, new Vector3(x, y, 0));
            }
        }

    }
}
