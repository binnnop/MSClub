using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class roofBulletMove : BulletMove
{

    public const float g = 9.8f;
    public float verticalSpeed = 2f;
    private Vector3 moveDirection;
    private float angleSpeed;
    private float angle = 45;
    private float time;
    public float sSpeed = 8;
    public float attackRadius = 5f;

    public Vector3 targetPosition;

    new void Start()
    {
        targetPosition = target.transform.position;

        float tmepDistance = Vector3.Distance(transform.position, targetPosition);
        float tempTime = tmepDistance / sSpeed;
        float riseTime, downTime;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;
        transform.LookAt(targetPosition);
        float tempTan = verticalSpeed / sSpeed;
        double hu = Mathf.Atan(tempTan);
        angle = (float)(180 / Mathf.PI * hu);
        transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);
        angleSpeed = angle / riseTime;
        moveDirection = targetPosition - transform.position;
    }

 

    new void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) <0.5)
        {
            Attack();
            GameObject anim = Instantiate(Resources.Load("roof") as GameObject, transform.position + new Vector3(0, 0, 0), transform.rotation * Quaternion.Euler(0f, 90f, 0f));
            ParticleSystem particleSystem = anim.GetComponent<ParticleSystem>();
            //float scale = Mathf.Lerp(180f, 1000f, (maxHitPoint - 5) / 95);
            var mainModule = particleSystem.main;
            //mainModule.startSize = deadAnimScale;
            particleSystem.Stop();
            particleSystem.Play();
            Destroy(anim, mainModule.duration);
        }  
        time += Time.deltaTime;
        float test = verticalSpeed - g * time;
        transform.Translate(moveDirection.normalized * sSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * test * 4*Time.deltaTime, Space.World);
        float testAngle = -angle + angleSpeed * time;
        transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);

    }






    new void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius);

        foreach (Collider collider in colliders)
        {
            // 检查是否是敌人
            if (collider.CompareTag("Enemy"))
            {
                // 获取敌人的 EnemyAI 组件
                EnemyAI enemy = collider.GetComponent<EnemyAI>();

                if (enemy != null)
                {
                    dealDamage(enemy, atk);
                }
            }
        }

        // 销毁子弹
        Destroy(gameObject);

    }
}
