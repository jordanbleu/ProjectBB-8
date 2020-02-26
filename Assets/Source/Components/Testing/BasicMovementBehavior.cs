using Assets.Source.Components.Base;
using Assets.Source.Input.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use this for really simple tests and stuff to use the menu movement buttons to move stuff
/// </summary>
public class BasicMovementBehavior : ComponentBase
{

    [SerializeField]
    private float translateSpeed = 0.1f;

    public override void Step()
    {

        if (InputManager.IsKeyHeld(InputConstants.K_MENU_LEFT))
        {
            transform.Translate(new Vector2(-translateSpeed, 0));
        }
        else if (InputManager.IsKeyHeld(InputConstants.K_MENU_RIGHT))
        {
            transform.Translate(new Vector2(translateSpeed, 0));
        }

        if (InputManager.IsKeyHeld(InputConstants.K_MENU_UP))
        {
            transform.Translate(new Vector2(0, translateSpeed));
        }
        else if (InputManager.IsKeyHeld(InputConstants.K_MENU_DOWN))
        {
            transform.Translate(new Vector2(0, -translateSpeed));
        }


        base.Step();
    }

}
