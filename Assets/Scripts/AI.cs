using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Agent
{
    private Entity current_target;

    private float wanderDistance = 8f;
    private float wanderTimer = 0f;
    private float wanderTimerInterval = 2f;

    private float visionDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(current_target == null)
        {
            Wander();
            SearchForEnemy();
        }
    }

    private void SearchForEnemy()
    {
        var start = -55f;
        var end = 55f;
        var rays = 10;

        for(int i = 0; i < rays; i++)
        {
            RaycastHit hit;
            var rotation = start + (end - start) * (i + 1) / rays;
            Physics.Raycast(transform.position, Quaternion.Euler(0f, rotation, 0f) * transform.forward, out hit, visionDistance);

            if (hit.collider && hit.collider.tag == "Player")
            {
                Debug.Log("Player Found");
            }
        }
    }

    private void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0)
        {
            // Using this method instead of insideUnitCircle because we want to randomize x and z instead of x and y.
            destination = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * wanderDistance;
            wanderTimer = wanderTimerInterval;
        }
    }
}
