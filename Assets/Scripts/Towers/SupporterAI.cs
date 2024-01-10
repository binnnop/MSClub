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
            // �ڸ��������ҵ����е� "TowerAI"
            TowerAI[] towerAIs = parentTransform.GetComponentsInChildren<TowerAI>();

            // �޸����� "TowerAI" �� AttackSpeed
            foreach (TowerAI towerAI in towerAIs)
            {
                towerAI.ModifyAttackSpeed(buffValue);
            }

            BlazeAI[] blazeAIs = parentTransform.GetComponentsInChildren<BlazeAI>();

            // �޸����� "TowerAI" �� AttackSpeed
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


