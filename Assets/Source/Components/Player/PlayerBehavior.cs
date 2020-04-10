using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Components.Camera;
using Assets.Source.Components.Director.Base;
using Assets.Source.Components.UI;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerBehavior : ComponentBase
    {
        private readonly float MOVE_SPEED = 2f;
        private readonly float STABILIZATION_RATE = 0.01f;

        // Components
        private Rigidbody2D rigidBody;
        private ActorBehavior actorBehavior;
        private Animator animator;
        private AudioSource audioSource;

        // Prefab references
        private GameObject bulletPrefab;
        private GameObject explosionPrefab;

        // Hierarchy References
        private GameObject cameraObject;
        
        // Other object's Components
        private CameraEffectComponent cameraEffector;
        private CanvasMenuSelectorComponent menuSelector;
        private DirectorComponent levelDirector;

        // Audio Clips
        private AudioClip blasterSound;

        // Physics
        private Vector2 externalVelocity;


        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            animator = GetRequiredComponent<Animator>();
            audioSource = GetRequiredComponent<AudioSource>();

            bulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.PlayerBullet}");
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");

            cameraObject = GetRequiredObject("PlayerVCam");

            cameraEffector = GetRequiredComponent<CameraEffectComponent>(cameraObject);
            menuSelector = GetRequiredComponent<CanvasMenuSelectorComponent>(FindOrCreateCanvas());
            levelDirector = GetRequiredComponent<DirectorComponent>(FindLevelObject());

            blasterSound = GetRequiredResource<AudioClip>($"{ResourcePaths.SoundFXFolder}/Player/playerBlaster");

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

            #region Dashing Simple Implementation
            //Very simple implementation of dashing, not final product
            //This version just teleports the shit a little to the left/right
            //Final version should include actual movement

            // todo: This should not set position directly, it should use velocity.  This curently breaks the player barrier

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

            if (InputManager.IsKeyPressed(InputConstants.K_ATTACK_PRIMARY))
            {
                FireBlaster();
            }

            if (InputManager.IsKeyPressed(InputConstants.K_PAUSE))
            {
                // Opens the pause menu
                menuSelector.ShowMenu<PauseMenuComponent>();
            }

            if (InputManager.IsKeyPressed(InputConstants.K_MENU_LEFT)) 
            {
                Warning("booty");
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
            }
            else 
            {
                Warning("No Ammo");
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
