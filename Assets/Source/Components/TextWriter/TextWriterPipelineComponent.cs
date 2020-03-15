using Assets.Source.Components.Base;
using Assets.Source.Constants;
using Assets.Source.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.TextWriter
{
    /// <summary>
    /// This component handles creating text writers for each message in the stack, until it runs out
    /// </summary>
    public class TextWriterPipelineComponent : ComponentBase
    {
        private Stack<string> texts;
        private GameObject textWriterObject;

        private GameObject textWriterPrefab;

        public override void ComponentAwake()
        {
            textWriterPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/TextWriter/{GameObjects.TextWriter}");
            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            if (textWriterObject == null)
            {
                if (texts != null && texts.Any())
                {
                    InstantiateNext();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            base.ComponentUpdate();
        }

        private void InstantiateNext()
        {
            string message = texts.Pop();

            textWriterObject = InstantiatePrefab(textWriterPrefab, FindOrCreateCanvas().transform);
            textWriterObject.transform.localPosition = new Vector3(0, -80, 0);

            TextWriterComponent textWriter = GetRequiredComponent<TextWriterComponent>(textWriterObject);
            textWriter.LoadText(message);
            textWriterObject.SetActive(true);
        }

        /// <summary>
        /// Loads in a string resource from XML as a set of strings
        /// </summary>
        /// <param name="resource">String Resource name (name in the path after the language code)</param>
        public void LoadText(string resource)
        {
            Destroy(textWriterObject);

            StringsLoader loader = new StringsLoader();
            loader.Load(resource);

            if (loader.Value != null)
            {
                // This probably isn't very efficient
                texts = new Stack<string>(loader.Value.Values.Reverse().ToList());
            }
        }
    }
}
