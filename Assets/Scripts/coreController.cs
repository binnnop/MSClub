using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coreController : MonoBehaviour

   
{
    public int maxHP;
    public int nowHP;
    public TextMeshProUGUI HPbar;
    private test maybeTest;

    void Start()
    {
        nowHP = maxHP;
        maybeTest = GameObject.Find("test").GetComponent<test>();
        if (maybeTest.isTestMode && maybeTest.life999)
        {
            nowHP = 1000;
        }

    }

    void Update()
    {
        HPbar.text = nowHP.ToString();
    }

    public void damaged(int damage)
    {
        nowHP -= damage;
    }
}
