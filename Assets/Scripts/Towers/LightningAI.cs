using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAI : EmptyAI
{
    public int damage = 50;
    public float range = 5f;
    public LineRenderer lightningRenderer;
    public GameObject allLight;

    private GameObject tempLine;

    void Start()
    {
        if (lightningRenderer == null)
        {
            lightningRenderer = GetComponent<LineRenderer>();
        }

        if (lightningRenderer != null)
        {
            lightningRenderer.enabled = false;

            // 在Start中获取范围内的电塔
            Collider[] colliders = GetTowersInRange();

            foreach (Collider collider in colliders)
            {
                LightningAI i;
                i = collider.gameObject.GetComponent<LightningAI>();
                if(i!=null&&collider.gameObject!=this.gameObject)
                    ConnectToLightningTower(collider.transform);
                
            }
        }
    }

    Collider[] GetTowersInRange()
    {
    
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(range, range, range) / 2f);
        return colliders;
    }

    void ConnectToLightningTower(Transform target)
    {
        // 获取目标物体上的 LightningTower 组件
        LightningAI targetTower = target.GetComponent<LightningAI>();
        print("connected!"+"    "+targetTower);
        if (targetTower != null)
        {
            CreateLightning(target.position);
            CreateCollisionBetweenStartAndEnd(target.position,targetTower,tempLine);
            tempLine = null;
        }
    }

    void CreateLightning(Vector3 targetPosition)
    {
        // 使用 Resources.Load 加载 LightningBolt 预制体
        GameObject lightningBoltPrefab = Resources.Load<GameObject>("LightningBoltPrefab");

        // 射线检测
        
            GameObject lightningBolt = Instantiate(lightningBoltPrefab, transform);
            tempLine = lightningBolt;
            lightningBolt.transform.parent = allLight.transform;

            Transform lightningStart = lightningBolt.transform.Find("LightningStart");
            Transform lightningEnd = lightningBolt.transform.Find("LightningEnd");

            if (lightningStart != null && lightningEnd != null)
            {
                lightningStart.position = transform.position;
                lightningEnd.position = targetPosition;
            }
           
       
      
    }

    void CreateCollisionBetweenStartAndEnd(Vector3 targetPosition, LightningAI targetTower,GameObject targetLine)
    {
        // 在 LightningStart 和 LightningEnd 之间生成一个碰撞体
        GameObject collisionPrefab = Resources.Load<GameObject>("CollisionPrefab");

        if (collisionPrefab != null)
        {
            GameObject collision = Instantiate(collisionPrefab, allLight.transform);
            // 设置碰撞体的位置
            collision.transform.position = (transform.position + targetPosition) / 2f;
            collision.transform.LookAt(transform.position);
            // 计算碰撞体的大小
            float distance = Vector3.Distance(transform.position, targetPosition);
            collision.transform.localScale = new Vector3(0.1f, 0.1f, distance);

            // 添加一个脚本用于处理碰撞触发事件
            LightningCollision lightningCollision = collision.AddComponent<LightningCollision>();

            // 设置碰撞体关联的电塔
            lightningCollision.damage = damage;
            Transform start = targetLine.transform.GetChild(0);
            Transform end = targetLine.transform.GetChild(1);
            lightningCollision.SetTargetTower(this,targetTower,targetLine,start,end);
          
        }
        else
        {
            Debug.LogError("Failed to load CollisionPrefab from Resources!");
        } 
    }



    bool HasObstacleBetweenStartAndEnd(Vector3 start, Vector3 end)
    {
        // 射线检测，检查两点之间是否有障碍物
        RaycastHit hit;
        if (Physics.Linecast(start, end, out hit))
        {
            // 如果碰到障碍物，返回 true
            return true;
        }

        // 如果没有碰到障碍物，返回 false
        return false;
    }

}
