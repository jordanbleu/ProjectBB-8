using Assets.Source.Components.Base;
using UnityEngine;

public class ParallaxGroup : ComponentBase
{
    public Parallaxer Parallaxer { get; set; }
    public Sprite Sprite { get; set; }
    public string Name { get; set; }
    public float ParallaxEffect { get; set; }
    public int SortingOrder { get; set; }

    public override void Create()
    {
        ConstructParallaxLayers();

        //adjusting the sorting order ensures that one groups images are on top of the other - smaller # means farther back in the render order
        SpriteRenderer spriteRenderer = GetRequiredComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = SortingOrder;

        base.Create();
    }

    private void ConstructParallaxLayers()
    {
        for (int i = 0; i < Parallaxer.GetLayersInGroup(); i++)
        {
            GameObject parallaxLayerPrefab = InstantiatePrefab(GetRequiredResource<GameObject>($"{Parallaxer.GetPrefabDirectory()}/ParallaxLayer"), transform.position);
            parallaxLayerPrefab.name = $"{Name}Layer{i}";

            ParallaxLayer parallaxLayer = parallaxLayerPrefab.GetComponent<ParallaxLayer>();
            parallaxLayer.ParallaxGroup = this;
            parallaxLayer.Name = Name;
            parallaxLayer.PositionNum = i;
        }
    }
}
