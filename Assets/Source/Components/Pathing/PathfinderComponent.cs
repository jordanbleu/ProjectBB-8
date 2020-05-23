using Assets.Source.AStar;
using Assets.Source.Components.Base;
using Assets.Source.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Pathing
{
    /// <summary>
    /// The pathfinder component relies on an existing navigation mesh to find its way around obstacles
    /// </summary>
    public class PathfinderComponent : ComponentBase
    {
        // The list of nodes in the last calculated path
        // This only exists for debug purposes and should be deleted someday
        private List<Node> lastCalculatedPath;

        // Current nav mesh to follow
        private NavigationMeshComponent navigationMesh;

        // Required to perform the AStar algorithm
        private AStarPathMapper pathMapper;

        /// <summary>
        /// The pathfinders overall current destination 
        /// </summary>
        public Vector2? Destination { get; private set; }

        /// <summary>
        /// The current destination that the pathfinder needs to get to
        /// </summary>
        public Vector2? CurrentPoint { get; private set; }

        public override void ComponentAwake()
        {
            navigationMesh = GetRequiredComponent<NavigationMeshComponent>(GetRequiredChild("NavigationMesh", FindLevelObject()));
            pathMapper = new AStarPathMapper(navigationMesh);
            base.ComponentAwake();
        }

        public void SeekPath(Vector2 destination)
        {
            Destination = destination;
            lastCalculatedPath = pathMapper.FindPath(transform.position, destination, true);
            CurrentPoint = lastCalculatedPath?.FirstOrDefault()?.Center;
        }

        private void OnDrawGizmosSelected()
        {
            if (lastCalculatedPath != null && CurrentPoint.HasValue && Destination.HasValue)
            {
                // the blue circles show the mapped path
                Gizmos.color = Color.cyan;
                foreach (Node node in lastCalculatedPath)
                {
                    Gizmos.DrawWireSphere(node.Center, 0.1f);
                }

                // The red line is the currently seeked node
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, CurrentPoint.Value);

                // The green circle is the actor's destination
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(Destination.Value, 0.1f);
            }
        }
    }
}
