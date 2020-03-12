using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;

/// <summary>
/// All this does is randomly generate asteroids.  Kinda cool though!
/// </summary>
public class AsteroidLevelBehavior : ComponentBase
{
    private GameObject asteroidPrefab;
    public override void PerformAwake()
    {
        asteroidPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.Asteroid}");
        base.PerformAwake();
    }

    public void SpawnAsteroid()
    {
        float x = Random.Range(-1.5f, 1.5f);
        float y = Random.Range(1.5f, 2.5f);
        // todo:  Do we want to spawn level items under a "Level" object?  This keeps the hierarchy cleaner for sure
        InstantiatePrefab(asteroidPrefab, new Vector3(x, y, transform.position.z), transform);
    }
}
