using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Agent
{
    public Text health_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        health_text.text = $"Health : {health}";
    }
}
