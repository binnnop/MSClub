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
    public int voidHealth;
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
    public bool fallOut2 = false;

    public bool goDie = false;
    public bool isStunOver =false;

    //��������
    public GameObject holdingBaby;

    //Debuff
    public bool debuff_onFire=false;
    const int debuff_onFire_damage = 10;

    public Rigidbody rig;

    public const float g = 9.8f;
    public float verticalSpeed = 4f;
    private Vector3 moveDirection;
    private float angleSpeed;
    private float angle = 45;
    private float time;
    public float sSpeed = 8;
    public Vector3 targetPosition;
    public bool isRoofGo = false;

    public NavMeshLink link;
    public NavMeshLink lastLink;
    public bool hasLink=false;

    public bool canJump = true;

    void Start()
    {
        //agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        HitPoint = maxHitPoint;
        voidHealth = maxHitPoint;
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < 0)
            Destroy(gameObject);

        //��ֹϵ����
        if (!isStatic) {

            if (agent.enabled)
            {
                print(gameObject.name + "          " + agent.isOnNavMesh);
            }





            //�Ƿ��е�λ-----������״̬
            if (agent.isOnNavMesh && agent.enabled)
            {
                
                if (agent.isOnOffMeshLink)
                {


                    print("isOnOffMeshLink");


                    OffMeshLinkData data = agent.currentOffMeshLinkData;
                    Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;




                    //����һ�뱻�жϵ����
                    if (tryFindLink() == null&&!hasLink)
                    {
                        if (link.tag == "jump" && isRoofGo)
                        {
                            initialRoof(endPos);
                            print("�ϴεĵ�" + endPos+"     "+data);
                            hasLink = true;
                        }
                    }
                    //û�б��жϵ��������¼Lastlink
                    else
                    {
                        if(tryFindLink()!=null)
                        link = tryFindLink();

                        if (link.tag == "jump" && !isRoofGo)
                        {
                            initialRoof(endPos);
                        }
                        else if (isRoofGo)
                        {
                            time += Time.deltaTime;
                            float test = verticalSpeed - g * time;
                            transform.Translate(moveDirection.normalized * sSpeed * Time.deltaTime, Space.World);
                            transform.Translate(Vector3.up * test * Time.deltaTime * 4, Space.World);
                            float testAngle = -angle + angleSpeed * time;
                            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
                        }
                        else
                        {
                            rig.isKinematic = true;
                            //Move the agent to the end point
                            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                            print(transform.position + "        start:" + data.startPos + "        end:" + data.endPos);

                            //when the agent reach the end point you should tell it, and the agent will "exit" the link and work normally after that      
                        }


                    }






                    //���
                    // if (  agent.transform.position == endPos)
                    if (Vector3.Distance(agent.transform.position, endPos) < 2 && isRoofGo)
                    {
                        agent.CompleteOffMeshLink();
                        hasLink = false;
                        link = null;
                        time = 0;

                        rig.isKinematic = false;
                        isRoofGo = false;
                        NavMeshHit navMeshHit;
                        if (NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 2,NavMesh.AllAreas))
                        {
                            
                            transform.position = navMeshHit.position;
                            agent.destination = allPos[currentDestinationIndex].position;
                            agent.CalculatePath(allPos[currentDestinationIndex].position, path);
                            agent.SetPath(path);
                            print("����Ѱ�ҵ���������setPath");
                            //rig.isKinematic = false;
                        }
                        //backToMesh();
                        //SetNextDestination(); 


                    }
                    else if (Vector3.Distance(agent.transform.position, endPos) < 0.1f)
                    {
                        rig.isKinematic = false;
                        agent.CompleteOffMeshLink();
                        hasLink = false;
                        link = null;
                        time = 0;
                    }

                }

                if (fallOut2 && isStunOver)
                {

                    if (!agent.isOnNavMesh)
                    {
                        NavMeshHit navMeshHit;
                        if (NavMesh.FindClosestEdge(agent.transform.position, out navMeshHit, NavMesh.AllAreas))
                        {
                            isStunOver = false;
                            fallOut2 = false;
                            transform.position = navMeshHit.position;
                            agent.destination = allPos[currentDestinationIndex].position;
                            agent.CalculatePath(allPos[currentDestinationIndex].position, path);
                            agent.SetPath(path);
                            Collider c = GetComponent<Collider>();
                            c.isTrigger = true;
                        }
                        print("stunOverReset");
                    }
                    else
                    {
                        isStunOver = false;
                        fallOut2 = false;
                        Collider c = GetComponent<Collider>();
                        c.isTrigger = true;
                    }
                }


                #region  ����Ŀ��
                Vector3 nowLookAt = agent.destination;
                nowLookAt.y = transform.position.y;
                Vector3 targetDirection = (agent.steeringTarget - transform.position).normalized;
                targetDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                if (targetRotation != Quaternion.Euler(0, 0, 0))
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
                #endregion
                //backToMesh();


                #region 2.���Ŀ��
                /*
                agent.CalculatePath(allPos[currentDestinationIndex].position, path);
                agent.SetPath(path);
                */
                if (!agent.isOnOffMeshLink)
                {
                    if (Vector3.Distance(agent.destination, allPos[allPos.Length - 1].position) > 2)
                    {
                        print("destination��" + agent.destination);
                        agent.CalculatePath(allPos[currentDestinationIndex].position, path);
                        agent.SetPath(path);
                    }


                    // Debug.Log("Path Points: " + string.Join(", ", path.corners));

                    if (Vector3.Distance(transform.position, allPos[currentDestinationIndex].position) < 1f)
                    {

                        if (currentDestinationIndex != allPos.Length - 1)
                        {
                            // print("goingToSetNextDestination");
                            currentDestinationIndex++;
                            SetNextDestination();

                        }
                        //else if(Vector3.Distance(transform.position, allPos[allPos.Length - 1].position) < 0.1f)
                        else if (Vector3.Distance(transform.position, allPos[allPos.Length - 1].position) < 1f)
                        {
                            //print(agent.destination);
                            checkBomb();
                        }

                    }


                }



                #endregion

                UpdateHealthBarPosition();
            }

          
            }
      

        if (voidHealth < 0)
        {
            goDie = true;
        }

    }


    public void StartBurning()
    {
        debuff_onFire= true;
        InvokeRepeating("debuff_Burning", 1f, 1f); // ÿ�����ApplyBurnDamage����
    }

    public void debuff_Burning() {
        hurt(debuff_onFire_damage);   
    }

    void initialRoof(Vector3 pos)
    {
        targetPosition = pos;
        rig.isKinematic = true;

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
        isRoofGo = true;


    }

    void SetNextDestination()
    {
        // ���·��������Ϊ�գ�ֱ�ӷ���
        if (allPos == null || allPos.Length == 0)
        {
            Debug.LogWarning("Path points array is not set or empty.");
            return;
        }
        agent.destination = allPos[currentDestinationIndex].position;
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

    NavMeshLink tryFindLink() {

        NavMeshLink nowLink = (NavMeshLink)agent.navMeshOwner;
        return nowLink;
    }

    public void lastDance()
    {
        print("��Ҫȥ���ˣ��ҵ�λ����:" + transform.position + "     ���ĵ�λ���ǣ�" + core.transform.position + "     �����ڵ�index�ǣ�" + currentDestinationIndex+"     �ҵ�fallout�ǣ�"+fallOut);
        Destroy(gameObject);
        if (scripts != null && scripts.enemy.Contains(gameObject))
            scripts.enemy.Remove(gameObject);

        if(Vector3.Distance(transform.position,core.transform.position) < 5f)
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
                /*
                if (scripts.isGold)
                {
                    CardManager engine = GameObject.Find("Engine").GetComponent<CardManager>();
                    engine.currentMoney += scripts.goldIncome;
                    engine.UpdateMoneyText();
                }
                */
               
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
        // agent.autoTraverseOffMeshLink = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
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
 
       
       // print("SetNextPathOver");
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (fallOut2 && collision.gameObject.CompareTag("ground"))
        {
            print("1111111111111111111111111111111111111111111111111");
            agent.enabled = true;
            isStunOver = true;
        }
        else
            print("???????????????????????????????????????");
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (fallOut2 && collision.gameObject.CompareTag("ground"))
        {
            print("222222222222222222222222222222222222222222222222");
            agent.enabled = true;
            isStunOver = true;
        }
        else
            print("???????????????????????????????????????");
    }
    

    public IEnumerator DisableAndEnableNavMeshAgent2()
    {
        print("kinematic�Ѿ�����");
        rig.isKinematic = false;
        time = 0;
        if (agent.isOnOffMeshLink)
        {
            agent.CompleteOffMeshLink();
        }
            // �ر�NavMeshAgent
            isStunOver = false;
        agent.enabled = false;
        Collider c = GetComponent<Collider>();
        c.isTrigger = false;
        fallOut2 = true;
        //RemoveFromTarget();
        // �ȴ�1��
        print("ʧ��--��ʼ�ȴ�");
        yield return new WaitForSeconds(1f);
        if (!isStunOver) {

            agent.enabled = true;
            isStunOver = true;
        }
        

        // print("SetNextPathOver");
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
        if (agent.isOnNavMesh)
        {

            print("isnotOnMesh");
            //RaycastHit hit;
            //float raycastDistance = 0.01f;
            
                    print("hit the ground");
                    NavMeshHit navMeshHit;
                    if (NavMesh.SamplePosition (transform.position, out navMeshHit, 2f,NavMesh.AllAreas))
                    {
                        if (transform.position.y-navMeshHit.position.y >0.5f )
                        {
                            print("�ع���Ϣ��" + navMeshHit.position + "                       " + transform.position);
                            transform.position = navMeshHit.position;
                            agent.destination = allPos[currentDestinationIndex].position;
                            agent.CalculatePath(allPos[currentDestinationIndex].position, path);
                            agent.SetPath(path);

                        }
                        else
                        {
                    print("��Ϊλ��̫���ж�����");
                            Destroy(gameObject);
                        }

                    }
                    else
                    {
                print("��Ϊ�Ҳ���Nav�ж�����");
                Destroy(gameObject);
                    }
                    //print("reset" + agent.remainingDistance);
                }
                


        else {
            Destroy(gameObject);

            //agent.destination = allPos[currentDestinationIndex].position;
            agent.CalculatePath(allPos[currentDestinationIndex].position, path);
            agent.SetPath(path);
            
            print("�Ѿ�����������Ŷ");
            Collider c = GetComponent<Collider>();
            c.isTrigger = true;
        }
            
    }
}
