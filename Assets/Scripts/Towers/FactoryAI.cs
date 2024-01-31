using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryAI : EmptyAI
{
    
    public float moneyIncreaseInterval = 5f;
    public int amount = 1;
    public float generateTowerInterval = 60f;
    private Base home;
    private CardManager cardManager;

    void Start()
    {
        Transform parentTransform = transform.parent;
        home = parentTransform.GetComponent<Base>();
        cardManager = GameObject.Find("Engine").GetComponent<CardManager>();
        InvokeRepeating("MoneyIncrease", moneyIncreaseInterval, moneyIncreaseInterval);
        InvokeRepeating("GenerateTower", generateTowerInterval, generateTowerInterval);
    }

    void MoneyIncrease()
    {
        cardManager.currentMoney += amount;
        cardManager.UpdateMoneyText();
    }

    void GenerateTower()
    {
        if (home != null&& home.cardCount<cardManager.maxLayer)
        {
            home.GenerateBuilding("Factory",0);
        }
    }
}
