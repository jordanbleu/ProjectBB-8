using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;

namespace Assets.Source.Components.Environment
{
    public class ParallaxGroupComponent : ComponentBase
    {
        public ParallaxComponent ParallaxComponent { get; set; }
        public Sprite Sprite { get; set; }
        public string Name { get; set; }
        public float ParallaxEffect { get; set; }
        public int SortingOrder { get; set; }

        public override void Create()
        {
            ConstructParallaxBehavior();

            base.Create();
        }

        private void ConstructParallaxBehavior()
        {
            GameObject parallaxLayerResource = GetRequiredResource<GameObject>($"{ResourcePaths.EnvironmentPrefabsFolder}/ParallaxLayer");

            if (ParallaxComponent != null)
            {
                for (int i = 0; i < ParallaxComponent.GetLayersInGroup(); i++)
                {
                    GameObject parallaxLayerPrefab = InstantiatePrefab(parallaxLayerResource, transform.position);
                    parallaxLayerPrefab.name = $"{Name}Layer{i}";

                    ParallaxBehavior parallaxLayer = parallaxLayerPrefab.GetComponent<ParallaxBehavior>();
                    parallaxLayer.ParallaxGroupComponent = this;
                    parallaxLayer.Name = Name;
                    parallaxLayer.PositionNum = i;
                }
            }
        }
    }
}