using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using cakeslice;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform targetPos;
    public Transform[] allPos;
    public NavMeshPath path;
    private int currentDestinationIndex ;
    public coreController coreControl;
    public GameObject core;
    public int atk;
    public int maxHitPoint;
    public int HitPoint;
    public TowerAI scripts = null;
    public GameObject route;
    public float deadAnimScale = 1;

    //��ʾѪ��
    public GameObject healthCanvas;
    public Text healthText;
    public Image healthBarFill;

    public GameObject model;
    public bool isFlying;
    public bool verticalFlying=false;
    public bool isflyingUp = false;
    public float flyingSpeed;
    public bool isPlayingFlyingEnd = false;
    public bool isStatic;
    public bool isCompleteBorn = false;

    public bool fallOut=false;
    

    //��������
    public GameObject holdingBaby;
 

    void Start()
    {
        HitPoint = maxHitPoint;
    }

    void Update()
    {
        if (transform.position.y < -20)
            Destroy(gameObject);

        //��ֹϵ����
        if (!isStatic) {


            if (isFlying)
            {
                if (!isPlayingFlyingEnd)
                {
                    // ����Ƿ������ֱ���ƶ���Ŀ��λ�ã�����y��
                    Vector3 targetPosition = allPos[currentDestinationIndex].position;

                    if(verticalFlying)
                    targetPosition.y = transform.position.y;
                    
                    transform.LookAt(targetPosition);
                    transform.Translate(Vector3.forward * Time.deltaTime * flyingSpeed);
                    
                    
                    //transform.position = Vector3.MoveTowards(transform.position, targetPosition, flyingSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                    {                
                        checkBomb();
                    }
                }

                if (isflyingUp)
                {
                    transform.Translate(Vector3.up * flyingSpeed * Time.deltaTime);
                    if (transform.position.y > 30)
                        Destroy(gameObject);
                }
                UpdateHealthBarPosition();
                
            }



            else
            {
                //�Ƿ��е�λ-----������״̬
                if (agent.isOnNavMesh && agent.enabled)
                {
                    #region  ����Ŀ��
                    Vector3 nowLookAt = agent.destination;
                    nowLookAt.y = transform.position.y;
                    Vector3 targetDirection = (agent.steeringTarget - transform.position).normalized;
                    targetDirection.y = 0;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    if (targetRotation != Quaternion.Euler(0,0,0)) 
                        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, Time.deltaTime );
                    #endregion
                    //backToMesh();
   

                    # region 2.���Ŀ��
                    if (agent.remainingDistance < 0.1f)
                    {
                        
                        if (currentDestinationIndex != allPos.Length - 1)
                        {
                            print("goingToSetNextDestination");
                            currentDestinationIndex++;
                            SetNextDestination();

                        }
                        //else if(Vector3.Distance(transform.position, allPos[allPos.Length - 1].position) < 0.1f)
                        else
                        {
                            print(agent.destination);
                            checkBomb();
                        }

                    }

                    #endregion

                    UpdateHealthBarPosition();
                }
                   
            }
        }
        
    }
    
    void SetNextDestination()
    {
        // ���·��������Ϊ�գ�ֱ�ӷ���
        if (allPos == null || allPos.Length == 0)
        {
            Debug.LogWarning("Path points array is not set or empty.");
            return;
        }

        // ������һ��Ŀ���
        //CalculatePath��Vector3 targetPosition, NavMeshPath path��
        //agent.SetDestination(allPos[currentDestinationIndex].position);
        agent.CalculatePath(allPos[currentDestinationIndex].position, path);
        agent.SetPath(path);
        print("SetNextPathOver");
       
    }
    

    void checkBomb() {
        print("check bomb");
        if (isFlying)
        {
            isPlayingFlyingEnd = true;
            Animator anim = model.transform.GetComponent<Animator>();

            //����
            if (verticalFlying)
            {
                holdingBaby.transform.parent = null;
                isflyingUp = true;
                healthCanvas.SetActive(false);
                Collider c = GetComponent<SphereCollider>();
                c.enabled = false;
                RemoveFromTarget();
                release();
            }
            else {

                if (anim != null)
                    anim.Play("flyingEnd");
                healthBarFill.gameObject.SetActive(false);
                Invoke("lastDance", 1f);

            }           

        }
        else 
        {
                lastDance();
        }
       

        
   
   
    }
    public void lastDance()
    {
        Destroy(gameObject);
        if (scripts != null && scripts.enemy.Contains(gameObject))
            scripts.enemy.Remove(gameObject);
        coreControl.damaged(atk);
    }

    IEnumerator FlashOutline()
    {
   
        cakeslice.Outline outline = model.GetComponent<cakeslice.Outline>();
        if(outline!=null)
        outline.color = 1;
        
        healthBarFill.color = new Color(1,1,1,0.5f);
        // �ȴ�1��
        yield return new WaitForSeconds(0.5f);

        // �ָ�ԭʼ��ɫ
        if (outline != null)
            outline.color = 0;
        healthBarFill.color = new Color(1, 1, 1, 1);
    }


    public void hurt(int damage)
    {
        StartCoroutine("FlashOutline");
        HitPoint -= damage;
        //healthText.text = HitPoint.ToString();
        if (HitPoint <= 0)
        {
            if (verticalFlying)
            {
                print("holdingbaby��" + holdingBaby + "parent:" + holdingBaby.transform.parent);
                holdingBaby.transform.parent = null;
                release();
            }

            //scripts.enemy.Remove(gameObject);
            if (scripts!=null)
            {
                if (scripts.isGold)
                {
                    CardManager engine = GameObject.Find("Engine").GetComponent<CardManager>();
                    engine.currentMoney += scripts.goldIncome;
                    engine.UpdateMoneyText();
                }
               
            }
            if (!isStatic)
            {
                ShowBlood();
                Destroy(gameObject);
            }
                
            else {
                Animator anim = transform.GetChild(0).GetComponent<Animator>();
                anim.Play("death");
                Invoke("death", 1.5f);
            }
        }
    }

    void UpdateHealthBarPosition()
    {
        // ��������Ը��ݹ����Ѫ���ٷֱȸ���Ѫ���������
        float healthPercentage = (float)HitPoint / maxHitPoint;
        healthBarFill.fillAmount = healthPercentage;
    }

    public void InitializeMonster()
    {
        agent = GetComponent<NavMeshAgent>();
        currentDestinationIndex = 0;
        core = GameObject.Find("CORE");
        coreControl = core.GetComponent<coreController>();
        targetPos = core.transform;
        FindRoute();
        
    }

    public void FindRoute()
    {
        if (route != null)
        {
            // ��ȡ������������ΪѰ·��
            allPos = new Transform[route.transform.childCount];
            for (int i = 0; i < route.transform.childCount; i++)
            {
                allPos[i] = route.transform.GetChild(i);
            }
            if (!isFlying)
            {
                path = agent.path;
                agent.CalculatePath(allPos[0].position, path);
                //(allPos[0] + "   setPathOver");
                agent.SetPath(path);
            }
            
                

        }
        else
        {
            Debug.LogError("Cannot find 'allPos' object in the scene.");
        }
        //if (!isFlying)
            //agent.destination = allPos[0].position;
    }


    public void death()
    {

        Destroy(gameObject);
    
    }


    // ����
    public void release()
    {
        Rigidbody targetRigidbody = holdingBaby.GetComponent<Rigidbody>();
        targetRigidbody.isKinematic = false;
        balloonTrigger trig = holdingBaby.GetComponent<balloonTrigger>();
        trig.startChecking = true;

    }

    public void RemoveFromTarget()
    {

        TowerAI[] towers = FindObjectsOfType<TowerAI>();

        foreach (TowerAI tower in towers)
        {
           
            if (tower.targetObject == this.gameObject)
            {
               
                tower.targetObject = null;

                if (tower.enemy.Contains(this.gameObject))
                {
                    tower.enemy.Remove(this.gameObject);
                }
            }
        }

    }

    public IEnumerator DisableAndEnableNavMeshAgent()
    {
        // �ر�NavMeshAgent
        agent.enabled = false;
        Collider c = GetComponent<Collider>();
        c.isTrigger = false;
        fallOut = true;
        RemoveFromTarget();
        // �ȴ�1��
        yield return new WaitForSeconds(1f);

        // ��������NavMeshAgent,�ر���ײ�壬agent��Ѱ·��
        c.isTrigger = true;
        agent.enabled = true;
        fallOut = false;
        backToMesh();
       
        print("SetNextPathOver");
    }

    public void ShowBlood()
    {
        GameObject blood = Instantiate(Resources.Load("blood") as GameObject, transform.position+new Vector3(0,0.5f,0), transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        ParticleSystem particleSystem = blood.GetComponent<ParticleSystem>();
        //float scale = Mathf.Lerp(180f, 1000f, (maxHitPoint - 5) / 95);
        var mainModule = particleSystem.main;
        mainModule.startSize = deadAnimScale;
        particleSystem.Stop();
        particleSystem.Play();
        Destroy(blood, mainModule.duration);
        
    }

    public void backToMesh()
    {
        if (!agent.isOnNavMesh)
        {
          
            print("isnotOnMesh");
            RaycastHit hit;
            float raycastDistance = 0.01f;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
            {
                // ������߻����˵��棬��Ϊ�����Ѿ����
                if (hit.collider.tag == "ground")
                {
                    print(hit);
                    NavMeshHit navMeshHit;
                    if (NavMesh.FindClosestEdge(agent.transform.position, out navMeshHit, NavMesh.AllAreas))
                    {
                        transform.position = navMeshHit.position;
                        agent.CalculatePath(allPos[currentDestinationIndex].position, path);
                        agent.SetPath(path);
                        print("reset");
                    }
                }

            }
        }

    }
}
