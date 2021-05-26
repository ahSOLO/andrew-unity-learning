using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float health = 100;
    protected EFaction faction = EFaction.neutral;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if(health < 0)
        {
            health = 0;
        }
    }

    public enum EFaction
    {
        player,
        enemy,
        neutral
    }
}
