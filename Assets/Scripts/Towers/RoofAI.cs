using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofAI : TowerAI
{
    public float verticalSpeed = 2f;
    public float speed = 8;
    public float attackRadius = 5f;
    public override void LookTarget()
    {  
        times += Time.deltaTime;
        if (times >= attackSpeed / (1 + attackSpeedBuff))
        {

            if (CheckIfLastChild())
            {
                Attack();
                times = 0;
            }        
        }

    }


    private void Attack()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);

        //���ӵ��ҽű�
        bullet.AddComponent<roofBulletMove>().target = targetObject;
        bullet.GetComponent<roofBulletMove>().scripts = this;
        bullet.GetComponent<roofBulletMove>().verticalSpeed = verticalSpeed;
        //bullet.GetComponent<roofBulletMove>().gravity = gravity;
        bullet.GetComponent<roofBulletMove>().sSpeed =speed;
        bullet.GetComponent<roofBulletMove>().attackRadius = attackRadius;
        bullet.GetComponent<roofBulletMove>().atk = towerAtk + heroRobotIncrease;
      
        totalDamage += (towerAtk + heroRobotIncrease);
    }


    bool CheckIfLastChild()
    {
        // ��ȡ��ǰ����ĸ��ڵ�
        Transform parent = transform.parent;

        // ����Ƿ��и��ڵ�
        if (parent != null)
        {
            Transform[] siblings = new Transform[parent.childCount];
            for (int i = 0; i < parent.childCount; i++)
            {
                siblings[i] = parent.GetChild(i);
            }

            // ��ȡ��ǰ������ͬ���������е�����
            int index = System.Array.IndexOf(siblings, transform);


            if (index == siblings.Length - 1)
            {
                return true;
            }
            else
            {
                print("nonono"+index);
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
