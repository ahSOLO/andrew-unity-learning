using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    
    private int width;
    private int length;
    private float cellSize;
    public Vector3 originPos;
    private TGridObject[,] gridArray;

    public Grid(int width, int length, float cellSize, Vector3 originPos, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.length = length;
        this.cellSize = cellSize;
        this.originPos = originPos;

        gridArray = new TGridObject[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 100f);
            }
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetLength()
    {
        return length;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    private void GetGridPosition(Vector3 worldPos, out int x, out int y)
    {
        var relativePos = worldPos - originPos;
        x = Mathf.FloorToInt(relativePos.x / cellSize);
        y = Mathf.FloorToInt(relativePos.z / cellSize);
    }

    private Vector3 GetWorldPosition(int x, int z)
    {
        return (new Vector3(x, 0f, z) * cellSize) + originPos;
    }

    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < length)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetGridPosition(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < length)
        {
            return gridArray[x, y];
        } 
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetGridPosition(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
