using System.Collections;
using System.Collections.Generic;
using Assets.Source.Components.Base;
using UnityEngine;

public class Parallax : ComponentBase
{
    public struct ParallaxInfo
    {
        public Sprite sprite;
        public int layer;
        public float parallaxEffect;
    };
    public int duplicatesPerChild = 3;

    public ParallaxInfo parallaxInfo;
    public GameObject cam;
    public GameObject[] children;
    public int layers = 2;
    public Sprite[] sprites;

    public override void PerformAwake()
    {
        //children = GetComponentsInChildren<GameObject>();
        base.PerformAwake();
    }

    public override void PerformStart()
    {
        Vector3 camPosition = cam.transform.position;
        //Transform firstChildTransform = children[1].transform;
        //Transform secondChildTransform = children[2].transform;
        RectTransform rectTransform = cam.transform as RectTransform;
        float height = rectTransform.rect.height;
        float width = rectTransform.rect.width;
        //firstChildTransform.position = new Vector3(camPosition.x, camPosition.y, firstChildTransform.position.z);
        //secondChildTransform.position = new Vector3(firstChildTransform.position.x, firstChildTransform.position.y + );

        base.PerformStart();
    }

    public override void PerformOnDestroy()
    {
        base.PerformOnDestroy();
    }

    public override void PerformUpdate()
    {
        base.PerformUpdate();
    }
}
