using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCollision : MonoBehaviour
{
    public float animScale=0.1f;
    private LightningAI sourceTower;
    private LightningAI targetTower;
    private Transform start;
    private Transform end;
    private GameObject targetLine;
    public int damage;

    Collider c;

    private void Start()
    {
        c = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        // 如果碰撞到了怪物，造成伤害
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>().hurt(damage);
            GameObject blood = Instantiate(Resources.Load("Lightning") as GameObject,other.transform.position + new Vector3(0, 0.5f, 0), other.transform.rotation);
            
            ParticleSystem particleSystem = blood.GetComponent<ParticleSystem>();
            //float scale = Mathf.Lerp(180f, 1000f, (maxHitPoint - 5) / 95);
            var mainModule = particleSystem.main;

        }
    }
    public void SetTargetTower(LightningAI source,LightningAI target,GameObject line,Transform Lstart,Transform Lend)
    {
        sourceTower = source;
        targetTower = target;
        targetLine = line;
        start = Lstart;
        end = Lend;
    }

    void Update()
    {
        // 在每一帧检查关联的电塔是否存在，如果不存在，销毁碰撞体
        if (targetTower == null)
        {
            Destroy(targetLine);
            Destroy(gameObject);   
        }

        if (start.position != sourceTower.transform.position||end.position!=targetTower.transform.position)
        {
            UpdateCollision();
            c.enabled = false;
        }
        else if(!c.enabled)
        {
            c.enabled = true;
        }

        
    }

    public void UpdateCollision()
    {
        start.position = sourceTower.transform.position;
        end.position = targetTower.transform.position;
        transform.position = (sourceTower. transform.position + targetTower.transform. position) / 2f;
       transform.LookAt(sourceTower.transform.position);
        // 计算碰撞体的大小
        float distance = Vector3.Distance(sourceTower.transform. position, targetTower.transform.position);
        transform.localScale = new Vector3(0.1f, 0.1f, distance);

    }

}
