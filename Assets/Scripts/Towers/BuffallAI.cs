using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffallAI : EmptyAI
{
    void Start()
    {
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            TowerAI[] towerAIs = parentTransform.GetComponentsInChildren<TowerAI>();            
            foreach (TowerAI towerAI in towerAIs)
            {
                buff(towerAI);
            }

            BlazeAI[] BlazeAIs = parentTransform.GetComponentsInChildren<BlazeAI>();
            foreach (BlazeAI towerAI in BlazeAIs)
            {
                buff(towerAI);
            }
        }
         }

    public virtual void buff(TowerAI tower)
    {
        print("°¡£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿£¿");
    }

    public virtual void buff(BlazeAI tower)
    {

    }


}
