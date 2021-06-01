using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Agent
{
    private float wanderDistance = 5f;
    private float wanderTimer = 0f;
    private float wanderTimerInterval = 2f;

    private Entity current_target;
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
        }
    }

    private void SearchForEnemy()
    {
        for(int i = 0; i < 10; i++)
        {

        }
    }

    private void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0)
        {
            // Using this method instead of insideUnitCircle because we want to randomize x and z instead of x and y.
            destination = new Vector3(Random.value, 0, Random.value) * wanderDistance;
            wanderTimer = wanderTimerInterval;
        }
    }
}
