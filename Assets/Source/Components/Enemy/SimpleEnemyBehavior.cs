using Assets.Source.Components.Enemy.Base;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    public class SimpleEnemyBehavior : BaseEnemyBehavior
    {
        public override void Step()
        {
            UpdateActorBehavior();
            UpdateActorStatus();
            base.Step();
        }

        public override void UpdateActorBehavior()
        {
            Vector3 newEnemyPosition = UpdatedEnemyPosition(transform);

            rigidBody.MovePosition(newEnemyPosition);

            if (ShouldFire(transform))
            {
                GameObject bullet = InstantiatePrefab(enemyBulletPrefab);
                bullet.transform.position = transform.position.Copy();
            }
        }
    }
}