using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class balloonTrigger : MonoBehaviour
{
    public Collider sCollider;
    public NavMeshAgent sAgent;
    public EnemyAI sEnemy;
    public Animator anim;
    public GameObject sHP;
    public bool startChecking = false;
    public bool isGrounded = false;

    public float  downHurt;
    public float hurtPerSecond;
    public int extraDamage;

    void Start()
    {
        downHurt = 0;
    }


    void Update()
    {
        if (startChecking)
        {
            CheckGrounded();
            downHurt += (hurtPerSecond)*Time.deltaTime;
        }
            

        if (isGrounded)
        {
            startChecking = false;
            Born();
        }
    }

    public void Born()
    {
        sAgent = GetComponent<NavMeshAgent>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        sEnemy = GetComponent<EnemyAI>();
        sCollider = GetComponent<CapsuleCollider>();

        sCollider.enabled = true;
        sAgent.enabled = true;
        anim.enabled = true;
        sHP.SetActive(true);

        sEnemy.route =GameObject.Find("onlyPos");
        sEnemy.enabled = true;
        sEnemy.InitializeMonster();
        Invoke("delayHurt", 0.1f);
        isGrounded = false;

    }

    void delayHurt()
    {
        sEnemy = GetComponent<EnemyAI>();
        int damage = (int)downHurt+extraDamage;
        sEnemy.hurt(damage);
           
    }

    void CheckGrounded()
    {
        
        RaycastHit hit;
        float raycastDistance = 1f; 

        // �����������¼��
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // ������߻����˵��棬��Ϊ�����Ѿ����
            if (hit.collider.tag == "ground")
            {
                isGrounded = true;
                startChecking = false;
            }
               

        }
        else
        {
            // ��������δ���
            isGrounded = false;
        }

        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

}
