using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiantAI : EmptyAI
{
    private Transform baseTransform;
    private int siblingIndex;
    public int buffValue;

    private void Start()
    {
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            TowerAI[] towerAIs = parentTransform.GetComponentsInChildren<TowerAI>();
            foreach (TowerAI towerAI in towerAIs)
            {
                towerAI.towerAtk += buffValue;
            }

            BlazeAI[] BlazeAIs = parentTransform.GetComponentsInChildren<BlazeAI>();
            foreach (BlazeAI towerAI in BlazeAIs)
            {
                towerAI.attackDamage += buffValue;
            }
        }
    }

}