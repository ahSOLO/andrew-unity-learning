﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Agent
{
    private Entity current_target;

    private float wanderDistance = 8f;
    private float wanderTimer = 0f;
    private float wanderTimerInterval = 2f;

    private float visionDistance = 10f;

    EViewConeStage view_stage = EViewConeStage.A;


    private float last_attack_time = 0;
    private float attack_delay = 1;
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
            //SearchForEnemy();
            OptimizedSearchForPlayer(110, 10);
        }
        else
        {
            FollowPlayer();
        }
    }

    private void FixedUpdate()
    {
        var diff = destination - transform.position;
        var direction = diff.normalized;
        Turn(direction);
        if (diff.sqrMagnitude > 2)
            Move(direction);
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
            if (Physics.Raycast(transform.position, Quaternion.Euler(0f, rotation, 0f) * transform.forward, out hit, visionDistance))
            {
                if (hit.collider && hit.collider.tag == "Player")
                {
                    Debug.Log("Player Found");
                    current_target = hit.transform.GetComponent<Entity>();
                }
            }
            Debug.DrawLine(transform.position, transform.position + Quaternion.Euler(0f, rotation, 0f) * transform.forward * visionDistance, Color.red);


        }
    }


    private void OptimizedSearchForPlayer(float FOV, int rays)
    {
        var start = -FOV/2;
        if(view_stage == EViewConeStage.A)
        {
            start = -FOV / 2;
            Debug.Log("A");
        }
        else if(view_stage == EViewConeStage.B)
        {
            start = -FOV / 2 + FOV / rays * 0.25f;
            Debug.Log("B");

        }
        else if(view_stage == EViewConeStage.C)
        {
            start = -FOV / 2 + FOV / rays * 0.5f;
            Debug.Log("C");
        }
        else
        {
            start = -FOV / 2 + FOV / rays * 0.75f;
            Debug.Log("D");

        }



        for (int i = 0; i < rays; i++)
        {
            if (view_stage != EViewConeStage.A && i == rays - 1)
            {
                break;
            }
            RaycastHit hit;
            var rotation = start + FOV * (i + 1) / rays;
            if (Physics.Raycast(transform.position, Quaternion.Euler(0f, rotation, 0f) * transform.forward, out hit, visionDistance))
            {
                if (hit.collider && hit.collider.tag == "Player")
                {
                    Debug.Log("Player Found");
                    current_target = hit.transform.GetComponent<Entity>();
                }
            }
            Debug.DrawLine(transform.position, transform.position + Quaternion.Euler(0f, rotation, 0f) * transform.forward * visionDistance, Color.red);


        }
        if(view_stage == EViewConeStage.D)
        {
            view_stage = EViewConeStage.A;
        }
        else
        {
            view_stage++;
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

    private void FollowPlayer()
    {
        destination = current_target.transform.position;
        if(Vector3.Distance(current_target.transform.position, transform.position) < 2f)
        {
            if(Time.timeSinceLevelLoad > last_attack_time + attack_delay)
            {
                current_target.TakeDamage(10);
                last_attack_time = Time.timeSinceLevelLoad;
            }
        }
    }

    enum EViewConeStage
    {
        A,
        B,
        C,
        D
    }

}
