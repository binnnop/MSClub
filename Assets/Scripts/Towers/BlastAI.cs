using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastAI : EmptyAI
{
    private Transform baseTransform;
    private int siblingIndex;
    public bool isFinding = false;

    private void Start()
    {
        baseTransform = transform.parent;
        siblingIndex = transform.GetSiblingIndex();

        //寻找非Hero的TowerAI
        TowerAI frontTower = baseTransform.GetChild(siblingIndex - 2).GetComponent<TowerAI>();
        //print(baseTransform.GetChild(siblingIndex - 1));

        if (baseTransform.GetChild(siblingIndex - 2).tag == "Hero")
            frontTower = baseTransform.GetChild(siblingIndex - 3).GetComponent<TowerAI>();
        if (frontTower != null)
        {
            frontTower.abilityBlast = true;
        }
        else
            print("什么也没找到？？？");
        isFinding = true;

    }

    private void Update()
    {
        if (isFinding)
            FindingTower();
    }

    private void FindingTower()
    {
        siblingIndex = transform.GetSiblingIndex();
        int currentChildCount = baseTransform.childCount;
        if (currentChildCount != siblingIndex + 1)
        {

            Transform nextTower;
            nextTower = baseTransform.GetChild(siblingIndex + 1);

            if (nextTower != null && nextTower.tag != "Hero")
            {
                TowerAI frontTower = nextTower.GetComponent<TowerAI>();
                if (frontTower != null)
                {
                    frontTower.abilityBlast = true;
                }
            }

            if (nextTower.tag != "Hero" && !nextTower.gameObject.name.Contains("Model"))
                isFinding = false;
        }
    }
}
