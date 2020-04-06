using Assets.Source.Components.Base;
using Assets.Source.Components.TextWriter;
using Assets.Source.Components.UI.Base;
using Assets.Source.Director;
using Assets.Source.Director.Enums;
using Assets.Source.Director.Interfaces;
using Assets.Source.Mathematics;
using UnityEngine;

namespace Assets.Source.Components.Director.Base
{
    public class DirectorComponent : ComponentBase
    {
        [SerializeField]
        private LevelRepository.Level level = LevelRepository.Level.TestLevel;

        [SerializeField]
        private Square boundaries = new Square(5,5);
        
        /// <summary>
        /// Returns the current boundaries for the level.  
        /// </summary>
        public Square Boundaries => boundaries;

        /// <summary>
        /// The context is an abstraction layer between the director and the level phase
        /// </summary>
        protected ILevelContext Context { get; private set; }

        public override void ComponentAwake()
        {
            Context = new LevelContext();
            Context.BeginPhase(LevelRepository.FindStartPhase(level));
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(boundaries.TopLeft, boundaries.TopRight);
            Gizmos.DrawLine(boundaries.TopRight, boundaries.BottomRight);
            Gizmos.DrawLine(boundaries.BottomRight, boundaries.BottomLeft);
            Gizmos.DrawLine(boundaries.BottomLeft, boundaries.TopLeft);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(0, -boundaries.Height, 0), new Vector3(0, boundaries.Height, 0));
            Gizmos.DrawLine(new Vector3(-boundaries.Width, 0, 0), new Vector3(boundaries.Width, 0, 0));
            
        }

    }
}
