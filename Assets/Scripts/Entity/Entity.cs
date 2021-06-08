using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float health;
    [SerializeField] protected float maxHealth = 100f;
    protected EFaction faction = EFaction.neutral;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            // This feels like an expensive call -- any way I can prevent it?
            TakeDamage(other.GetComponentInParent<Weapon>().damage);
        }
    }

    public virtual void TakeDamage(float dmg)
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
