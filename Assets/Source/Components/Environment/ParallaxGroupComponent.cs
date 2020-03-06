using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;

namespace Assets.Source.Components.Environment
{
    public class ParallaxGroupComponent : ComponentBase
    {
        public ParallaxComponent Parallaxer { get; set; }
        public Sprite Sprite { get; set; }
        public string Name { get; set; }
        public float ParallaxEffect { get; set; }
        public int SortingOrder { get; set; }

        public override void Construct()
        {
            ConstructParallaxBehavior();

            //adjusting the sorting order ensures that one groups images are on top of the other - smaller # means farther back in the render order
            SpriteRenderer spriteRenderer = GetRequiredComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = SortingOrder;

            base.Construct();
        }

        private void ConstructParallaxBehavior()
        {
            GameObject parallaxLayerResource = GetRequiredResource<GameObject>($"{ResourcePaths.EnvironmentPrefabsFolder}/ParallaxLayer");

            for (int i = 0; i < Parallaxer.GetLayersInGroup(); i++)
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