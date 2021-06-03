using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEntity(GameObject spawnable, Vector3 position, Vector3 rotation = new Vector3())
    {
        SpawnEntity(spawnable, position, Quaternion.Euler(rotation));
        
    }

    public void SpawnEntity(GameObject spawnable, Vector3 position, Quaternion rotation)
    {
        Instantiate(spawnable, position, rotation);

    }
}
