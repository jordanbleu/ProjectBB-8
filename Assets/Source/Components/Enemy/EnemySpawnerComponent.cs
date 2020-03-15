using UnityEngine;
using Assets.Source.Components.Base;
using Assets.Source.Constants;

namespace Assets.Source.Components.Enemy
{
    public class EnemySpawnerComponent : ComponentBase
    {
        //this doesn't do anything for now but it will once additional functionality is added.
        public enum SpawnTypes
        {
            Sequential,
            Instant
        }

        [SerializeField]
        private SpawnTypes spawnType = SpawnTypes.Sequential;

        [SerializeField]
        private GameObject enemyPrafabToSpawn;

        [SerializeField]
        private int spawnCount = 2;

        [SerializeField]
        private float spawnInterval = 4.0f;

        private bool finishedSpawning = false;
        private float timeSinceLastSpawn = 0f;
        private GameObject enemy;
        private GameObject player;
        private float distanceToPlayer = 0.0f;

        public override void Construct()
        {
            player = GetRequiredObject(GameObjects.Player);
            distanceToPlayer = transform.position.y - player.transform.position.y;

            if (enemyPrafabToSpawn == null)
            {
                enemyPrafabToSpawn = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Actors/{GameObjects.Enemy}");
            }

            base.Construct();
        }

        public override void Step()
        {
            if(enemy == null)
            {
                timeSinceLastSpawn += Time.deltaTime;
                if (finishedSpawning)
                {
                    Destroy(gameObject);
                }
            }

            if(timeSinceLastSpawn >= spawnInterval)
            {
                timeSinceLastSpawn = 0f;
                SpawnEnemySequential();
            }

            base.Step();
        }

        private void SpawnEnemySequential()
        {
            AdjustSpawnPosition();

            enemy = InstantiatePrefab(enemyPrafabToSpawn, transform);

            if (--spawnCount == 0)
            {
                finishedSpawning = !finishedSpawning;
            }
        }

        private void SpawnEnemyInstant()
        {
            //todo: Spawn all enemies at same time and space them out
        }


        private void AdjustSpawnPosition()
        {
            if(player != null)
            {
                Vector3 playerPosition = player.transform.position;
                Vector3 adjustedSpawnPosition = new Vector3(playerPosition.x, playerPosition.y + distanceToPlayer, transform.position.z);
                transform.position = adjustedSpawnPosition;
            }
        }
    }
}