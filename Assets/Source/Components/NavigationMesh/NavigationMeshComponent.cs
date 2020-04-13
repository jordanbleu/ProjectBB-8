using Assets.Source.AStar;
using Assets.Source.Components.Base;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.NavigationMesh
{
    public class NavigationMeshComponent : ComponentBase
    {
        [Tooltip("How many tiles going horizontally")]
        [SerializeField]
        private int gridWidth = 20;
        public int GridWidth => gridWidth;

        [Tooltip("How many tiles going vertically")]
        [SerializeField]
        private int gridHeight = 15;
        public int GridHeight => gridHeight;


        [Tooltip("The size of each tile in units.  Should be large enough for a pathfinder to pass through " +
            "without clipping the edge of corners.")]
        [SerializeField]
        private float tileSize = 0.5f;

        [Tooltip("Recalculates the entire navigation mesh every frame.  Leave this on if things in your " +
            "level move around, or set to false if the level is static for a performance boost.")]
        [SerializeField]
        private bool syncEveryFrame = true;

        private Node[][] nodes;

        public override void ComponentAwake()
        {
            nodes = new Node[gridWidth][];
            GenerateNavigationMesh();
            base.ComponentAwake();
        }
                
        // Generate the initial nav mesh
        private void GenerateNavigationMesh()
        {
            for (var ix = 0; ix < gridWidth; ix++)
            {
                nodes[ix] = new Node[gridHeight];
                for (var iy = 0; iy < gridHeight; iy++)
                {
                    var center = new Vector2(x: (ix * tileSize) + transform.position.x, y: (iy * tileSize) + transform.position.y);
                    var isSolid = TileContainsSolid(center);

                    nodes[ix][iy] = new Node(center, ix, iy, isSolid);
                }
            }
        }

        // Returns true if any solid exists in a radius of 'tileSize' around the specified center position
        private bool TileContainsSolid(Vector2 center) => Physics2D.OverlapArea(new Vector2(center.x - (tileSize / 2), center.y - (tileSize / 2)),
                                                                                new Vector2(center.x + (tileSize / 2), center.y + (tileSize / 2))) != null;

        

        public override void ComponentUpdate()
        {
            if (syncEveryFrame)
            {
                UpdateTiles();
            }
            base.ComponentUpdate();
        }

        /// <summary>
        /// Used to clone the 2d array of nodes so we can modify them safely
        /// </summary>
        /// <returns></returns>
        public Node[][] CloneGrid() => nodes.Select(n => n.ToArray()).ToArray();


        // For each tile, update its "isSolid" status.
        private void UpdateTiles()
        {
            if (nodes != null)
            {
                foreach (var nodeRow in nodes)
                {
                    foreach (var node in nodeRow)
                    {
                        node.IsSolid = TileContainsSolid(node.Center);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a tuple which contains the position of the nearest node.  If the specified position is beyond the boundaries 
        /// of the navigation mesh, it will return the closest node it can get within the confines of the grid. 
        /// </summary>
        /// <param name="worldPosition">Position in world space </param>
        /// <returns>x / y position of the node</returns>
        public (int ix, int iy) FindNearestNodeIndex(Vector2 worldPosition)
        {
            // Basically reverse the function to plot the grid square to figure out its index
            var row = (int)Mathf.Clamp(Mathf.Round((worldPosition.x - transform.position.x) / tileSize), 0, gridWidth - 1);
            var col = (int)Mathf.Clamp(Mathf.Round((worldPosition.y - transform.position.y) / tileSize), 0, gridHeight - 1);
            return (row, col);
        }


        // todo: change to this line because otherwise that grid will get annoying 
        //private void OnDrawGizmosSelected()
        private void OnDrawGizmos()
        {
            if (nodes != null)
            {
                for (var ix = 0; ix < nodes.Length; ix++)
                {
                    for (var iy = 0; iy < nodes[0].Length; iy++)
                    {
                        var node = nodes[ix][iy];

                        if (node.IsSolid)
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            Gizmos.color = Color.grey;
                        }

                        Gizmos.DrawWireCube(node.Center, new Vector2(tileSize * 0.95f, tileSize * 0.95f));
                    }
                }
            }
        }
    }
}
