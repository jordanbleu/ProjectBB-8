using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;

public class Parallaxer : ComponentBase
{
    private readonly int LAYERS_PER_GROUP = 3; //just in case we decide to change it eventually
    private readonly string PARALLAX_PREFAB_DIRECTORY = $"{ResourcePaths.PrefabsFolder}/Environment";
    private readonly int SORTING_ORDER = -10; //we want these images to be behind everything else

    [System.Serializable]
    public struct ParallaxInfo
    {
        public string name;
        public Sprite sprite;
        [Range(1, 1000)]
        [Tooltip("1 will not move at all, 1000 will move the fastest. Calculates speed using log of this value")]
        public float parallaxEffect;
    };

    [Tooltip("Each Parallax Info is the information needed to generate one layer of the parallax effect")]
    public ParallaxInfo[] parallaxInfos;

    public override void Construct()
    {
        Transform startingPos = Camera.main.transform;
        transform.position = startingPos.position;

        ConstructParallaxGroups();

        base.Construct();
    }

    private void ConstructParallaxGroups()
    {
        int tempSortingOrder = SORTING_ORDER;
        int index = 0;
        foreach (ParallaxInfo parallaxInfo in parallaxInfos)
        {
            GameObject parallaxGroupPrefab = InstantiatePrefab(GetRequiredResource<GameObject>($"{PARALLAX_PREFAB_DIRECTORY}/ParallaxGroup"), transform.position);
            parallaxGroupPrefab.name = $"{parallaxInfo.name}Group";

            ParallaxGroup parallaxGroup = parallaxGroupPrefab.GetComponent<ParallaxGroup>();
            parallaxGroup.Parallaxer = this;
            parallaxGroup.Sprite = parallaxInfo.sprite;
            parallaxGroup.ParallaxEffect = parallaxInfo.parallaxEffect;
            parallaxGroup.Name = parallaxInfo.name;
            parallaxGroup.SortingOrder = tempSortingOrder;

            tempSortingOrder++;
            index++;
        }
    }

    public int GetLayersInGroup()
    {
        return LAYERS_PER_GROUP;
    }

    public string GetPrefabDirectory()
    {
        return PARALLAX_PREFAB_DIRECTORY;
    }
}