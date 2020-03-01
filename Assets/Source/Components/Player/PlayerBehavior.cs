﻿using Assets.Source.Components.Base;
using Assets.Source.Components.Projectile;
using Assets.Source.Components.Reactor.Interfaces;
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

            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.PlayerBullet}");

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
            base.Step();
        }

        public void ReactToProjectileHit(Collision2D collision)
        {
            // If something hits the player that is not the player bullet, we got hit
            if (!collision.otherCollider.gameObject.TryGetComponent<PlayerBulletBehavior>(out _)) 
            { 
                Debug.Log($"Oof you got hit by {collision.gameObject.name} fam.");
            }
        }

    }
}
