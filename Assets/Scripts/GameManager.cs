using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gM; // Singleton Var
    public GameObject enemy;
    public float lastSpawnTime;

    private void OnEnable()
    {
        // Instantiate Singleton
        if (gM == null)
        {
            gM = this;
        }
        else
        {
            // Setting to inactive because destruction doesn't occur until the end of the frame.
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject SpawnEntityInArea(GameObject spawnable, Vector3 origin, float radius, Vector3 rotation = new Vector3())
    {
        // Get approximate height and width of spawnable
        var spawnableCol = spawnable.GetComponent<Collider>();
        var spawnableHeight = spawnableCol.bounds.max.y;
        var spawnableWidth = Mathf.Max(spawnableCol.bounds.max.x, spawnableCol.bounds.max.z);

        int checkCount = 0;

        while (checkCount < 20)
        {
            // Set spawn coords to random point on a circle drawn on the floor
            var randomPointInCircle = Random.insideUnitCircle * radius;
            var spawnCoords = origin + new Vector3(randomPointInCircle.x, 0, randomPointInCircle.y);

            // Cast a capsule the approx height and width of the spawnable
            var capsuleTop = new Vector3(spawnCoords.x, spawnable.transform.position.y + spawnableHeight / 2, spawnCoords.z);
            var capsuleRadius = spawnableWidth / 2;

            if (Physics.CheckCapsule(capsuleTop, spawnCoords, capsuleRadius) == false)
            {
                return SpawnEntity(spawnable, spawnCoords, rotation);
            }
            else
            {
                checkCount++;
            }
        }

        Debug.Log("Could not find a valid spawn location");
        return null;
    }

    public static GameObject SpawnEntity(GameObject spawnable, Vector3 position, Vector3 rotation = new Vector3())
    {
        return SpawnEntity(spawnable, position, Quaternion.Euler(rotation));
    }

    public static GameObject SpawnEntity(GameObject spawnable, Vector3 position, Quaternion rotation)
    {
        GameManager.gM.lastSpawnTime = Time.timeSinceLevelLoad;
        return Instantiate(spawnable, position, rotation);
    }
}
