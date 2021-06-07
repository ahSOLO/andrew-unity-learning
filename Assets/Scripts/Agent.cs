using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Entity
{
    protected float movement_speed = 4f;
    protected float turn_speed = 270f;

    protected Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Move(Vector3 direction)
    {
        // Do not move vertically
        var moveDir = new Vector3(direction.x, 0f, direction.z) * movement_speed * Time.fixedDeltaTime;
        transform.Translate(moveDir, Space.World);

        //transform.Translate(transform.forward * movement_speed * Time.fixedDeltaTime, Space.World);
    }

    protected void Turn(Vector3 direction)
    {        
        var rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            // Only rotate on the Y axis
            Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, Quaternion.LookRotation(direction).eulerAngles.y, turn_speed * Time.fixedDeltaTime), 
            transform.rotation.eulerAngles.z
            );
        
        transform.rotation = rotation;
    }

    protected void UseItem()
    {

    }

    protected void Attack()
    {

    }

}
