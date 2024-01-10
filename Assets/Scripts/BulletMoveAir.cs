using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletMoveAir : BulletMove
{
    public float impactForce = 5f;
    public float explosionRadius = 5f; // ±¬Õ¨°ë¾¶

    new void Update()
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

    new public void Attack()
    {
        if (target != null)
        {
            //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
            if (Vector3.Distance(transform.position, target.transform.position) < 1f)
            {

                enemyScript.scripts = scripts;
                enemyScript.hurt(atk);

                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

                foreach (var collider in colliders)
                {
                    Rigidbody targetRb = collider.GetComponent<Rigidbody>();
                    if (targetRb != null)
                    {
                        //NavMeshAgent navMeshAgent = collider.gameObject.GetComponent<NavMeshAgent>();
                        //navMeshAgent.enabled = false;
                        EnemyAI e = collider.gameObject.GetComponent<EnemyAI>();
                        e.StartCoroutine("DisableAndEnableNavMeshAgent");
                        Vector3 impactDirection = transform.forward;  // ·´Ïò»÷ÍË
                        //impactDirection.y = 0;
                        targetRb.AddForce(impactDirection * impactForce, ForceMode.Impulse);
                    }
                }
              
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
