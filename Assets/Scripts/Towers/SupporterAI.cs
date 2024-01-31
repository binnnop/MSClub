using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupporterAI : BuffallAI
{
    public float buffValue;
    
     void Start()
    {
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            // 在父物体中找到所有的 "TowerAI"
            TowerAI[] towerAIs = parentTransform.GetComponentsInChildren<TowerAI>();

            // 修改所有 "TowerAI" 的 AttackSpeed
            foreach (TowerAI towerAI in towerAIs)
            {
                buff(towerAI);
            }

            BlazeAI[] blazeAIs = parentTransform.GetComponentsInChildren<BlazeAI>();

            // 修改所有 "TowerAI" 的 AttackSpeed
            foreach (BlazeAI blaze in blazeAIs)
            {
                buff(blaze);
            }

        }
        else
        {
            Debug.LogError("SupportTurret has no parent.");
        }
    }

    public override void buff(TowerAI tower)
    {
        tower.ModifyAttackSpeed(buffValue);
    }

    public override void buff(BlazeAI tower)
    {
        tower.ModifyAttackSpeed(buffValue);
    }

}


