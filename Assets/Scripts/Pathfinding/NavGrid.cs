using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    Grid<int> grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<int>(100, 100, 1, -50, -50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
