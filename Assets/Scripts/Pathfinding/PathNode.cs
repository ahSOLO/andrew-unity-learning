using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int z;
    public bool isWalkable = true;

    public float gCost;
    public float hCost;
    public float fCost { get => gCost + hCost; }

    public PathNode lastNode;

    public PathNode(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        if (isWalkable)
            return x + "," + z + "\n" + fCost;
        else
            return "Unwalkable";
    }
}