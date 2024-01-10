using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeController : MonoBehaviour
{ 
    private bool isGreenAreaVisible = false; // ��� GreenArea �Ƿ�ɼ�
    public GameObject nowShining;

    void Start()
    {
        // ��ʼʱ���� GreenArea
        //SetGreenAreaVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        // ��������
        if (Input.GetMouseButtonDown(0))
        {
            checkTowerCollider();
        }
    }


    //��Ŀ��TowerAI�Ŀɼ�����Ϊ����
    void SetGreenAreaVisibility(EmptyAI hitTower,bool visible)
    {
        // ���� GreenArea �Ŀɼ���
        if (hitTower.greenArea != null)
        {
            hitTower.SetGreenAreaVisibility(visible);
        }
    }


   public  void CloseAllGreenAreaVisibility()
    {
        EmptyAI[] allTurrets = FindObjectsOfType<EmptyAI>();
        foreach (EmptyAI turret in allTurrets)
        {
            turret.SetGreenAreaVisibility(false);
        }
    }

    public void checkTowerCollider()
    {
        int layerMask = 1 << LayerMask.NameToLayer("RangeControl");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

        if (hits.Length > 0)
        {
            EmptyAI tower = hits[0].collider.GetComponentInParent<EmptyAI>();
            if (tower != null)
            {
                
                if (tower.gameObject == nowShining)
                {
                    SetGreenAreaVisibility(tower, false);
                    nowShining = null;
                }
                else
                {
                    if (nowShining != null)
                    {
                        EmptyAI x = nowShining.GetComponent<EmptyAI>();
                        SetGreenAreaVisibility(x, false);
                    }                  
                    SetGreenAreaVisibility(tower, true);
                    nowShining = tower.gameObject;
                }
                
                
            }

            else//ʲô��û�㵽
            {         
                CloseAllGreenAreaVisibility();
                nowShining = null;
            }
        }
    }

}
