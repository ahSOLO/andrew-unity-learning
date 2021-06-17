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
        public int z;
    }
    
    private int width;
    private int length;
    private float cellSize;
    public Vector3 originPos;
    private TGridObject[,] gridArray;

    public Grid(int width, int length, float cellSize, Vector3 originPos, Func<int, int, TGridObject> createGridObject)
    {
        GameObject[,] textObjs = new GameObject[width, length];
        this.width = width;
        this.length = length;
        this.cellSize = cellSize;
        this.originPos = originPos;

        gridArray = new TGridObject[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                gridArray[x, z] = createGridObject(x, z);
                
                // Debug
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 100f);
                
                //var textObj = new GameObject(x.ToString() + z.ToString());
                //textObj.AddComponent<TextMesh>().text = gridArray[x, z].ToString();
                //textObj.transform.parent = GameObject.FindGameObjectWithTag("TextObjs").transform;
                //textObj.transform.position = GetWorldPosition(x, z) + new Vector3(cellSize / 2, 0f, cellSize / 2);
                //textObj.GetComponent<TextMesh>().fontSize = 20;
                //textObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //textObj.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                //textObjs[x, z] = textObj;
            }
        }

        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
            //var textMesh = textObjs[eventArgs.x, eventArgs.z].GetComponent<TextMesh>();
            //textMesh.text = gridArray[eventArgs.x, eventArgs.z].ToString();
            //textMesh.color = Color.red;
        };
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

    public void GetGridPosition(Vector3 worldPos, out int x, out int z)
    {
        var relativePos = worldPos - originPos;
        x = Mathf.FloorToInt(relativePos.x / cellSize);
        z = Mathf.FloorToInt(relativePos.z / cellSize);
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return (new Vector3(x, 0f, z) * cellSize) + originPos;
    }

    public void SetValue(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < length)
        {
            gridArray[x, z] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, z = z });
        }
    }

    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, z;
        GetGridPosition(worldPosition, out x, out z);
        SetValue(x, z, value);
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public TGridObject GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < length)
        {
            return gridArray[x, z];
        } 
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetGridPosition(worldPosition, out x, out z);
        return GetValue(x, z);
    }
}
