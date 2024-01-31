using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySkill : MonoBehaviour
{
    public float maxMana = 100f;          // 最大法力值   
    protected float currentMana = 0f;    // 当前法力值
    public bool isManaRegenerating = true;  // 是否正在恢复法力值

    public Image manaBarFill;

    public void Start()
    {
        currentMana = 0f;
        isManaRegenerating = true;
      
    }

    public void Update()
    {
        RegenerateMana();
        UpdateHealthBarPosition();
        if (currentMana >= maxMana)
        {
            isManaRegenerating = false;  // 法力值满了停止恢复
        }
    }

    public void RegenerateMana()
    {
        if (isManaRegenerating)
        {
            currentMana += Time.deltaTime;
            //print(currentMana);

            if (currentMana > maxMana)
            {
                currentMana = maxMana;  
            }

            if (currentMana == maxMana)
            {
                CastSkill();
            }
        }
       
    }

    void UpdateHealthBarPosition()
    {
        float manaPercentage =currentMana/maxMana;
        manaBarFill.fillAmount = manaPercentage;
    }

    public void CastSkill()
    {
        if (currentMana >= 0f)
        {
            //isManaRegenerating = true;
            currentMana = 0f;
            PerformSkill();         
        }
    }

    protected virtual void PerformSkill()
    {
       
    }
}
