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

        public override void PerformAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();

            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.PlayerBullet}");
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");

            cameraObject = GetRequiredObject("PlayerVCam");
            cameraEffector = GetRequiredComponent<CameraEffectComponent>(cameraObject);

            base.PerformAwake();
        }

        public override void PerformUpdate()
        {
            UpdatePlayerControls();
            UpdateActorStatus();
            base.PerformUpdate();
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
                InstantiatePrefab(explosionPrefab, transform.position);
                GameObject canvas = FindOrCreateCanvas();
                
                // How to show a UI menu
                CanvasMenuSelectorComponent ui = GetRequiredComponent<CanvasMenuSelectorComponent>(canvas);
                ui.ShowMenu<GameOverMenuComponent>();
                
                Destroy(gameObject);
            }        
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            if (!collision.otherCollider.gameObject.name.Equals(bulletPrefab.name))
            {
                actorBehavior.Health -= baseDamage;
                cameraEffector.TriggerImpulse1();
            }
        }
    }
}
