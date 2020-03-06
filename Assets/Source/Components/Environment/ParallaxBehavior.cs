using System;
using Assets.Source.Components.Base;
using UnityEngine;

public class ParallaxLayer : ComponentBase
{
    public ParallaxGroup ParallaxGroup { get; set; }
    public string Name { get; set; }
    public int PositionNum { get; set; }

    private float totalGroupHeight;
    private float height;
    private float startpos;
    private float timePassed;
    private float distance;
    private Vector3 adjustedPosition;

    public override void Create()
    {
        SpriteRenderer spriteRenderer = GetRequiredComponent<SpriteRenderer>();
        spriteRenderer.sprite = ParallaxGroup.Sprite;
        spriteRenderer.sortingOrder = ParallaxGroup.SortingOrder;
        height = spriteRenderer.bounds.size.y;

        totalGroupHeight = height * ParallaxGroup.Parallaxer.GetLayersInGroup();
        adjustedPosition = new Vector3(transform.position.x, transform.position.y + (height * PositionNum));
        transform.position = adjustedPosition;
        startpos = transform.position.y;

        base.Create();
    }

    public override void Step()
    {
        //doing it this way will make it so that if we want to pause the game we change Time.timescale to 0 and this will freeze the parallax
        timePassed += Time.deltaTime;
        distance = timePassed * (float)Math.Log(ParallaxGroup.ParallaxEffect);
        transform.position = new Vector3(transform.position.x, startpos - distance, transform.position.z);

        float offScreenPosition = height + startpos;
        if(distance > offScreenPosition)
        {
            //need to align this layer to the top of the group by adjusting by the height of the group and current location of group
            startpos = totalGroupHeight - height - (distance - offScreenPosition);
            timePassed = 0;
            distance = 0;
        }

        base.Step();
    }
}
