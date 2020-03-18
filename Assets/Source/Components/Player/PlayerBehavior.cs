using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Components.Camera;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Components.UI;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerBehavior : ComponentBase, IProjectileReactor
    {
        private readonly float MOVE_SPEED = 2f;
        private readonly float STABILIZATION_RATE = 0.01f;

        // Components
        private Rigidbody2D rigidBody;
        private ActorBehavior actorBehavior;

        // Prefab references
        private GameObject bulletPrefab;
        private GameObject explosionPrefab;

        // Hierarchy References
        private GameObject cameraObject;

        // Other object's Components
        private CameraEffectComponent cameraEffector;
        private CanvasMenuSelectorComponent menuSelector;

        // Physics
        private Vector2 externalVelocity;


        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            
            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.PlayerBullet}");
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");

            cameraObject = GetRequiredObject("PlayerVCam");
            
            cameraEffector = GetRequiredComponent<CameraEffectComponent>(cameraObject);
            menuSelector = GetRequiredComponent<CanvasMenuSelectorComponent>(FindOrCreateCanvas());

            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            UpdatePlayerControls();
            UpdateActorStatus();
            UpdateExternalVelocity();
            base.ComponentUpdate();
        }

        private void UpdatePlayerControls()
        {
            // This returns a value between -1 and 1, which determines how much the player is moving the analog stick
            // in either direction.  For keyboard it just returns EITHER -1 or 1
            float horizontalMoveDelta = (InputManager.GetAxisValue(InputConstants.K_MOVE_RIGHT) * MOVE_SPEED) -
                (InputManager.GetAxisValue(InputConstants.K_MOVE_LEFT) * MOVE_SPEED);

            float verticalMoveDelta = (InputManager.GetAxisValue(InputConstants.K_MOVE_UP) * MOVE_SPEED) -
                (InputManager.GetAxisValue(InputConstants.K_MOVE_DOWN) * MOVE_SPEED);

            rigidBody.velocity = rigidBody.velocity.Copy(horizontalMoveDelta, verticalMoveDelta) + externalVelocity;

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

            if (InputManager.IsKeyPressed(InputConstants.K_PAUSE)) 
            {
                // Opens the pause menu
                menuSelector.ShowMenu<PauseMenuComponent>();
            }
        }

        private void UpdateActorStatus()
        {
            // todo: this is just placeholder stuff
            if (actorBehavior.Health <= 0) 
            {
                InstantiatePrefab(explosionPrefab, transform.position);
                menuSelector.ShowMenu<GameOverMenuComponent>();                
                Destroy(gameObject);
            }        
        }

        private void UpdateExternalVelocity()
        {
            // Normalize
            if (externalVelocity.x > 0)
            {
                externalVelocity = externalVelocity.Copy(x: externalVelocity.x - STABILIZATION_RATE);
            }
            else if (externalVelocity.x < 0)
            {
                externalVelocity = externalVelocity.Copy(x: externalVelocity.x + STABILIZATION_RATE);
            }

            if (externalVelocity.y > 0)
            {
                externalVelocity = externalVelocity.Copy(y: externalVelocity.y - STABILIZATION_RATE);
            }
            else if (externalVelocity.y < 0)
            {
                externalVelocity = externalVelocity.Copy(y: externalVelocity.y + STABILIZATION_RATE);
            }

            // Prevents overshoot
            if (externalVelocity.x.IsWithin(STABILIZATION_RATE, 0f)) { externalVelocity = externalVelocity.Copy(x: 0f); }
            if (externalVelocity.y.IsWithin(STABILIZATION_RATE, 0f)) { externalVelocity = externalVelocity.Copy(y: 0f); }

        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            if (!collision.otherCollider.gameObject.name.Equals(bulletPrefab.name))
            {
                actorBehavior.Health -= baseDamage;

                // todo: Once we have enemies, we should add a weight value to them which will affect the hard coded values below

                // Hit from the left side
                if (collision.otherCollider.transform.position.x < transform.position.x)
                {
                    externalVelocity = externalVelocity.Copy(x: externalVelocity.x + 1f);
                    cameraEffector.Trigger_Impact_Left();
                }
                // Hit from the right side
                else 
                {
                    externalVelocity = externalVelocity.Copy(x: externalVelocity.x - 1f);
                    cameraEffector.Trigger_Impact_Right();
                }

                // Hit from the ass
                if (collision.otherCollider.transform.position.y < transform.position.y)
                {
                    externalVelocity = externalVelocity.Copy(y: externalVelocity.x + 1f);
                }
                // Hit from the right side
                else
                {
                    externalVelocity = externalVelocity.Copy(y: externalVelocity.y - 1f);
                }


            }
        }
    }
}
