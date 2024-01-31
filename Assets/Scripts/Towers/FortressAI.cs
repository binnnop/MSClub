using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressAI : EmptyAI
{
    public float moneyIncreaseInterval = 1f;
    public int amount = 1;
    private CardManager cardManager;

    void Start()
    {
        cardManager = GameObject.Find("Engine").GetComponent<CardManager>();
        InvokeRepeating("MoneyIncrease", moneyIncreaseInterval, moneyIncreaseInterval);
    }

    void MoneyIncrease()
    {
        cardManager.currentMoney += amount;
        //cardManager.UpdateMoneyText();
    }
}
