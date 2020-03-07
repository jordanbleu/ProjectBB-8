using System.Collections;
using System.Collections.Generic;
using Assets.Source.Components.Base;
using UnityEngine;

public class ParallaxLayer : ComponentBase
{
    public float height;
    public float startpos;
    [Range(0, 100)]
    [Tooltip("0 will not move at all, 100 will move the fastest")]
    public float parallaxEffect;
    private float timePassed;
    public float distance;
    private Parallax parallax;
    public int childOrder;

    public override void PerformAwake()
    {
        parallax = GetComponentInParent<Parallax>();
        base.PerformAwake();
    }

    public override void PerformStart()
    {
        startpos = transform.position.y;
        SpriteRenderer spriteRenderer = GetRequiredComponent<SpriteRenderer>();
        height = spriteRenderer.bounds.size.y;
        base.PerformStart();
    }

    public override void PerformOnDestroy()
    {
        base.PerformOnDestroy();
    }

    public override void PerformUpdate()
    {
        timePassed += Time.deltaTime;
        distance = timePassed * parallaxEffect;


        transform.position = new Vector3(transform.position.x, startpos - distance, transform.position.z);

        //if(distanceRelativeToCamera > startpos + height)
        //{
        //    startpos += height;
        //}
        //else if(distanceRelativeToCamera < startpos - height)
        //{
        //    startpos -= height;
        //}

        if(distance > height + startpos)
        {
            timePassed = 0;
            distance = 0;
            float amountToAdd = startpos + height;// * (parallax.duplicatesPerChild - 1);
            startpos = 33 - height;
            Debug.Log("starpos += " + amountToAdd);
        }

        base.PerformUpdate();
    }
}
