using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryAI : MonoBehaviour
{
    private Transform baseTransform;
    private int siblingIndex;
    private GameObject downFx;
    private GameObject upFx;
    public bool isFinding = false;

    private void Start()
    {
        upFx = transform.GetChild(1).gameObject;
        downFx = transform.GetChild(2).gameObject;

        baseTransform = transform.parent;
        siblingIndex = transform.GetSiblingIndex();

        //Ѱ�ҷ�Hero��TowerAI
        TowerAI frontTower = baseTransform.GetChild(siblingIndex - 1).GetComponent<TowerAI>();
        
        if (baseTransform.GetChild(siblingIndex - 1).tag=="Hero")
            frontTower = baseTransform.GetChild(siblingIndex - 2).GetComponent<TowerAI>();
        if (frontTower != null)
        {
            frontTower.towerAtk += 5;
            downFx.SetActive(true);
        }

        //Ѱ��BlazeAI
        BlazeAI frontBlaze = baseTransform.GetChild(siblingIndex - 1).GetComponent<BlazeAI>();
        if (frontBlaze != null)
        {
            frontBlaze.attackDamage+= 5;
            downFx.SetActive(true);
        }


        isFinding = true;
        // ���÷�����ǰ������������ӹ�����
        //UpdatePreviousTowerAttack();
    }

    private void Update()
    {
        if(isFinding)
        FindingTower();
    }

    private void FindingTower()
    {
        siblingIndex = transform.GetSiblingIndex();
        int currentChildCount = baseTransform.childCount;
        if (currentChildCount != siblingIndex + 1)
        {

            Transform nextTower;
            nextTower= baseTransform.GetChild(siblingIndex + 1);

            if (nextTower != null&&nextTower.tag!="Hero")
            {
                TowerAI frontTower =nextTower.GetComponent<TowerAI>();
                if (frontTower != null)
                {
                    frontTower.towerAtk += 5;
                    upFx.SetActive(true);
                }
                BlazeAI frontBlaze = nextTower.GetComponent<BlazeAI>();
                if (frontBlaze != null)
                {
                    frontBlaze.attackDamage += 5;
                    upFx.SetActive(true);
                }
            }

            if(nextTower.tag != "Hero")
            isFinding = false;
        }
    }
}
