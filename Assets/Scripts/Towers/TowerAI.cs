using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.AI;


public class TowerAI : EmptyAI
{

    public List<GameObject>  enemy;

    public GameObject core;
    public GameObject targetObject;
    public float dis;
    public float times=0;
    public GameObject bulletPrefab;
    public Transform firePos;
    
    public EnemyAI checkHaveOrNot;


    public float attackSpeed;
    public float attackSpeedBuff=0;
    public float rotateSpeed;
    public int towerAtk=0;
    public int heroRobotIncrease=0;
    //可以旋转的部位

    

    public bool isGold = false;
    public int goldIncome = 0;
    public int totalDamage=0;
    public bool canAttackFly=true;
    

    protected void Start()
    {
        dis =10000;
        targetObject = null;
        enemy = new List<GameObject>();
        model = transform.GetChild(0);
        model.forward *= -1;
        core = GameObject.Find("CORE");


        if (transform.parent != null)
        {
            SupporterAI[] supportTurrets = transform.parent.GetComponentsInChildren<SupporterAI>();
            if (supportTurrets != null)
            {
                foreach (SupporterAI supportTurret in supportTurrets)
                {
                    ModifyAttackSpeed(supportTurret.buffValue);
                }
            }
        }
        
        
    }

 
    protected void LateUpdate()
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

    public void OnTriggerEnter(Collider other)  //检测怪物进入攻击范围
    {
      
        if (other.tag == "Enemy")
        {
          
            if (!enemy.Contains(other.gameObject))
            {
                EnemyAI i = other.gameObject.GetComponent<EnemyAI>();
                if (!i.fallOut)
                {
                    if(!(!canAttackFly&&i.isFlying))
                    enemy.Add(other.gameObject);
                }
                
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
       
        if (other.tag == "Enemy")
        {
            if (enemy.Contains(other.gameObject))
            {
                //已在列表时：如果目标未死 && 是正在打的目标，就让target消失
                if (targetObject!=null && other.name == targetObject.name) targetObject = null;
                enemy.Remove(other.gameObject);//从列表移除
            
            }
        }
    }

    public  GameObject SelectTarget()
    {
        GameObject temp = null;
        float distance ;
        
        for (int i = 0; i < enemy.Count; i++)
        {
            distance = Vector3.Distance(transform.position, enemy[i].transform.position);
            if (distance <= dis)
            {
                dis = distance;
                temp = enemy[i];
            

            }
          
        }
        

        return temp;

    }

    public virtual void LookTarget()
    {
        //print(model.forward);
        Vector3 pos = targetObject.transform.position;
        pos.y =model.transform.position.y;
        model.transform.LookAt(pos);
        
      
        times += Time.deltaTime;
         if (times >= attackSpeed / (1 + attackSpeedBuff))
            {
            Attack();
            times = 0;
        }

    }


    private void Attack()
    {

        GameObject bullet = Instantiate(bulletPrefab,firePos.position,Quaternion.identity);
       
        //给子弹挂脚本
        bullet.AddComponent<BulletMove>().target=targetObject;
        bullet.GetComponent<BulletMove>().scripts = this;
        bullet.GetComponent<BulletMove>().atk = towerAtk+heroRobotIncrease;
        bullet.transform.LookAt(targetObject.transform.position);
        totalDamage += (towerAtk + heroRobotIncrease);
    }

    public void ModifyAttackSpeed(float multiplier)
    {
        attackSpeedBuff += multiplier;
    }

   

    public GameObject  FindClosestEnemy()
    {
       GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var enemyTransform in enemy)
        {
            if (enemyTransform != null && enemyTransform.gameObject.activeSelf)
            {
                float distanceToCore = GetNavMeshPathDistance(enemyTransform);

                if (distanceToCore < closestDistance)
                {
                    closestDistance = distanceToCore;
                    closestEnemy = enemyTransform;
                }
            }
        }

        return closestEnemy;
    }

    public float GetNavMeshPathDistance(GameObject target)
    {

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(core.transform.position, target.transform.position, NavMesh.AllAreas, path))
        {
            float distance = 0f;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return distance;
        }

        EnemyAI tryAI;
        tryAI = target.GetComponent<EnemyAI>();
        if (tryAI != null&&tryAI.verticalFlying)
        {
            return 0.1f;
        }


        return Vector3.Distance(core.transform.position, target.transform.position); 
    }
}
