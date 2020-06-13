using Assets.Source.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Components.Animation
{
    /// <summary>
    /// To use this component, add an event to your animation timeline that calls the destroy method when the animation is done 
    /// </summary>
    public class AnimationDestroyComponent : ComponentBase
    {

        public void DestroySelf() {
            Destroy(gameObject);
        }

    }
}
