using Assets.Source.Components.Pathing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.AStar
{
    public class AStarPathMapper
    {


        private NavigationMeshComponent navigationMesh;

        private Node[][] nodes;
        private int gridWidth;
        private int gridHeight;

        public AStarPathMapper(NavigationMeshComponent navMesh)
        {
            this.navigationMesh = navMesh;
        }

        public List<Node> FindPath(Vector2 startPosition, Vector2 targetPosition, bool allowDiagonalMovement = false)
        {
            // Perform all pathing operations on a clone of currrent A* grid
            nodes = navigationMesh.CloneGrid();
            gridWidth = navigationMesh.GridWidth;
            gridHeight = navigationMesh.GridHeight;

            // Find the nodes closest to our destination positions
            // if these are off the grid, will return the edge of the grid as close as it can get
            var startNodePosition = navigationMesh.FindNearestNodeIndex(startPosition);
            var targetNodePosition = navigationMesh.FindNearestNodeIndex(targetPosition);
            var startNode = nodes[startNodePosition.ix][startNodePosition.iy];
            // For the target node, if the destination is a solid node, find the nearest non-solid neighbor node instead
            var targetNode = FindNearestOpenNode(nodes[targetNodePosition.ix][targetNodePosition.iy], startNode);

            // The "open list" is the list of nodes that we need to visit
            var openList = new List<Node>() { startNode };

            // The "closed set" is the list of nodes that we have visited already
            var closedList = new List<Node>();

            // Loop until we have no more nodes to visit
            while (openList.Count > 0)
            {
                // Grab whatever the next node on the list is
                var currentNode = openList.First();

                // Iterate through the open list starting from the second element to figure out what node we want to visit next
                // If there is no other element, this whole thing is skipped
                foreach (var node in openList.Skip(1))
                {
                    // if this node appears to get us to our destination quicker, visit that boy first
                    if (node.FCost <= currentNode.FCost && node.HCost < currentNode.HCost)
                    {
                        currentNode = node;
                    }
                }

                // mark this node as visited, remove it from the open list and add it to the closed list
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // if we find our target node
                if (currentNode == targetNode)
                {
                    // Crawl backwards to retrieve the total path
                    return TraceFinalPath(startNode, targetNode);
                }

                foreach (Node neighbor in FindNeighborNodes(currentNode, allowDiagonalMovement))
                {
                    // Ignore any solid or previously visited nodes
                    if ((neighbor != targetNode && neighbor.IsSolid) || closedList.Contains(neighbor))
                    {
                        continue;
                    }

                    // For this implementation of A*, the GCost is the linear distance between node indices 
                    // Add the distance between currentNode and this particular neighbor 
                    //var moveCost = currentNode.GCost + GetDistance(currentNode, neighbor); //<- This might not be right(?)
                    var moveCost = currentNode.GCost + GetDistance(currentNode, targetNode);

                    // Check if this neighbor node should be where we visit next
                    if (moveCost < neighbor.GCost || !openList.Contains(neighbor))
                    {
                        neighbor.GCost = moveCost;
                        neighbor.HCost = GetDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            // If we managed to get here, it means we have visited every node and could not find a path.
            // Return an empty list, and leave it up to the implementor to decide what to do 
            return new List<Node>();
        }

        // Find the linear distance between two indices in the node array
        private float GetDistance(Node node1, Node node2)
        {
            var x = Mathf.Abs(node1.XIndex - node2.XIndex);
            var y = Mathf.Abs(node2.YIndex - node2.YIndex);
            return x + y;
        }

        // Follow the parent nodes backwards from destination => start
        private List<Node> TraceFinalPath(Node startNode, Node targetNode)
        {
            var path = new List<Node>();

            var currentNode = targetNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            // reverse the list so we are going from start to finish 
            path.Reverse();

            return path;
        }

        private IEnumerable<Node> FindNeighborNodes(Node node, bool includeDiagonals = true)
        {
            for (var ix = -1; ix < 2; ix++)
            {
                for (var iy = -1; iy < 2; iy++)
                {
                    var nx = node.XIndex + ix;
                    var ny = node.YIndex + iy;

                    if (nx >= 0 && nx <= nodes.Length - 1)
                    {
                        if (ny >= 0 && ny <= nodes[nx].Length - 1)
                        {
                            if (includeDiagonals)
                            {
                                // return all neighbor nodes who are not me 
                                if (ix != 0 || iy != 0)
                                {
                                    yield return nodes[nx][ny];
                                }
                            }
                            else
                            {
                                // Only return this node if it is not a diagonal
                                // Diagonals are defined as coordinates relative to this current node
                                // where the absolute value of both x and y index is non-zero
                                if (Mathf.Abs(ix) == 0 || Mathf.Abs(iy) == 0)
                                {
                                    // return all nodes who are not me
                                    if (ix != 0 || iy != 0)
                                    {
                                        yield return nodes[nx][ny];
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        // Gets the actual total distance in worlds space between two points
        private float GetPhysicalDistance(Node node1, Node node2) => Vector2.Distance(node1.Center, node2.Center);

        // recursively seek out a valid non-solid node
        private Node FindNearestOpenNode(Node node, Node startNode, Node originalNode = null, List<Node> visited = null)
        {
            // If this node is non-solid, return it right away
            if (!node.IsSolid)
            {
                return node;
            }

            // If sourceNode is null, that means this is the first layer of recursion
            var sourceNode = originalNode ?? node;

            // Begin keeping track of visited neighbors
            if (visited == null)
            {
                visited = new List<Node>();
            }
            visited.Add(node);

            // Grab each surrounding nodes around this node, but order by its physical distance
            // ordering it like that assures us that the first node we check will be the closest to us
            var neighbors = FindNeighborNodes(node, true)
                            .Where(n => visited != null && !visited.Contains(n))
                            .OrderBy(n => GetPhysicalDistance(n, startNode))
                            .OrderBy(n => GetPhysicalDistance(n, sourceNode));

            // Check each neighbor for its nearest non-solid node
            // Remember, this will return the node itself if the node isn't solid
            foreach (Node neighbor in neighbors)
            {
                var firstNonSolidNode = FindNearestOpenNode(neighbor, startNode, sourceNode, visited);
                if (firstNonSolidNode != null)
                {
                    return firstNonSolidNode;
                }
            }
            // searched all neighbors, found no solids 
            return null;
        }
    }
}
