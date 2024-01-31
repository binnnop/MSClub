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

    public bool abilityBlast = false;
    public bool abilityBurn = false;
    private CardManager cardManager;


    protected void Start()
    {
        dis =10000;
        targetObject = null;
        enemy = new List<GameObject>();
        model = transform.GetChild(0);
        model.forward *= -1;
        core = GameObject.Find("CORE");
        cardManager = GameObject.Find("Engine").GetComponent<CardManager>();

        if (transform.parent != null)
        {
            BuffallAI[] supportTurrets = transform.parent.GetComponentsInChildren<BuffallAI>();
            if (supportTurrets != null&&gameObject.tag!="Hero")
            {
                foreach (BuffallAI supportTurret in supportTurrets)
                {
                    supportTurret.buff(this);
                }
            }
        }
        
        
    }

 
    protected void LateUpdate()
    {
        //Debug.Log(enemy.Count);
        times += Time.deltaTime;
        enemy.RemoveAll(target => target == null || !target.activeSelf);

        for (int i = 0; i < enemy.Count; i++)
        {
            EnemyAI script = enemy[i].GetComponent<EnemyAI>();
            if (script.goDie)
            {
                enemy.Remove(enemy[i]);
            }

        }
        foreach (GameObject enemyObject in enemy)
        {
           
        }

        if (targetObject != null)
        {
            LookTarget();
        }
        //有目标时 索敌

        if (enemy.Count > 0)
        {
            GameObject closestEnemy = FindClosestEnemy();

            if (closestEnemy != null&&closestEnemy!=targetObject)
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
        
      
        
         if (times >= attackSpeed / (1 + attackSpeedBuff))
            {
            Attack();
            times = 0;
        }

    }


    public void Attack()
    {
        if (isGold)
        {
            cardManager.currentMoney += goldIncome;
            cardManager.UpdateMoneyText();
        }

        if (abilityBurn)
            bulletPrefab = Resources.Load("FireBullet") as GameObject;
        GameObject bullet = Instantiate(bulletPrefab,firePos.position,Quaternion.identity);

        //给子弹挂脚本
        EnemyAI enemy = targetObject.GetComponent<EnemyAI>();

        bullet.AddComponent<BulletMove>().target=targetObject;
        bullet.GetComponent<BulletMove>().scripts = this;
        bullet.GetComponent<BulletMove>().atk = towerAtk+heroRobotIncrease;
        bullet.GetComponent<BulletMove>().abilityBlast = abilityBlast;
        bullet.GetComponent<BulletMove>().abilityBurn = abilityBurn;
        bullet.transform.LookAt(targetObject.transform.position);

        enemy.voidHealth -= towerAtk;
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
           

            if (enemyTransform != null && enemyTransform.gameObject.activeSelf )
            {
                float distanceToCore = GetNavMeshPathDistance(enemyTransform);

                //bool hasObstacle = CheckObstacleBetweenTurretAndTarget(enemyTransform.transform);

                if (distanceToCore < closestDistance )
                {
                    closestDistance = distanceToCore;
                    closestEnemy = enemyTransform;
                }
            }



        }

        return closestEnemy;
    }

    public bool CheckObstacleBetweenTurretAndTarget(Transform targetTransform)
    {
        // 发射一条射线，检测炮塔和目标之间是否存在遮挡物
        Vector3 turretPosition = transform.position;
        Vector3 targetPosition = targetTransform.position;
        Vector3 direction = targetPosition - turretPosition;

        RaycastHit[] hits = Physics.RaycastAll(turretPosition, direction, Vector3.Distance(turretPosition,targetPosition));

        foreach (var hit in hits)
        {
            // 如果射线击中了遮挡物，并且该遮挡物带有"ground"标签，返回 true
            if (hit.collider.gameObject != targetTransform.gameObject && hit.collider.CompareTag("ground"))
            {
                return true;
            }
        }

        // 没有击中带有"ground"标签的遮挡物，返回 false
        return false;
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
            distance += Vector3.Distance(target.transform.position, path.corners[0]);
            distance += Vector3.Distance( path.corners[path.corners.Length-1],core.transform.position);

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
