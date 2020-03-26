using Assets.Source.Components.Base;
using Assets.Source.Components.Director.Interfaces;
using Assets.Source.Components.TextWriter;
using Assets.Source.Components.UI.Base;
using Assets.Source.Constants;
using System;
using UnityEngine;

namespace Assets.Source.Components.Director.Base
{
    public abstract class DirectorComponentBase : ComponentBase
    {
        // todo: if we ever need another implementation of ILevelContext, I think we should make this not abstract, and do the start phase / context stuff a different way

        /// <summary>
        /// The context is an abstraction layer between the director and the level phase
        /// </summary>
        protected abstract ILevelContext Context { get; }

        protected abstract void SetStartPhase();


        public override void ComponentAwake()
        {
            SetStartPhase();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (Context.IsCompleted && CanUpdatePhase())
            {
                Context.CompletePhase();
            }
            else
            { 
                Context.UpdatePhase();
            }
            base.ComponentUpdate();
        }

        private bool CanUpdatePhase()
        {
            // Currently returns true if there's no open menu and no text writer
            return (!ComponentExists<TextWriterPipelineComponent>() &&
                    !ComponentExists<TextWriterComponent>() &&
                    !ComponentExists<MenuComponentBase>());
            
        }

    }
}
