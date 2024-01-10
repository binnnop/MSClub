using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeController : MonoBehaviour
{ 
    private bool isGreenAreaVisible = false; // 标记 GreenArea 是否可见
    public GameObject nowShining;

    void Start()
    {
        // 初始时隐藏 GreenArea
        //SetGreenAreaVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0))
        {
            checkTowerCollider();
        }
    }


    //将目标TowerAI的可见性设为正或负
    void SetGreenAreaVisibility(EmptyAI hitTower,bool visible)
    {
        // 设置 GreenArea 的可见性
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

            else//什么都没点到
            {         
                CloseAllGreenAreaVisibility();
                nowShining = null;
            }
        }
    }

}
