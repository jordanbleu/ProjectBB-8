using Assets.Source.AStar;
using Assets.Source.Components.Projectile.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Pathing
{
    /// <summary>
    /// The pathfinder component relies on an existing navigation mesh to find its way around obstacles
    /// </summary>
    public abstract class ProjectilePathfinderComponent : ProjectileComponentBase
    {
        // The list of nodes in the last calculated path
        // This only exists for debug purposes and should be deleted someday
        private List<Node> lastCalculatedPath;

        // Current nav mesh to follow
        private NavigationMeshComponent navigationMesh;

        // Required to perform the AStar algorithm
        private AStarPathMapper pathMapper;


        private Vector2? _destination;
        /// <summary>
        /// The pathfinders overall current destination 
        /// </summary>
        protected Vector2? Destination => _destination;

        private Vector2? _currentPoint;
        /// <summary>
        /// The current destination that the pathfinder needs to get to
        /// </summary>
        protected Vector2? CurrentPoint => _currentPoint;

        public override void ComponentAwake()
        {
            navigationMesh = GetRequiredComponent<NavigationMeshComponent>(GetRequiredChild("NavigationMesh", FindLevelObject()));
            pathMapper = new AStarPathMapper(navigationMesh);
            base.ComponentAwake();
        }

        protected void SeekPath(Vector2 destination)
        {
            lastCalculatedPath = pathMapper.FindPath(transform.position, destination);
            _currentPoint = lastCalculatedPath?.FirstOrDefault()?.Center;
            _destination = destination;
        }

        private void OnDrawGizmosSelected()
        {
            if (lastCalculatedPath != null && _currentPoint.HasValue && _destination.HasValue)
            {
                // the blue circles show the mapped path
                Gizmos.color = Color.cyan;
                foreach (Node node in lastCalculatedPath)
                {
                    Gizmos.DrawWireSphere(node.Center, 0.1f);
                }

                // The red line is the currently seeked node
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _currentPoint.Value);

                // The green circle is the actor's destination
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_destination.Value, 0.1f);
            }
        }
    }
}
