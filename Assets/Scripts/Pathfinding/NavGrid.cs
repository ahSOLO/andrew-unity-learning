using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : IGameService
{
    public Grid<PathNode> grid;
    [SerializeField] int width;
    [SerializeField] int length;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 originPos;

    private const float MOVE_STRAIGHT_COST = 1f;
    private const float MOVE_DIAGONAL_COST = 1.414214f;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<PathNode>(width, length, cellSize, originPos, (int x, int z) => new PathNode(x, z));
        DetectWalkable();
        ServiceLocator.Register<NavGrid>(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DetectWalkable()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetLength(); z++)
            {
                if (Physics.OverlapBox(grid.GetWorldPosition(x, z), new Vector3(cellSize / 2, cellSize / 2, cellSize / 2), Quaternion.identity, LayerMask.GetMask("Wall")).Length > 0)
                {
                    grid.GetValue(x, z).isWalkable = false;
                    grid.TriggerGridObjectChanged(x, z);
                }
            }
        }
    }

    public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ)
    {        
        PathNode startNode = grid.GetValue(startX, startZ);
        PathNode endNode = grid.GetValue(endX, endZ);
        
        // Initialize lists or clear them if already initialized
        if (openList != null)
        {
            openList.Clear();
            openList.Add(startNode);
        }
        else 
            openList = new List<PathNode> { startNode};
        
        if (closedList != null)
            closedList.Clear();
        else
            closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetLength(); z++)
            {
                PathNode pathNode = grid.GetValue(x, z);
                pathNode.gCost = float.MaxValue; // Set the g Cost to the max value when initializing the grid.
                pathNode.lastNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached Final Node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (var neighborNode in GetNeighborList(currentNode))  
            {
                if (closedList.Contains(neighborNode)) continue;
                float tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.lastNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        // Out of nodes on open list
        return null;
    }

    public List<PathNode> FindPath(Vector3 startPos, Vector3 endPos)
    {
        int startGridX;
        int startGridZ;
        int endGridX;
        int endGridZ;

        grid.GetGridPosition(startPos, out startGridX, out startGridZ);
        grid.GetGridPosition(endPos, out endGridX, out endGridZ);

        return FindPath(startGridX, startGridZ, endGridX, endGridZ);
    }

    private List<PathNode> GetNeighborList(PathNode currentNode)
    {
        List<PathNode> neighborList = new List<PathNode>();

        if (currentNode.x > 0)
        {
            // Add Left
            AddNeighborIfWalkable(currentNode.x - 1, currentNode.z);
            if (currentNode.z > 0)
                // Add Bottom Left
                AddNeighborIfWalkable(currentNode.x - 1, currentNode.z - 1);
            if (currentNode.z < grid.GetLength() - 1)
                // Add Top Left
                AddNeighborIfWalkable(currentNode.x - 1, currentNode.z + 1);
        }
        if (currentNode.x < grid.GetWidth() - 1)
        {
            // Add Right
            AddNeighborIfWalkable(currentNode.x + 1, currentNode.z);
            if (currentNode.z > 0)
                // Add Bottom Right
                AddNeighborIfWalkable(currentNode.x + 1, currentNode.z - 1);
            if (currentNode.z < grid.GetLength() - 1)
                // Add Top Right
                AddNeighborIfWalkable(currentNode.x + 1, currentNode.z + 1);
        }
        // Add Bottom
        if (currentNode.z > 0)
        {
            AddNeighborIfWalkable(currentNode.x, currentNode.z - 1);
        }

        // Add Top
        if (currentNode.z < grid.GetLength() - 1)
        {
            AddNeighborIfWalkable(currentNode.x, currentNode.z + 1);
        }

        return neighborList;

        void AddNeighborIfWalkable(int x, int z)
        {
            var pathNode = grid.GetValue(x, z);
            if (pathNode.isWalkable)
                neighborList.Add(pathNode);
        }
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode> { endNode };
        PathNode currentNode = endNode;
        while (currentNode.lastNode != null)
        {
            path.Add(currentNode.lastNode);
            currentNode = currentNode.lastNode;
        }
        path.Reverse();
        //foreach (var node in path)
        //{
        //    grid.TriggerGridObjectChanged(node.x, node.z);
        //}
        return path;
    }

    private float CalculateDistanceCost(PathNode a, PathNode b)
    {
        float xDistance = Mathf.Abs(a.x - b.x);
        float zDistance = Mathf.Abs(a.z - b.z);
        float remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}
