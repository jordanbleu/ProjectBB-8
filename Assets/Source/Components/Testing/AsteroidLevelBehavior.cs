using Assets.Source.Components.Base;
using Assets.Source.Components.Director;
using Assets.Source.Constants;
using UnityEngine;

/// <summary>
/// All this does is randomly generate asteroids.  Kinda cool though!
/// </summary>
public class AsteroidLevelBehavior : ComponentBase
{
    private GameObject asteroidPrefab;
    public override void ComponentAwake()
    {
        asteroidPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.Asteroid}");
        base.ComponentAwake();
    }

    public void SpawnAsteroid()
    {
        float x = Random.Range(-1.5f, 1.5f);
        float y = Random.Range(3f, 4f);
        // todo:  Do we want to spawn level items under a "Level" object?  This keeps the hierarchy cleaner for sure
        InstantiateRootPrefab(asteroidPrefab, new Vector3(x, y, transform.position.z), transform);
    }
}
