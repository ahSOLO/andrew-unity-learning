using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    protected new EDamageType damageType = EDamageType.Crush;
 
    public new float damage = 10f;
    public new float cooldown = 1f;

}
