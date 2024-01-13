using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PreviewPath : MonoBehaviour
{
    public Transform[] pathPoints;  // ���·���������
    public float moveSpeed = 5f;    // �ƶ��ٶ�
    public float previewHeight = 1f; // ����Ԥ���ĸ߶�
    public float previewLength = 5f; // ����Ԥ���ĳ���

    private LineRenderer lineRenderer;
    private int currentPointIndex = 1;

    public bool isMid=false;
    public bool isGo = false;
    public bool isReach = false;
    Vector3 headPosition;
    Vector3 tailPosition;
    Vector3 midPoint;

    public GameObject point;
    public int pointCount;
    public List<Vector3> extraPoints;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pointCount = 1;
        lineRenderer.positionCount = 2; // ��ʼ�����߶�ֻ�������㣬�������յ�

        Vector3 direction = (pathPoints[0].position);


        transform.position = direction + Vector3.up * previewHeight;
        lineRenderer.SetPosition(1, direction + Vector3.up * previewHeight);
        lineRenderer.SetPosition(0, direction + Vector3.up * previewHeight);
        headPosition= direction + Vector3.up * previewHeight;
        tailPosition= direction + Vector3.up * previewHeight;

        point.transform.position = direction;


    }

    void Update()
    {
        MoveLine();
        //MoveObjectAlongPath();
    }

    void MoveLine()
    {
        //ͷ���˶�
        if (!isReach)
        {
            Vector3 Hdirection = (pathPoints[currentPointIndex].position + Vector3.up * previewHeight - headPosition).normalized;
            headPosition += (Hdirection * moveSpeed * Time.deltaTime);
        }


        //β���˶�
        if (!isGo)
        {
            Vector3 temp = headPosition;
            temp.y = pathPoints[0].position.y;
            if (Vector3.Distance(temp, pathPoints[0].position) > previewLength)
            {
                print("   " + temp + "    " + pathPoints[0].position);
                isGo = true;
            }

                
        }
        if (isGo)//������Ҫô����ǰ����Ҫô�����Ķ����ǰ��
        {
            if (extraPoints.Count == 0)
            {
                Vector3 Tdirection = (pathPoints[currentPointIndex].position + Vector3.up * previewHeight - tailPosition).normalized;
                tailPosition += (Tdirection * moveSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 Tdirection = (extraPoints[0] - tailPosition).normalized;
                tailPosition += (Tdirection * moveSpeed * Time.deltaTime);
            }
        }


        //ͷ���ж�
        float HdistanceToNextPoint = Vector3.Distance(headPosition, pathPoints[currentPointIndex].position + Vector3.up * previewHeight);
        if (HdistanceToNextPoint <0.1f)
        {
            if (currentPointIndex != pathPoints.Length - 1)
            {
                print("added");
                extraPoints.Add(pathPoints[currentPointIndex].position + Vector3.up * previewHeight);
                currentPointIndex++;       
                lineRenderer.positionCount++;
            }
            else
            {
                isReach = true;
            }

        }

        //β���ж�
        if (extraPoints.Count > 0)
        {
            float TdistanceToNextPoint = Vector3.Distance(tailPosition, extraPoints[0]);
            if (TdistanceToNextPoint <0.1f)
            {
                print("deleted");
                extraPoints.RemoveAt(0);
                lineRenderer.positionCount--;
            }

        }
        else
        {
            float TdistanceToNextPoint = Vector3.Distance(tailPosition, pathPoints[currentPointIndex].position + Vector3.up * previewHeight);
            if (TdistanceToNextPoint <= 0.1f && isReach)
            {
                Destroy(gameObject);
                return;
            }

        }


        lineRenderer.SetPosition(extraPoints.Count+1, headPosition);
        if (extraPoints.Count > 0)
        {
            for (int i = 1; i <=extraPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, extraPoints[i - 1]);
            }
        
        }
        lineRenderer.SetPosition(0, tailPosition);

    }
    public void MoveObjectAlongPath()
    {
        // ����Ѿ��������һ��·���㣬��ֹͣ�ƶ�
        if (isReach)
        {
            return;
        }

        point.transform.LookAt(pathPoints[currentPointIndex].position);
        // ���㵱ǰ�㵽��һ����ķ���
        Vector3 direction = (pathPoints[currentPointIndex].position - point.transform.position).normalized;

        // �ƶ�����
        point.transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // �ж��Ƿ񵽴���һ��·����
        float distanceToNextPoint = Vector3.Distance(point.transform.position, pathPoints[currentPointIndex].position);
        if (distanceToNextPoint <= 0.1f)  // ���������ֵ����Ӧ�������
        {
            // ���嵽��һ��Ŀ��������һ��Ŀ���ǰ��
           point.transform.position = pathPoints[currentPointIndex].position;
            //currentPointIndex++;
        }
    }

}
