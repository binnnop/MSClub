using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnAI : BuffallAI
{
    void Start()
    {
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {

            TowerAI[] towerAIs = parentTransform.GetComponentsInChildren<TowerAI>();
            foreach (TowerAI towerAI in towerAIs)
            {
                if(towerAI.gameObject.tag!="Hero")
                buff(towerAI);
            }


        }
    }


    public override void buff (TowerAI tower)
     {
        tower.abilityBurn = true;
        //print("ßÀßÀ                                             ßÀßÀ");
     }


    }