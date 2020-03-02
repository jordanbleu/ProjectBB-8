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

            #region Dashing Simple Implementation
            //Very simple implementation of dashing, not final product
            //This version just teleports the shit a little to the left/right
            //Final version should include actual movement

            float dashDistance = .5f; //TODO: make this a constant somewhere
            if (InputManager.IsKeyPressed(InputConstants.K_DASH_LEFT))
            {
                rigidBody.position += Vector2.left * dashDistance;
            }
            if (InputManager.IsKeyPressed(InputConstants.K_DASH_RIGHT))
            {
                rigidBody.position += Vector2.right * dashDistance;
            }
            #endregion

            if (InputManager.IsKeyPressed(InputConstants.K_ATTACK_PRIMARY)) {
                GameObject bullet = InstantiatePrefab(bulletPrefab);
                bullet.transform.position = transform.position.Copy();
                // todo: Add InstantiatePrefab method to ComponentBase which ooes this for us
                bullet.transform.position = transform.position;
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
