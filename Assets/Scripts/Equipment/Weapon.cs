using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public new EEquipmentType equipmentType = EEquipmentType.Weapon;

    public Collider damageCollider;

    public enum EDamageType { Crush, Slash, Pierce};
    // Using System.NonSerialized as you cannot serialize the same field multiple times in parent and child classes
    [System.NonSerialized] protected EDamageType damageType = EDamageType.Crush;

    [System.NonSerialized] public float damage = 10f;
    [System.NonSerialized] public new float cooldown = 1f;

    protected override void Start()
    {
        damageCollider = GetComponentInChildren<Collider>();    
    }

    public void ActivateDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DeactivateDamageCollider()
    {
        damageCollider.enabled = false;
    }
}
