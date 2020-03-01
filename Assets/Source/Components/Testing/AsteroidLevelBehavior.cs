using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;

/// <summary>
/// All this does is randomly generate asteroids.  Kinda cool though!
/// </summary>
public class AsteroidLevelBehavior : ComponentBase
{
    private GameObject asteroidPrefab;
    public override void Construct()
    {
        asteroidPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.Asteroid}");
        base.Construct();
    }

    public void SpawnAsteroid()
    {
        float x = UnityEngine.Random.Range(-2.5f, 2.5f);
        float y = UnityEngine.Random.Range(1.5f, 2.5f);
        InstantiatePrefab(asteroidPrefab, null, new Vector2(x, y));
    }
}
