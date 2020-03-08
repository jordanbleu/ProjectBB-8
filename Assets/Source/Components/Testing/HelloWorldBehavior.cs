using Assets.Source.Components.Base;
using Assets.Source.Configuration;
using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Behavior.Testing
{
    // To see this in action open sample scene and click on "GameObject"
    // Then run the game

    public class HelloWorldBehavior : ComponentBase
    {
        // This is how we make stuff show up in the inspector.  You can also make it public 
        // but i hate that.  This will allow us to type stuff in right from the inspector window
        [SerializeField]
        private string testStringus;

        [SerializeField]
        // Can add headers like so:
        [Header("This is some text")]
        private string myName;
        private SpriteRenderer exampleComponent;

        public override void Construct()
        {
            // Construct should be used to set up references to components, game objects, etc
            // Unity Behaviors cannot have actual constructors or it'll break things
            
            // This automatically checks for nulls, its awesome
            exampleComponent = GetRequiredComponent<SpriteRenderer>();

            // Also should validatation type things here
            if (string.IsNullOrWhiteSpace(myName))
            {
                Debug.LogError("You were supposed to set a name in the inspector, nice try.");
            }

            Debug.Log($"Hello my dudes, we hit the construct method. The attached" +
                $" sprite renderer's color value is: {exampleComponent.color.ToString()}");

            base.Construct();
        }

        public override void Create()
        {
            // Create can be used to set stuff up after we've gotten all our references.
            // It gets called after the Construct method.

            // positioning and stuff would happen here
            transform.position = Vector3.zero;

            // here's an example of loading from the configuration repo
            Debug.Log($"The configured language code is: '{ConfigurationRepository.SystemConfiguration.Language}'");

            base.Create();
        }

        public override void Step()
        {
            // Step is called for every frame

            // These values get updated in the inspector in real time.  
            // If you want to see some dope ass shit, change the "My Name" value in the inspector
            // while the game is running.  This is why unity is amazing.
            testStringus = $"Hello, {myName}.  Delta Tyme: {Time.deltaTime}. ";

            // *** Try pressing these keys *** 
            if (InputManager.IsKeyPressed(InputConstants.K_MENU_ENTER)) {
                Debug.Log("you pressed the menu enter key");
            }

            if (InputManager.IsKeyReleased(InputConstants.K_MENU_ENTER)) {
                Debug.Log("You let go of the enter key.");
            }


            if (InputManager.IsKeyHeld(InputConstants.K_MENU_DOWN)) {
                Debug.Log("You are holding down the K_MENU_DOWN key.  Prepare to have the console spammed.");
            }

            base.Step();
        }

        

        // Destroy gets called whenever OnDestroy() is called -or- whenever the game closes
        // This isn't often used though. 
        public override void Destroy()
        {
            Debug.Log("Destroy was called");

            // Clean up resources, etc

            base.Destroy();
        }


        // To see this one, you can run the unity game and toggle the checkbox 
        // for the game object in the inspector
        public override void Activate()
        {
            Debug.Log("What is up guys, we have hit the activate method");
            // We should make it a habbit to leave in the call to the 
            // base methods even though they don't currently do anything
            base.Activate();
        }

        


    }
}
