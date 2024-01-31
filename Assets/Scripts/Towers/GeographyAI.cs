using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeographyAI : EmptyAI
{
    private TowerAI towerAI;
    private CardManager engine;
    public GameObject upFx;
    public GameObject downFx;
    public Base b;

    public int incomeSpeed = 4;
    public float moneyIncreaseInterval = 1f;
    public int atkBuff=10;

    public bool isHighest=false;
    public bool isLowest = false;

    void Start()
    {
        b = transform.parent.GetComponent<Base>();
        towerAI = GetComponent<TowerAI>();
        engine = GameObject.Find("Engine").GetComponent<CardManager>();
        InvokeRepeating("BombCoin", moneyIncreaseInterval, moneyIncreaseInterval);

    }

    void Update()
    {
        // 定期检查条件
        CheckFengShui();
    }


    void BombCoin()
    {
        if (isLowest)
        {
            engine.currentMoney += incomeSpeed;
            engine.UpdateMoneyText();
        }
        
    }


    void CheckFengShui()
    {
        EmptyAI[] emptyAIObjs = GameObject.FindObjectsOfType<EmptyAI>();

        if (emptyAIObjs.Length == 0)
        {
            // 没有其他EmptyAI物体，关闭TowerAI并返回
            if (towerAI != null)
                towerAI.enabled = false;

            return;
        }

        // 找到最低和最高的EmptyAI物体
        GameObject lowestEmptyAI = FindLowestEmptyAI(emptyAIObjs);
        GameObject highestEmptyAI = FindHighestEmptyAI(emptyAIObjs);

        // 判断是否是最低的EmptyAI
        isLowest = gameObject == lowestEmptyAI; 

        // 判断是否是最高的EmptyAI
        isHighest = gameObject == highestEmptyAI;

        // 根据条件进行处理
        if (isLowest)
        {
            
            downFx.SetActive(true);
        }
        else {

            downFx.SetActive(false);
        }

        // 处理TowerAI组件的开启和关闭
        if (isHighest)
        {
            // 是最高的EmptyAI，开启TowerAI组件
            if (towerAI != null)
            {
                if (!towerAI.enabled)
                {

                    towerAI.enabled = true;
                    towerAI.towerAtk += (b.cardCount - 1)*atkBuff;
                    upFx.SetActive(true);
                }
                
            }
               
        }
        else
        {
            // 不是最高的EmptyAI，关闭TowerAI组件
            if (towerAI != null&&towerAI.enabled)
            {
                towerAI.enabled = false;
                upFx.SetActive(false);
                towerAI.towerAtk = 0;
            }
                
        }
    }

    GameObject FindLowestEmptyAI(EmptyAI[] emptyAIObjs)
    {
        float lowestHeight = float.MaxValue;
        GameObject lowestEmptyAI = null;

        foreach (EmptyAI emptyAI in emptyAIObjs)
        {
           
                float height = emptyAI.gameObject. transform.position.y;

                if (height < lowestHeight)
                {
                    lowestHeight = height;
                    lowestEmptyAI = emptyAI.gameObject;
                }
         
        }

        return lowestEmptyAI;
    }

    GameObject FindHighestEmptyAI(EmptyAI[] emptyAIObjs)
    {
        float highestHeight = float.MinValue;
        GameObject highestEmptyAI = null;

        foreach (EmptyAI emptyAI in emptyAIObjs)
        {
           
                float height = emptyAI.gameObject.transform.position.y;

                if (height > highestHeight&& !emptyAI.gameObject.CompareTag("Hero"))
                {
                    highestHeight = height;
                    highestEmptyAI = emptyAI.gameObject;
                }
           
        }

        return highestEmptyAI;
    }
}
 