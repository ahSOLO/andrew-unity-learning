using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public enum EEquipmentType { Weapon, Armor, Misc };
    // Using System.NonSerialized as you cannot serialize the same field multiple times in parent and child classes
    [System.NonSerialized] public EEquipmentType equipmentType = EEquipmentType.Weapon;

    [System.NonSerialized] public float cooldown = 1f;

    protected virtual void Start()
    {
        
    }

}
