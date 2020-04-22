using Assets.Source.Components.Actor;
using Assets.Source.Components.Camera;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.UI;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using Assets.Source.Input.Constants;
using UnityEngine;
using Assets.Source.Components.Timer;

namespace Assets.Source.Components.Player
{
    public class PlayerBehavior : ProjectileComponentBase
    {
        private readonly float MOVE_SPEED = 2f;
        private readonly float STABILIZATION_RATE = 0.01f;
        private readonly float DASH_DISTANCE = 1.5f; //TODO: make this a constant somewhere
        private readonly float DASH_COOLDOWN = 2000.0f; //milliseconds
        private readonly float SHOOT_COOLDOWN = 350.0f;

        // Components
        private Rigidbody2D rigidBody;
        private ActorBehavior actorBehavior;
        private Animator animator;
        private ActorDashBehavior actorDashBehavior;

        // Prefab references
        private GameObject bulletPrefab;
        private GameObject explosionPrefab;

        // Hierarchy References
        private GameObject cameraObject;
        
        // Other object's Components
        private CameraEffectComponent cameraEffector;
        private CanvasMenuSelectorComponent menuSelector;

        // Audio
        private AudioClip blasterSound;
        private AudioSource audioSource;

        // Physics
        private Vector2 externalVelocity;
        private bool isInvulnerable = false;

        // Timers
        public IntervalTimerComponent ShootTimer { get; private set; }

        protected override int BaseDamage => 100;

        public override void ComponentAwake()
        {
            SetupTimers();
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            animator = GetRequiredComponent<Animator>();
            audioSource = GetRequiredComponent<AudioSource>();
            actorDashBehavior = GetRequiredComponent<ActorDashBehavior>();

            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.Projectiles.PlayerBullet}");
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/EnemyExplosion");

            cameraObject = GetRequiredObject("PlayerVCam");

            cameraEffector = GetRequiredComponent<CameraEffectComponent>(cameraObject);
            menuSelector = GetRequiredComponent<CanvasMenuSelectorComponent>(FindOrCreateCanvas());

            actorDashBehavior.CooldownTime = DASH_COOLDOWN;
            actorDashBehavior.DashDistance = DASH_DISTANCE;

            blasterSound = GetRequiredResource<AudioClip>($"{ResourcePaths.SoundFXFolder}/Player/playerBlaster");

            base.ComponentAwake();
        }

        private void SetupTimers()
        {
            ShootTimer = gameObject.AddComponent<IntervalTimerComponent>();
            ShootTimer.UpdateInterval(SHOOT_COOLDOWN);
            ShootTimer.IsActive = true;
            ShootTimer.AutoReset = false;
        }

        public override void ComponentUpdate()
        {
            UpdatePlayerControls();
            UpdateActorStatus();
            UpdateExternalVelocity();
            base.ComponentUpdate();
        }

        public void ReactToHit(Collision2D collision, int baseDamage, float partDamageMultiplier)
        {
            string collisionName = collision.otherCollider.gameObject.name;
            if (!collisionName.Equals(bulletPrefab.name) && !isInvulnerable)
            {
                actorBehavior.Health -= baseDamage;

                // Warn player if health is less than 10%
                if (actorBehavior.Health > 0 && ((float)actorBehavior.Health / actorBehavior.MaxHealth) < 0.1f)
                {
                    Warning("Low Health");
                }

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

        private void UpdatePlayerControls()
        {
            isInvulnerable = actorDashBehavior.IsDashing;
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
            if (actorDashBehavior.IsDashing)
            {
                externalVelocity = actorDashBehavior.Dash(externalVelocity);
            }
            else
            {
                if (InputManager.IsKeyPressed(InputConstants.K_DASH_RIGHT))
                {
                    if (!actorDashBehavior.TrySetupDashRight())
                    {
                        //make some noise or animation to help signify the player can't dash yet?
                    }
                }
                else if (InputManager.IsKeyPressed(InputConstants.K_DASH_LEFT))
                {
                    if (!actorDashBehavior.TrySetupDashLeft())
                    {
                        //make some noise or animation to help signify the player can't dash yet?
                    }
                }
            }
            #endregion

            if (InputManager.IsKeyPressed(InputConstants.K_ATTACK_PRIMARY))
            {
                if (!ShootTimer.IsActive)
                {
                    FireBlaster();
                }
            }

            if (InputManager.IsKeyPressed(InputConstants.K_PAUSE))
            {
                // Opens the pause menu
                menuSelector.ShowMenu<PauseMenuComponent>();
            }
        }

        // Player pressed left mouse button and shot a bullet
        private void FireBlaster()
        {
            if (actorBehavior.BlasterAmmo > 0)
            {
                audioSource.PlayOneShot(blasterSound);
                GameObject bullet = InstantiateInLevel(bulletPrefab);
                bullet.transform.position = transform.position;
                actorBehavior.BlasterAmmo--;

                ShootTimer.Reset();
            }
            else 
            {
                Warning("No Ammo");
            }
        }

        private void UpdateActorStatus()
        {
            if(actorBehavior.Health <= 0)
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
    }
}
