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

    public bool abilityBlast=false;
    public float abilityBlastRadius = 2f;
    public bool abilityBurn = false;

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
        if (abilityBlast)
        {
            BlastAttack();
        }
        else
        Attack();
    }

    public void Attack()
    {
        if (target != null)
        {
            //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
            if (Vector3.Distance(transform.position, target.transform.position) < 1f)
            {

                dealDamage(enemyScript,atk);



                Limb limb;
                limb = target.GetComponent<Limb>();
                if (limb != null)
                {
                    if (enemyScript.HitPoint < limb.damageThreshold&&!limb.limbAdded)
                    {                  
                            Rigidbody limbRigidbody = limb.limb.AddComponent<Rigidbody>();
                        limbRigidbody.mass = 3f;
                        Vector3 forceDirection = (limb.transform.position - transform.position).normalized;

                             limb.transform.parent = null;
                            limbRigidbody.AddForce(forceDirection * limb.limbForce, ForceMode.Impulse);

                            limb.limbAdded = true;
                       
                    }
                }

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


    public void BlastAttack()
    {
        if (target != null)
        {
            //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
            if (Vector3.Distance(transform.position, target.transform.position) < 1f)
            {


                Collider[] colliders = Physics.OverlapSphere(target.transform.position, abilityBlastRadius);

                GameObject blood = Instantiate(Resources.Load("boom") as GameObject, target.transform.position + new Vector3(0, 1, 0), target.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
                ParticleSystem particleSystem = blood.GetComponent<ParticleSystem>();
                var mainModule = particleSystem.main;
                /*
                float scale = Mathf.Lerp(0.5f, 5f, (atk - 5) / 95);
                
                mainModule.startSize = scale;
                particleSystem.Stop();
                particleSystem.Play();
                */
                Destroy(blood, mainModule.duration);

                foreach (Collider collider in colliders)
                {
                    // 检查是否是敌人
                    if (collider.CompareTag("Enemy"))
                    {
                        // 获取敌人的 EnemyAI 组件
                        EnemyAI enemy = collider.GetComponent<EnemyAI>();

                        if (enemy != null && enemy ==enemyScript)
                        {
                            // 对敌人造成伤害
                            dealDamage(enemy, atk);
                        }
                        else if (enemy != null)
                        {
                            dealDamage(enemy, atk / 2);
                        }
                    }
                }

                // 销毁子弹
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        }

    public void dealDamage(EnemyAI enemy,int damage)
    {
        enemy.scripts = scripts;
        enemy.hurt(damage);
        if (abilityBurn&&! enemy.debuff_onFire)
        {
            GameObject burnParticle = Instantiate(Resources.Load("Fire") as GameObject,enemy.transform.position + new Vector3(0, 1, 0), enemy.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
            burnParticle.transform.localScale = enemy.transform.localScale;
            burnParticle.transform.parent = enemy.transform;
            enemy.StartBurning();
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
