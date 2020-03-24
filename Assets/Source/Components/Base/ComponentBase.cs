using Assets.Source.Components.Exception;
using Assets.Source.Components.SystemObject;
using Assets.Source.Components.TextWriter;
using Assets.Source.Constants;
using Assets.Source.Input;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Base
{
    public abstract class ComponentBase : MonoBehaviour
    {
        /// <summary>
        /// Provides access to the global input manager
        /// </summary>
        protected InputManager InputManager { get => GetRequiredComponent<SystemObjectBehavior>(FindOrCreateSystemObject()).InputManager; }

        private GameObject systemObject;
        private GameObject canvasObject;

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
        public static T GetRequiredComponent<T>(GameObject otherObject)
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
        public static T GetRequiredComponentInChildren<T>(GameObject otherObject)
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
        public static T GetRequiredResource<T>(string path) where T : UnityEngine.Object
        {
            T resource = Resources.Load<T>(path) as T
                ?? throw new MissingResourceException(path);

            return resource;
        }

        /// <summary>
        /// Finds an object located on the base of the hierarchy, or throws an exception if not found
        /// </summary>
        /// <param name="name">Name of the object to find</param>
        public static GameObject GetRequiredObject(string name)
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
        public static GameObject GetRequiredChild(GameObject otherObject, string name)
        {
            Transform tranformObject = otherObject.transform.Find(name)
                ?? throw new MissingRequiredChildException(otherObject, name);

            return tranformObject.gameObject;
        }

        /// <summary>
        /// Return a component that is expected in a parent object.  Throws an exception if 
        /// it does not exist.
        /// </summary>
        /// <typeparam name="T">The component Type</typeparam>
        /// <param name="otherObject">the object to check from</param>
        /// <returns>The component</returns>
        public static T GetRequiredComponentInParent<T>(GameObject otherObject) where T : MonoBehaviour
        {
            return otherObject.GetComponentInParent<T>()
                ?? throw new MissingRequiredComponentException(otherObject, typeof(T));            
        }

        /// <summary>
        /// Return a component that is expected in a parent object.  Throws an exception if 
        /// it does not exist.
        /// </summary>
        /// <typeparam name="T">The component Type</typeparam>
        /// <param name="otherObject">the object to check</param>
        /// <returns>The component</returns>
        protected T GetRequiredComponentInParent<T>() where T : MonoBehaviour
        {
            return GetRequiredComponentInParent<T>(this.gameObject);
        }

        /// <summary>
        /// Instantiates a prefab, maintaining the prefab's object name (dropping unity's "(Clone)" suffix).  The
        /// prefab will be instantiated in the prefab's default position
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <returns>The Instance</returns>
        public static GameObject InstantiatePrefab(GameObject prefab)
        {
            GameObject instance = Instantiate(prefab);
            instance.name = prefab.name;
            return instance;
        }

        /// <summary>
        /// Instantiates a prefab, maintainins the prefab's object name (dropping unity's "(Clone)" sufix).
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <param name="parentTransform">The parent object in the hierarchy</param>
        /// <returns>The Instance</returns>
        public static GameObject InstantiatePrefab(GameObject prefab, Transform parentTransform)
        {
            GameObject instance = Instantiate(prefab, parentTransform);
            instance.name = prefab.name;
            return instance;
        }

        /// <summary>
        /// Instantiates a prefab, maintainins the prefab's object name (dropping unity's "(Clone)" sufix).
        /// Re-positions the object to the specified <paramref name="position"/> after instantiation
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <param name="position">The position to relocate the instance to</param>
        /// <param name="parentTransform">The parent object in the hierarchy</param>
        /// <returns>The Instance</returns>
        /// 
        public static GameObject InstantiatePrefab(GameObject prefab,  Vector3 position, Transform parentTransform=null)
        {
            GameObject instance = Instantiate(prefab, parentTransform);
            instance.name = prefab.name;
            instance.transform.position = position;
            return instance;
        }

        /// <summary>
        /// Call this method to immediately display a string of dialogue text, loaded via XML resource
        /// </summary>
        /// <param name="stringsFile"></param>
        public static GameObject InitiateDialogueExchange(string stringsFile)
        {
            // A prefab won't work here because we need to set properties before instantiating
            GameObject obj = new GameObject(GameObjects.TextWriterPipeline);
            TextWriterPipelineComponent pipeline = obj.AddComponent<TextWriterPipelineComponent>();
            pipeline.LoadText(stringsFile);
            return Instantiate(obj);
        }

        /// <summary>
        /// Returns true if the specified component <typeparamref name="T"/> is active 
        /// on the hierarchy currently
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool ComponentExists<T>() where T : UnityEngine.Component
        {
            return Object.FindObjectsOfType<T>().Any();
        }

        #endregion

        #region overrides
        private void Awake()
        {
            ComponentAwake(); 
        }
        private void Start() { ComponentStart(); }
        private void Update() { ComponentUpdate(); }
        private void OnDestroy() { ComponentOnDestroy(); }
        private void OnEnable() { ComponentOnEnable(); }
        

        /// <summary>
        /// Override this method to add functionality to the monobehavior's Awake Method. 
        /// <para>This should be used for things such as setting references to components, etc</para>
        /// </summary>
        public virtual void ComponentAwake() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's Start method
        /// <para>Used for setting up an object in the scene after all items are built and ready.</para>
        /// </summary>
        public virtual void ComponentStart() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's Update method
        /// <para>This is used for updates that happen every frame</para>
        /// </summary>
        public virtual void ComponentUpdate() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's OnDestroy() method
        /// <para>Used for freeing up resources, etc</para>
        /// </summary>
        public virtual void ComponentOnDestroy() { }

        /// <summary>
        /// Override this method to add functionality to monobehaviour's OnEnable() method
        /// <para>This code is executed when an object is toggled to "active"</para>
        /// </summary>
        public virtual void ComponentOnEnable() { }
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

        #region Canvas
        /// <summary>
        /// Call to get or create the canvas prefab
        /// </summary>
        protected GameObject FindOrCreateCanvas()
        {
            // try to find the canvas
            if (canvasObject == null)
            {
                canvasObject = GameObject.Find(GameObjects.Canvas);
            }

            // if it doesn't exist, then create it
            if (canvasObject == null)
            {
                GameObject canvasPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/System/{GameObjects.Canvas}");
                canvasObject = InstantiatePrefab(canvasPrefab);
                canvasObject.name = GameObjects.Canvas;
            }
            return canvasObject;
        }

        #endregion
    }
}
