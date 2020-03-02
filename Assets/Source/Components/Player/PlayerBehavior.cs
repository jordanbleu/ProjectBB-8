using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerBehavior : ComponentBase
    {
        private readonly float MOVE_SPEED = 2f;

        // Components
        private Rigidbody2D rigidBody;
        private ActorBehavior actorBehavior;
        private SpriteRenderer spriteRenderer;

        // Prefab references
        private GameObject bulletPrefab;
        private GameObject explosionObject;

        public override void Construct()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            spriteRenderer = GetRequiredComponent<SpriteRenderer>();

            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.PlayerBullet}");

            explosionObject = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");
            base.Construct();
        }

        public override void Step()
        {
            UpdatePlayerControls();
            UpdateActorStatus();
            base.Step();
        }

        private void UpdatePlayerControls()
        {
            // This returns a value between -1 and 1, which determines how much the player is moving the analog stick
            // in either direction.  For keyboard it just returns EITHER -1 or 1
            float horizontalMoveDelta = (InputManager.GetAxisValue(InputConstants.K_MOVE_RIGHT) * MOVE_SPEED) -
                (InputManager.GetAxisValue(InputConstants.K_MOVE_LEFT) * MOVE_SPEED);

            float verticalMoveDelta = (InputManager.GetAxisValue(InputConstants.K_MOVE_UP) * MOVE_SPEED) -
                (InputManager.GetAxisValue(InputConstants.K_MOVE_DOWN) * MOVE_SPEED);

            rigidBody.velocity = rigidBody.velocity.Copy(horizontalMoveDelta, verticalMoveDelta);


            if (InputManager.IsKeyPressed(InputConstants.K_SHOOT))
            {
                GameObject bullet = InstantiatePrefab(bulletPrefab, transform.position);
            }
        }

        private void UpdateActorStatus()
        {
            // todo: this is just placeholder stuff
            if (actorBehavior.Health <= 0) 
            {
                InstantiatePrefab(explosionObject, transform.position);
                Destroy(gameObject);
            }        
        }

    }
}
