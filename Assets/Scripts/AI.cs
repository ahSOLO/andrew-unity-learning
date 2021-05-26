using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Agent
{

    private Entity current_target;
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
        }
    }

    private void SearchForEnemy()
    {
        for(int i = 0; i < 10; i++)
        {

        }
    }

    private void Wander()
    {
        
    }
}
