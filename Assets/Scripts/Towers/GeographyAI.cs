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
        // ���ڼ������
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
            // û������EmptyAI���壬�ر�TowerAI������
            if (towerAI != null)
                towerAI.enabled = false;

            return;
        }

        // �ҵ���ͺ���ߵ�EmptyAI����
        GameObject lowestEmptyAI = FindLowestEmptyAI(emptyAIObjs);
        GameObject highestEmptyAI = FindHighestEmptyAI(emptyAIObjs);

        // �ж��Ƿ�����͵�EmptyAI
        isLowest = gameObject == lowestEmptyAI; 

        // �ж��Ƿ�����ߵ�EmptyAI
        isHighest = gameObject == highestEmptyAI;

        // �����������д���
        if (isLowest)
        {
            
            downFx.SetActive(true);
        }
        else {

            downFx.SetActive(false);
        }

        // ����TowerAI����Ŀ����͹ر�
        if (isHighest)
        {
            // ����ߵ�EmptyAI������TowerAI���
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
            // ������ߵ�EmptyAI���ر�TowerAI���
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
 