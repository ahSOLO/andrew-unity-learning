using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Entity
{
    protected float movement_speed = 4f;
    protected float turn_speed = 360f;

    public Equipment rightHandEquip;
    public Animator rightHandEquipAnim;
    public float rightHandCooldownTimer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        rightHandCooldownTimer -= Time.deltaTime;
        if (health == 0)
        {
            Death();
        }
    }

    protected void Move(Vector3 direction)
    {
        // Option 1: Move in the given direction
        // Do not move vertically
        var moveDir = new Vector3(direction.x, 0f, direction.z) * movement_speed * Time.fixedDeltaTime;
        transform.Translate(moveDir, Space.World);

        // Option 2: Always move forward -- rely on the Turn function for rotations
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

    protected void rightHandAction()
    {
        if (rightHandCooldownTimer <= 0 && rightHandEquip.equipmentType == Equipment.EEquipmentType.Weapon)
        {
            Attack(rightHandEquip, rightHandEquipAnim);
            rightHandCooldownTimer = rightHandEquip.cooldown;
        }
    }

    protected void Attack(Equipment weapon, Animator anim)
    {
        anim.SetTrigger("Attack");
    }

    public virtual void Death()
    {
        GameManager.SpawnEntity(GameManager.gM.deathParticle, transform.position);
        Destroy(gameObject);
    }
}
