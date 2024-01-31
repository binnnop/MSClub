using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySkill : MonoBehaviour
{
    public float maxMana = 100f;          // �����ֵ   
    protected float currentMana = 0f;    // ��ǰ����ֵ
    public bool isManaRegenerating = true;  // �Ƿ����ڻָ�����ֵ

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
            isManaRegenerating = false;  // ����ֵ����ֹͣ�ָ�
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
