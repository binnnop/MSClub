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

            // ��Start�л�ȡ��Χ�ڵĵ���
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
        // ��ȡĿ�������ϵ� LightningTower ���
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
        // ʹ�� Resources.Load ���� LightningBolt Ԥ����
        GameObject lightningBoltPrefab = Resources.Load<GameObject>("LightningBoltPrefab");

        // ���߼��
        
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
        // �� LightningStart �� LightningEnd ֮������һ����ײ��
        GameObject collisionPrefab = Resources.Load<GameObject>("CollisionPrefab");

        if (collisionPrefab != null)
        {
            GameObject collision = Instantiate(collisionPrefab, allLight.transform);
            // ������ײ���λ��
            collision.transform.position = (transform.position + targetPosition) / 2f;
            collision.transform.LookAt(transform.position);
            // ������ײ��Ĵ�С
            float distance = Vector3.Distance(transform.position, targetPosition);
            collision.transform.localScale = new Vector3(0.1f, 0.1f, distance);

            // ���һ���ű����ڴ�����ײ�����¼�
            LightningCollision lightningCollision = collision.AddComponent<LightningCollision>();

            // ������ײ������ĵ���
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
        // ���߼�⣬�������֮���Ƿ����ϰ���
        RaycastHit hit;
        if (Physics.Linecast(start, end, out hit))
        {
            // ��������ϰ������ true
            return true;
        }

        // ���û�������ϰ������ false
        return false;
    }

}
