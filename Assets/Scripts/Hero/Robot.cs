using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Robot : MonoBehaviour
{
    private TowerAI self;
    public int attackBuff;
    public int finalBuffValue;

    void Start()
    {
        self = GetComponent<TowerAI>();
    }

    
    void Update()
    {
        Base local = transform.parent.GetComponent<Base>();
        finalBuffValue = (local.cardCount + local.extraCount)*attackBuff;
        self.heroRobotIncrease = finalBuffValue;
    }
}
