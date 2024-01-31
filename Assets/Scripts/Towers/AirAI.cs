using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAI : TowerAI

{
    public float impactForce = 5f;
    public float explosionRadius = 5f;
    public ParticleSystem attackFx;

    new void Start()
    {
        dis = 10000;
        targetObject = null;
        enemy = new List<GameObject>();
        model = transform.GetChild(0);
        model.forward *= -1;
        core = GameObject.Find("CORE");
        attackFx.Pause();


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



        new void LookTarget()
    {
        //print(model.forward);
        


        times += Time.deltaTime;
        if (times >= attackSpeed / (1 + attackSpeedBuff))
        {
            Vector3 pos = targetObject.transform.position;
            pos.y = model.transform.position.y;
            model.transform.LookAt(pos);
            Attack();
            times = 0;
        }

    }

    private new void Attack()
    {

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);

        //给子弹挂脚本
        attackFx.Play();
        bullet.AddComponent<BulletMoveAir>().target = targetObject;
        bullet.GetComponent<BulletMoveAir>().scripts = this;
        bullet.GetComponent<BulletMoveAir>().atk = towerAtk + heroRobotIncrease;
        bullet.GetComponent<BulletMoveAir>().impactForce = impactForce;
        bullet.GetComponent<BulletMoveAir>().explosionRadius = explosionRadius;
        bullet.transform.LookAt(targetObject.transform.position);
        totalDamage += (towerAtk + heroRobotIncrease);
    }
}
