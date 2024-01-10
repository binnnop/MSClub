using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupporterAI : MonoBehaviour
{
    public float buffValue;
    // Start is called before the first frame update
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
                towerAI.ModifyAttackSpeed(buffValue);
            }

            BlazeAI[] blazeAIs = parentTransform.GetComponentsInChildren<BlazeAI>();

            // 修改所有 "TowerAI" 的 AttackSpeed
            foreach (BlazeAI blaze in blazeAIs)
            {
               blaze.ModifyAttackSpeed(buffValue);
            }

        }
        else
        {
            Debug.LogError("SupportTurret has no parent.");
        }
    }
}


