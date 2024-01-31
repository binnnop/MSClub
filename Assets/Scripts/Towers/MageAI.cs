using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MageAI : TowerAI
{
    

    new void Start()
    {
        dis = 10000;
        targetObject = null;
        enemy = new List<GameObject>();
        model = transform.GetChild(0);
        firePos = model.transform.GetChild(0);
        bulletPrefab = Resources.Load<GameObject>("MageBullet");
        core = GameObject.Find("CORE");

        BuffallAI[] supportTurrets = transform.parent.GetComponentsInChildren<BuffallAI>();
        foreach (BuffallAI supportTurret in supportTurrets)
        {
            supportTurret.buff(this);
        }
    }


    new void LateUpdate()
    {
        //Debug.Log(enemy.Count);

        enemy.RemoveAll(target => target == null || !target.activeSelf);

        if (targetObject != null)
        {
            LookTarget();
        }
        //有目标时 索敌

        else if (enemy.Count > 0)
        {
            GameObject closestEnemy = FindClosestEnemy();

            if (closestEnemy != null)
            {
                targetObject = closestEnemy;
                LookTarget();
            }
        }
        }


       new GameObject SelectTarget()
    {
        GameObject temp = null;
        float distance;

        for (int i = 0; i < enemy.Count; i++)
        {
            distance = Vector3.Distance(transform.position, enemy[i].transform.position);
            if (distance <= dis)
            {
                dis = distance;
                temp = enemy[i];
                //Debug.Log(temp+dis.ToString());

            }
            //else
            //Debug.Log(i+"       distance is big:"+distance);
        }
        

        return temp;

    }

    new void LookTarget()
    {
        Vector3 pos = targetObject.transform.position;
        pos.y = model.transform.position.y;
        model.transform.LookAt(pos);
        times += Time.deltaTime;
        if (times >= attackSpeed / (1 + attackSpeedBuff))
        {
            Attack();
            times = 0;
        }

    }
    /*
    public void OnTriggerEnter(Collider other)  //检测怪物进入攻击范围
    {
        Debug.Log(other + "Enter");
        if (other.tag == "Enemy")
        {

            if (!enemy.Contains(other.gameObject))
            {
                enemy.Add(other.gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log(other + "Leave");
        if (other.tag == "Enemy")
        {
            if (enemy.Contains(other.gameObject))
            {
                //已在列表时：如果目标未死 && 是正在打的目标，就让target消失
                if (targetObject != null && other.name == targetObject.name) targetObject = null;
                enemy.Remove(other.gameObject);//从列表移除
                                               // Debug.Log(other+"Removed");
            }
        }
    }
    */

    private new void Attack()
    {
        if (abilityBurn)
            bulletPrefab = Resources.Load("FireBullet") as GameObject;
        GameObject bullet = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
        
        //给子弹挂脚本
        bullet.AddComponent<BulletMove>().target = targetObject;
        bullet.GetComponent<BulletMove>().scripts = this;
        bullet.GetComponent<BulletMove>().atk = towerAtk;
        bullet.GetComponent<BulletMove>().abilityBlast = abilityBlast;
        bullet.GetComponent<BulletMove>().abilityBurn = abilityBurn;
        bullet.transform.LookAt(targetObject.transform.position);
        totalDamage += (towerAtk + heroRobotIncrease);

        if (enemy.Count>1)
        {
            GameObject bullet2 = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);

            //给子弹挂脚本
            GameObject enemy2 = MageFindClosestEnemies(2)[1];

            bullet2.AddComponent<BulletMove>().target = enemy2;
            bullet2.GetComponent<BulletMove>().scripts = this;
            bullet2.GetComponent<BulletMove>().atk = towerAtk;
            bullet2.GetComponent<BulletMove>().abilityBlast = abilityBlast;
            bullet2.GetComponent<BulletMove>().abilityBurn = abilityBurn;
            bullet2.transform.LookAt(enemy2.transform.position);
            totalDamage += (towerAtk + heroRobotIncrease);


            if (enemy.Count > 2)
            {
                GameObject bullet3 = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);

                //给子弹挂脚本
                GameObject enemy3 = MageFindClosestEnemies(3)[2];

                //print("三个都找到了 1." + GetNavMeshPathDistance(targetObject) + "    2." + GetNavMeshPathDistance(enemy2) + "    3." + GetNavMeshPathDistance(enemy3));

                bullet3.AddComponent<BulletMove>().target = enemy3;
                bullet3.GetComponent<BulletMove>().scripts = this;
                bullet3.GetComponent<BulletMove>().atk = towerAtk;
                bullet3.GetComponent<BulletMove>().abilityBlast = abilityBlast;
                bullet3.GetComponent<BulletMove>().abilityBurn = abilityBurn;
                bullet3.transform.LookAt(enemy3.transform.position);
                totalDamage += (towerAtk + heroRobotIncrease);

            }

        }

       
    }


    List<GameObject> MageFindClosestEnemies(int n)
    {
        List<GameObject> closestEnemies = new List<GameObject>();

        foreach (var enemyTransform in enemy)
        {
            if (enemyTransform != null && enemyTransform.gameObject.activeSelf)
            {
                //bool hasObstacle = CheckObstacleBetweenTurretAndTarget(enemyTransform.transform);
                
                    float navMeshPathDistance = GetNavMeshPathDistance(enemyTransform);

                    if (closestEnemies.Count < n)
                    {
                        closestEnemies.Add(enemyTransform);
                    }
                    else
                    {
                        closestEnemies.Sort((a, b) => GetNavMeshPathDistance(a)
                                                      .CompareTo(GetNavMeshPathDistance(b)));

                        float farthestDistance = GetNavMeshPathDistance(closestEnemies[n - 1]);

                        if (navMeshPathDistance < farthestDistance)
                        {
                            closestEnemies.RemoveAt(n - 1);
                            closestEnemies.Add(enemyTransform);
                        }
                    }


               
                   
            }
        }

        return closestEnemies;
    }



  
}
