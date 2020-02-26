using Assets.Source.Components.Exception;
using Assets.Source.Components.SystemObject;
using Assets.Source.Constants;
using Assets.Source.Input;
using UnityEngine;

namespace Assets.Source.Components.Base
{
    public abstract class ComponentBase : MonoBehaviour
    {
        /// <summary>
        /// Provides access to the global input manager
        /// </summary>
        protected InputManager InputManager { get; private set; }

        private GameObject systemObject;

        #region Resource Helper Methods
        /// <summary>
        /// Loads a component from the current gameObject, throwing an exception if one isn't found.
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        protected T GetRequiredComponent<T>() where T : Component
        {
            return GetRequiredComponent<T>(gameObject);
        }

        /// <summary>
        /// Loads a component off of another game object, throwing an exception if one isn't found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="otherObject"></param>
        protected T GetRequiredComponent<T>(GameObject otherObject)
        {
            T component;
            try
            {
                component = otherObject.GetComponent<T>();
            }
            catch (MissingComponentException)
            {
                throw new MissingRequiredComponentException(otherObject, typeof(T));
            }
            return component;
        }

        /// <summary>
        /// Searches child game objects of the current game object for 
        /// the requested component.  Returns the first instance found.
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        protected T GetRequiredComponentInChildren<T>() where T : Component
        {
            return GetRequiredComponentInChildren<T>(gameObject);
        }

        /// <summary>
        /// Searches child game objects of the specified game object for 
        /// the requested component.  Returns the first instance found.
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        protected T GetRequiredComponentInChildren<T>(GameObject otherObject)
        {
            T component;

            try
            {
                component = otherObject.GetComponentInChildren<T>(true);
            }
            catch (MissingComponentException)
            {
                throw new MissingRequiredComponentException(otherObject, typeof(T));
            }
            return component;
        }

        /// <summary>
        /// Loads a resource from unity's resources directory, or throws an exception if it is not found
        /// </summary>
        protected T GetRequiredResource<T>(string path) where T : UnityEngine.Object
        {
            T resource = Resources.Load<T>(path) as T
                ?? throw new MissingResourceException(path);

            return resource;
        }

        /// <summary>
        /// Finds an object located on the base of the hierarchy, or throws an exception if not found
        /// </summary>
        /// <param name="name">Name of the object to find</param>
        protected GameObject GetRequiredObject(string name)
        {
            GameObject obj = GameObject.Find(name)
                ?? throw new MissingRequiredObjectException(name);
            return obj;
        }

        /// <summary>
        /// Gets a required child game object off the current game object, or throws an exception if not found
        /// </summary>
        protected GameObject GetRequiredChild(string name)
        {
            return GetRequiredChild(gameObject, name);
        }

        /// <summary>
        /// Gets a required child game object off the specified game object, or throws an exception if not found
        /// </summary>
        protected GameObject GetRequiredChild(GameObject otherObject, string name)
        {
            Transform tranformObject = otherObject.transform.Find(name)
                ?? throw new MissingRequiredChildException(otherObject, name);

            return tranformObject.gameObject;
        }
        #endregion

        #region overrides
        private void Awake()
        {
            InputManager = GetRequiredComponent<SystemObjectBehavior>(FindOrCreateSystemObject()).InputManager;
            Construct(); 
        }
        private void Start() { Create(); }
        private void Update() { Step(); }
        private void OnDestroy() { Destroy(); }
        private void OnEnable() { Activate(); }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's Awake Method. 
        /// <para>This should be used for things such as setting references to components, etc</para>
        /// </summary>
        public virtual void Construct() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's Start method
        /// <para>Used for setting up an object in the scene after all items are built and ready.</para>
        /// </summary>
        public virtual void Create() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's Update method
        /// <para>This is used for updates that happen every frame</para>
        /// </summary>
        public virtual void Step() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's OnDestroy() method
        /// <para>Used for freeing up resources, etc</para>
        /// </summary>
        public virtual void Destroy() { }

        /// <summary>
        /// Override this method to add functionality to monobehaviour's OnEnable() method
        /// <para>This code is executed when an object is toggled to "active"</para>
        /// </summary>
        public virtual void Activate() { }
        #endregion

        #region SystemObject
        /// <summary>
        /// Call to get or create the system object, which houses things like the InputHandlers, etc
        /// </summary>
        protected GameObject FindOrCreateSystemObject()
        {
            // try to find the system object...
            if (systemObject == null)
            {
                systemObject = GameObject.Find(GameObjects.SystemObject);
            }

            // if it doesn't exist, then create it
            if (systemObject == null)
            {
                GameObject systemPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/System/{GameObjects.SystemObject}");
                systemObject = Instantiate(systemPrefab);
                systemObject.name = GameObjects.SystemObject;
            }
            return systemObject;
        }
        #endregion
    }
}
