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

    protected Vector3 destination;
    protected Vector3 lastDestination;
    List<PathNode> path;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        destination = transform.position;
        lastDestination = Vector3.zero;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); 
        if(current_target == null)
        {
            Wander();
            //SearchForEnemy();
            SearchForPlayer(110, 10);
        }
        else
        {
            FollowPlayer();
        }
    }

    private void FixedUpdate()
    {
        // Convert destination into a move and turn direction
        var diff = destination - transform.position;
        if (diff.sqrMagnitude > 1f)
        {
            var direction = diff.normalized;
            Turn(direction);
            Move(direction);
        }
    }

    private void SearchForPlayer(float FOV, int rays)
    {
        var start = -FOV/2;
        if(view_stage == EViewConeStage.A)
        {
            start = -FOV / 2;
        }
        else if(view_stage == EViewConeStage.B)
        {
            start = -FOV / 2 + FOV / rays * 0.25f;
        }
        else if(view_stage == EViewConeStage.C)
        {
            start = -FOV / 2 + FOV / rays * 0.5f;
        }
        else
        {
            start = -FOV / 2 + FOV / rays * 0.75f;
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
        var sqrDist = Vector3.SqrMagnitude(current_target.transform.position - transform.position);
        if (sqrDist < 4f)
        {
            destination = current_target.transform.position;
            rightHandAction();
        } 
        else
        {
            var navGrid = ServiceLocator.Get<NavGrid>();
            if ((lastDestination - current_target.transform.position).sqrMagnitude >= 4f)
            {
                path = navGrid.FindPath(transform.position, current_target.transform.position);
                lastDestination = current_target.transform.position;
                Debug.Log("Debug One");
            }
            if (path != null)
            {
                Debug.Log(path.Count);
                var closestNodePos = navGrid.grid.GetWorldPosition(path[0].x, path[0].z);
                if ((closestNodePos - transform.position).sqrMagnitude < 1.8f)
                {
                    path.RemoveAt(0);
                    closestNodePos = navGrid.grid.GetWorldPosition(path[0].x, path[0].z);
                } 
                destination = closestNodePos;
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
