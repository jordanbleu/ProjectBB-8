using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;

namespace Assets.Source.Components.Environment
{
    public class ParallaxComponent : ComponentBase
    {
        private readonly int LAYERS_PER_GROUP = 3; //just in case we decide to change it eventually
        private readonly int SORTING_ORDER = -10; //we want these images to be behind everything else

        [System.Serializable]
        private struct ParallaxInfo
        {
            public string name;
            public Sprite sprite;

            [Range(1, 1000)]
            [Tooltip("1 will not move at all, 1000 will move the fastest. Calculates speed using log of this value")]
            public float parallaxEffect;
        };

        [Tooltip("Each Parallax Info is the information needed to generate one layer of the parallax effect")]
        [SerializeField]
        private ParallaxInfo[] parallaxInfos;

        public override void ComponentStart()
        {
            ConstructParallaxGroups();
            Transform startingPos = UnityEngine.Camera.main.transform;
            transform.position = startingPos.position;

            base.ComponentStart();
        }

        private void ConstructParallaxGroups()
        {
            GameObject parallaxGroupResource = GetRequiredResource<GameObject>($"{ResourcePaths.EnvironmentPrefabsFolder}/ParallaxGroup");
            int tempSortingOrder = SORTING_ORDER;

            foreach (ParallaxInfo parallaxInfo in parallaxInfos)
            {
                GameObject parallaxGroupPrefab = InstantiatePrefab(parallaxGroupResource, transform.position);
                parallaxGroupPrefab.name = $"{parallaxInfo.name}Group";

                ParallaxGroupComponent parallaxGroupComponent = parallaxGroupPrefab.GetComponent<ParallaxGroupComponent>();
                parallaxGroupComponent.ParallaxComponent = this;
                parallaxGroupComponent.Sprite = parallaxInfo.sprite;
                parallaxGroupComponent.ParallaxEffect = parallaxInfo.parallaxEffect;
                parallaxGroupComponent.Name = parallaxInfo.name;
                parallaxGroupComponent.SortingOrder = tempSortingOrder;

                tempSortingOrder++;
            }
        }

        public int GetLayersInGroup()
        {
            return LAYERS_PER_GROUP;
        }
    }
}   