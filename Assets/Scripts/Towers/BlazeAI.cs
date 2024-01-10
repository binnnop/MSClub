using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeAI : MonoBehaviour
{
    public float attackSpeed = 2f;
    public float attackSpeedBuff;
    public int attackDamage = 20; 
    public GameObject model;
    private float timeSinceLastAttack = 0f;
    private Animator animator;
    public List<GameObject> enemy;

    public Collider selfCollider;


    private void Start()
    {
        animator = model.transform.GetComponent<Animator>();
        enemy = new List<GameObject>();

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

    public void ModifyAttackSpeed(float multiplier)
    {
        attackSpeedBuff += multiplier;
    }

    private void FixedUpdate()
    {
        // ��ʱ�������ڿ��ƹ������
        timeSinceLastAttack += Time.deltaTime;
        enemy.RemoveAll(item => item == null);

        if (timeSinceLastAttack >= attackSpeed / (1 + attackSpeedBuff)  && enemy.Count > 0)
        {
            if (animator != null)
            {
                animator.Play("Blaze");
            }
            Attack();
            timeSinceLastAttack = 0f;
            
        }
    }

    private void Attack()
    {
        // �����Χ�ĵ���
        print("blazeAttack");

        foreach (GameObject enemyObject in enemy)
        {
            if (enemyObject == null)
            {
                continue; // �����ѱ����ٵĶ���
            }

            EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();
            
            if (enemyAI != null)
            {
                enemyAI.hurt(attackDamage);
            }            
         
        }
        

    }
 

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!enemy.Contains(other.gameObject))
            {
                enemy.Add(other.gameObject);
                print(other.gameObject + "added");
            }
            else
                print("?????????????????????????????????????");
        }
        else
        print("����ô˭����");
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {

            if (enemy.Contains(other.gameObject))
            {
                enemy.Remove(other.gameObject);
                print(other.gameObject + "removed");
            }
        }
    }




}
