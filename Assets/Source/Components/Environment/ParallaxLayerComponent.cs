using System;
using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.Environment
{
    public class ParallaxLayerComponent : ComponentBase
    {
        public string Name { private get; set; }
        public int PositionNum { private get; set; }
        public ParallaxGroupComponent ParallaxGroupComponent { private get; set; }

        private float totalGroupHeight;
        private float height;
        private float startpos;
        private float timePassed;
        private float distance;
        private Vector3 adjustedPosition;
        private SpriteRenderer spriteRenderer;

        public override void ComponentAwake()
        {
            spriteRenderer = GetRequiredComponent<SpriteRenderer>();
            
            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            spriteRenderer.sprite = ParallaxGroupComponent.Sprite;
            spriteRenderer.sortingOrder = ParallaxGroupComponent.SortingOrder;
            height = spriteRenderer.bounds.size.y;

            totalGroupHeight = height * ParallaxGroupComponent.ParallaxComponent.GetLayersInGroup();
            adjustedPosition = new Vector3(transform.position.x, transform.position.y + (height * PositionNum));
            transform.position = adjustedPosition;
            startpos = transform.position.y;

            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            //doing it this way will make it so that if we want to pause the game we change Time.timescale to 0 and this will freeze the parallax
            timePassed += Time.deltaTime;
            distance = timePassed * (float)Math.Log(ParallaxGroupComponent.ParallaxEffect);
            transform.position = new Vector3(transform.position.x, startpos - distance, transform.position.z);

            float offScreenPosition = height + startpos;
            if (distance > offScreenPosition)
            {
                //need to align this layer to the top of the group by adjusting by the height of the group and current location of group
                startpos = totalGroupHeight - height - (distance - offScreenPosition);
                timePassed = 0;
                distance = 0;
            }
            
            base.ComponentUpdate();
        }
    }
}