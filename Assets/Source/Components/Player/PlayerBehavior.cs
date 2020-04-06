using Assets.Source.Components.Actor;
using Assets.Source.Components.Camera;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Director.Base;
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
        private readonly float dashDistance = 1.5f; //TODO: make this a constant somewhere

        // Components
        private Rigidbody2D rigidBody;
        private ActorBehavior actorBehavior;
        private Animator animator;

        // Prefab references
        private GameObject bulletPrefab;
        private GameObject explosionPrefab;

        // Hierarchy References
        private GameObject cameraObject;
        

        // Other object's Components
        private CameraEffectComponent cameraEffector;
        private CanvasMenuSelectorComponent menuSelector;
        private DirectorComponent levelDirector;


        // Physics
        private Vector2 externalVelocity;
        private bool isDashingRight = false;
        private bool isDashingLeft = false;
        private float dashLocation;
        private float dashDelay = 2.0f;
        private float dashDelayRemaining = 0.0f;


        protected override int BaseDamage => 100;

        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            animator = GetRequiredComponent<Animator>();

            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.PlayerBullet}");
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");

            cameraObject = GetRequiredObject("PlayerVCam");

            cameraEffector = GetRequiredComponent<CameraEffectComponent>(cameraObject);
            menuSelector = GetRequiredComponent<CanvasMenuSelectorComponent>(FindOrCreateCanvas());
            levelDirector = GetRequiredComponent<DirectorComponent>(FindLevelObject());

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

            animator.SetInteger("horizontal_move", Mathf.RoundToInt(horizontalMoveDelta));

            float totalHorizontalVelocity = horizontalMoveDelta + externalVelocity.x;
            float totalVerticalVelocity = verticalMoveDelta + externalVelocity.y;

            rigidBody.velocity = rigidBody.velocity.Copy(totalHorizontalVelocity, totalVerticalVelocity);

            #region Dashing
            if(dashDelayRemaining <= 0.0f)
            {
                if ((InputManager.IsKeyPressed(InputConstants.K_DASH_RIGHT) || isDashingRight) && !isDashingLeft)
                {
                    Debug.Log("player is dashing right");
                    if (!isDashingRight)
                    {
                        dashLocation = transform.position.x + dashDistance;
                        isDashingRight = !isDashingRight;
                        dashDelayRemaining = dashDelay;
                    }
                    DashRight();
                }
                else if ((InputManager.IsKeyPressed(InputConstants.K_DASH_LEFT) || isDashingLeft) && !isDashingRight)
                {
                    Debug.Log("player is dashing left");
                    if (!isDashingLeft)
                    {
                        dashLocation = transform.position.x - dashDistance;
                        isDashingLeft = !isDashingLeft;
                        dashDelayRemaining = dashDelay;
                    }
                    DashLeft();
                }
            }
            else
            {
                dashDelayRemaining -= Time.deltaTime;
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

        private void DashRight()
        {
            externalVelocity = externalVelocity.Copy(x: dashDistance + externalVelocity.x);
            Debug.Log("DashVelocity: " + externalVelocity);

            if(transform.position.x > dashLocation)
            {
                Debug.Log("PLayer stopped dashing");
                isDashingRight = false;
                externalVelocity.x = 0f;
            }
        }

        private void DashLeft()
        {
            externalVelocity = externalVelocity.Copy(x: -dashDistance - externalVelocity.x);
            Debug.Log("DashVelocity: " + externalVelocity);

            if (transform.position.x < dashLocation)
            {
                Debug.Log("PLayer stopped dashing");
                isDashingRight = false;
                externalVelocity.x = 0f;
            }
        }
    }
}
