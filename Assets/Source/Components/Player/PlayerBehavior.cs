using Assets.Source.Components.Actor;
using Assets.Source.Components.Camera;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.UI;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerBehavior : ProjectileComponentBase
    {
        private readonly float MOVE_SPEED = 2f;
        private readonly float STABILIZATION_RATE = 0.01f;
        private readonly float DASH_DISTANCE = 1.5f; //TODO: make this a constant somewhere
        private readonly float DASH_COOLDOWN = 2000.0f; //milliseconds

        // Components
        private Rigidbody2D rigidBody;
        private ActorBehavior actorBehavior;
        private Animator animator;
        private ActorDash actorDash;

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
        private bool isInvulnerable = false;

        protected override int BaseDamage => 100;

        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            animator = GetRequiredComponent<Animator>();

            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.Projectiles.PlayerBullet}");
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");

            cameraObject = GetRequiredObject("PlayerVCam");

            cameraEffector = GetRequiredComponent<CameraEffectComponent>(cameraObject);
            menuSelector = GetRequiredComponent<CanvasMenuSelectorComponent>(FindOrCreateCanvas());

            actorDash = gameObject.AddComponent<ActorDash>();
            actorDash.CooldownTime = DASH_COOLDOWN;
            actorDash.DashDistance = DASH_DISTANCE;

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
            isInvulnerable = actorDash.IsDashing;
            // This returns a value between -1 and 1, which determines how much the player is moving the analog stick
            // in either direction.  For keyboard it just returns EITHER -1 or 1
            float horizontalMoveDelta = (InputManager.GetAxisValue(InputConstants.K_MOVE_RIGHT) * MOVE_SPEED) -
                (InputManager.GetAxisValue(InputConstants.K_MOVE_LEFT) * MOVE_SPEED);

            float verticalMoveDelta = (InputManager.GetAxisValue(InputConstants.K_MOVE_UP) * MOVE_SPEED) -
                (InputManager.GetAxisValue(InputConstants.K_MOVE_DOWN) * MOVE_SPEED);

            animator.SetInteger("horizontal_move", Mathf.RoundToInt(horizontalMoveDelta));

            float totalHorizontalVelocity = horizontalMoveDelta + externalVelocity.x;
            float totalVerticalVelocity = verticalMoveDelta + externalVelocity.y;

            rigidBody.velocity = rigidBody.velocity.Copy(totalHorizontalVelocity, totalVerticalVelocity);

            #region Dashing
            if (actorDash.IsDashing)
            {
                externalVelocity = actorDash.Dash(externalVelocity);
            }
            else
            {
                if (InputManager.IsKeyPressed(InputConstants.K_DASH_RIGHT))
                {
                    if (!actorDash.TrySetupDashRight())
                    {
                        //make some noise or animation to help signify the player can't dash yet?
                    }
                }
                else if (InputManager.IsKeyPressed(InputConstants.K_DASH_LEFT))
                {
                    if (!actorDash.TrySetupDashLeft())
                    {
                        //make some noise or animation to help signify the player can't dash yet?
                    }
                }
            }
            #endregion

            if (InputManager.IsKeyPressed(InputConstants.K_ATTACK_PRIMARY))
            {
                GameObject bullet = InstantiateInLevel(bulletPrefab);
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
                InstantiateInLevel(explosionPrefab, transform.position);
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

        public void ReactToHit(Collision2D collision, int baseDamage, float partDamageMultiplier)
        {
            string collisionName = collision.otherCollider.gameObject.name;
            if (!collisionName.Equals(bulletPrefab.name) && !isInvulnerable)
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
