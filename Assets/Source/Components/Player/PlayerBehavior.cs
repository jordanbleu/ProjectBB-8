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

        // Prefab references
        private GameObject bulletPrefab;

        public override void Construct()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();

            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/PlayerBullet");

            base.Construct();
        }


        public override void Step()
        {
            // This returns a value between -1 and 1, which determines how much the player is moving the analog stick
            // in either direction.  For keyboard it just returns EITHER -1 or 1
            float horizontalMoveDelta = (InputManager.GetAxisValue(InputConstants.K_MOVE_RIGHT) * MOVE_SPEED) -
                (InputManager.GetAxisValue(InputConstants.K_MOVE_LEFT) * MOVE_SPEED);

            float verticalMoveDelta = (InputManager.GetAxisValue(InputConstants.K_MOVE_UP) * MOVE_SPEED) -
                (InputManager.GetAxisValue(InputConstants.K_MOVE_DOWN) * MOVE_SPEED);

            // todo: vertical movement
            rigidBody.velocity = rigidBody.velocity.Copy(horizontalMoveDelta, verticalMoveDelta);

            float dashDistance = .5f; //TODO: make this a constant somewhere
            if (InputManager.IsKeyPressed(InputConstants.K_DASH_LEFT))
            {
                rigidBody.position += Vector2.left * dashDistance;
            }
            if (InputManager.IsKeyPressed(InputConstants.K_DASH_RIGHT))
            {
                rigidBody.position += Vector2.right * dashDistance;
            }

            if (InputManager.IsKeyPressed(InputConstants.K_ATTACK_PRIMARY)) {
                GameObject bullet = Instantiate(bulletPrefab);
                // todo: Add InstantiatePrefab method to ComponentBase which ooes this for us
                bullet.transform.position = transform.position;
            }
                       

            base.Step();
        }




    }
}
