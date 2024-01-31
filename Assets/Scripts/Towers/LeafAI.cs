using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafAI : TowerAI
{
   public int level=1;
    public float multiShootInterval;


    public override void LookTarget()
    {
        //print(model.forward);
        Vector3 pos = targetObject.transform.position;
        pos.y = model.transform.position.y;
        model.transform.LookAt(pos);


        times += Time.deltaTime;
        if (times >= attackSpeed / (1 + attackSpeedBuff))
        {
            Attack();
            if(level==2)
            Invoke("Attack", multiShootInterval);
            if (level == 3)
            {
                Invoke("Attack", multiShootInterval);
                Invoke("Attack", multiShootInterval*2);
                Invoke("Attack", multiShootInterval*3);
            }
            times = 0;
        }

    }


    
}
