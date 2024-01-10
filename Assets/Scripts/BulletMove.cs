using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed=20;
    public int atk = 1;
    public float maxSearchDistance = 3;
    public float aliveTime;
    public GameObject target=null;
    public EnemyAI enemyScript;
    public TowerAI scripts=null;

    protected  void Start()
    {
        aliveTime = 0;
        if (target != null)
            enemyScript = target.GetComponent<EnemyAI>();
        else
        {
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                enemyScript = closestEnemy.GetComponent<EnemyAI>();
            }
            else {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        


        aliveTime += Time.deltaTime;
        if (aliveTime >= 2)
        {
            Destroy(gameObject);
            Debug.Log("Destroy_overtime");
        }
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        Attack();
    }

    public void Attack()
    {
        if (target != null)
        {
            //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
            if (Vector3.Distance(transform.position, target.transform.position) < 1f)
            {

                enemyScript.scripts = scripts;
                enemyScript.hurt(atk);
                /*
                GameObject blood= Instantiate(Resources.Load("blood") as GameObject, target.transform.position+new Vector3(0,1,0),target.transform.rotation* Quaternion.Euler(0f, 90f, 0f));
                ParticleSystem particleSystem = blood.GetComponent<ParticleSystem>();
                float scale = Mathf.Lerp(0.5f, 5f, (atk-5) / 95);
                var mainModule = particleSystem.main;
                mainModule.startSize = scale;
                particleSystem.Stop();
                particleSystem.Play();
                Destroy(blood, mainModule.duration);
                */
                Destroy(gameObject);
            }
        }

        else {
            Destroy(gameObject);
        }
    }


    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // 假设敌人物体有"Enemy"标签

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != gameObject) // 排除自身
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

                if (enemyAI != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distance < closestDistance && distance <= maxSearchDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }

        return closestEnemy;
    }
}
