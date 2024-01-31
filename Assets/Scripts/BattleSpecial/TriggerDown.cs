using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TriggerDown : MonoBehaviour
{
    private bool hasTriggeredDescent = false;
    private int emptyAICount = 0;
    public float speed;
    public List<int> descentThresholds; // 阈值列表
    public List<NavMeshObstacle> obstacles;
    public List<NavMeshObstacle> obstacles2;
    public List<NavMeshObstacle> obstacles3;
    public List<NavMeshObstacle> obstacles4;
    public List<Base> bases;
    public List<InterfaceDown> downObj;

    public List<GameObject> hideList;

    public int index;
    public NavMeshSurface surface;

    public NavMeshLink inLink;
    public NavMeshLink outLink;

    public Collider colliderTrig;
    public List<EnemyAI> targetEnemy;

    public bool fillFix = false;

    private void Start()
    {
        index = 0;
        colliderTrig=GetComponent<Collider>();
    }
    void Update()
    {
        // 获取场上带有EmptyAI组件的物体数量
        //emptyAICount = GameObject.FindObjectsOfType<EmptyAI>().Length;

        // 遍历阈值列表
       if(index<descentThresholds.Count)
        {

            // 如果超过当前阈值且尚未触发下降
            if (GetCount() >= descentThresholds[index] && !hasTriggeredDescent)
            {
                // 触发下降
                StartCoroutine(Descent());
                foreach (InterfaceDown iterDown in downObj)
                {
                    iterDown.StartCoroutine("Descent");
                }
                index++;
            }
        }
    }

    int GetCount() {
        int count = 0;
    foreach(Base baseObj in bases)
     {
            count += baseObj.cardCount;       
      }
        return count;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI e = other.GetComponent<EnemyAI>();
            if (e != null&&!targetEnemy.Contains(e))
            {
                targetEnemy.Add(e);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI e = other.GetComponent<EnemyAI>();
            if (e != null && targetEnemy.Contains(e))
            {
                targetEnemy.Remove(e);
            }
        }
    }
    IEnumerator Descent()
    {
        // 向下运动4个单位
        fillFix = false;
        float descentDistance = 4f;
        Vector3 targetPosition = transform.position - new Vector3(0f, descentDistance, 0f);
        float descentSpeed = speed;

        if (inLink != null)
            inLink.endPoint = inLink.endPoint - new Vector3(0, 4, 0);
        if (outLink != null)
            outLink.startPoint = outLink.startPoint - new Vector3(0, 4, 0);

        List<NavMeshObstacle> obstacleList=new List<NavMeshObstacle>();
        if (index == 0)
            obstacleList = obstacles;
        if(index==1)
            obstacleList = obstacles2;
        if (index == 2)
            obstacleList = obstacles3;
        if (index == 3)
            obstacleList = obstacles4;

        if (obstacleList .Count>0)
        {
            for (int i = 0; i < obstacleList.Count; i++)
            {
                obstacleList[i].enabled = !obstacleList[i].enabled;
            }
        }

        if (hideList.Count > index)
        {
            if(hideList[index]!=null)
            hideList[index].SetActive(false);
        }
        



        //EnemyAI[] enemyAIs = GameObject.FindObjectsOfType<EnemyAI>();

        // 遍历所有找到的EnemyAI物体并触发DisableAndEnableNavMeshAgent方法
        foreach (EnemyAI enemyAI in targetEnemy)
        {
            if (enemyAI != null)
            {
                Rigidbody targetRb = enemyAI.gameObject.GetComponent<Rigidbody>();
                if (targetRb != null)
                {
                    EnemyAI e = enemyAI.gameObject.GetComponent<EnemyAI>();
                    e.StartCoroutine("DisableAndEnableNavMeshAgent2");
                    Vector3 impactDirection = Vector3.down;
                    targetRb.AddForce(impactDirection * 2, ForceMode.Impulse);
                    print("推完了");
                }
                else
                    print("这个b没有刚体");
            }
            
        }

        while (transform.position.y > targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * descentSpeed);
            print("在下降······································");
            yield return null;
        }

        fillFix = true;
    }
}
