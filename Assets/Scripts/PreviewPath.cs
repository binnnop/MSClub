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


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3; // ��ʼ�����߶�ֻ�������㣬�������յ�

        Vector3 direction = (pathPoints[0].position);


        transform.position = direction + Vector3.up * previewHeight;
        lineRenderer.SetPosition(1, direction + Vector3.up * previewHeight);
        lineRenderer.SetPosition(2, direction + Vector3.up * previewHeight);
        lineRenderer.SetPosition(0, direction + Vector3.up * previewHeight);
        headPosition= direction + Vector3.up * previewHeight;
        tailPosition= direction + Vector3.up * previewHeight;

        point.transform.position = direction;


    }

    void Update()
    {
        MoveLine();
        MoveObjectAlongPath();
    }

    void MoveLine()
    {

        

        if (!isMid)
        {
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
                if(Vector3.Distance(temp, pathPoints[0].position) > previewLength)
                isGo = true;
                //print(previewLength + "    " + Vector3.Distance(headPosition, pathPoints[0].position));
            }
            if (isGo)
            {
                Vector3 Tdirection = (pathPoints[currentPointIndex].position + Vector3.up * previewHeight - tailPosition).normalized;
                tailPosition += (Tdirection * moveSpeed * Time.deltaTime);
            }




            float HdistanceToNextPoint = Vector3.Distance(headPosition, pathPoints[currentPointIndex].position + Vector3.up * previewHeight);


            if (HdistanceToNextPoint <= 0.1f)
            {
                if (currentPointIndex != pathPoints.Length - 1)
                {
                    currentPointIndex++;
                    isMid = true;
                    midPoint = headPosition;
                }
                else
                {
                    isReach = true;
                }

            }


            float TdistanceToNextPoint = Vector3.Distance(tailPosition, pathPoints[currentPointIndex].position + Vector3.up * previewHeight);
            if (TdistanceToNextPoint <= 0.1f &&isReach)
            {
                Destroy(gameObject);
                return;
            }


            lineRenderer.SetPosition(0, tailPosition);
            lineRenderer.SetPosition(1, headPosition);
            lineRenderer.SetPosition(2, headPosition);

        }



        else
        {
            //ͷ���˶�
            if (!isReach)
            {
                Vector3 Hdirection = (pathPoints[currentPointIndex].position + Vector3.up * previewHeight - headPosition).normalized;
                headPosition += (Hdirection * moveSpeed * Time.deltaTime);
            }
            

            //β���˶�
            if (!isGo && Vector3.Distance(headPosition, pathPoints[0].position) > previewLength)
            {
                isGo = true;
            }
            if (isGo)
            {
                Vector3 Tdirection = (midPoint - tailPosition).normalized;
                tailPosition += (Tdirection * moveSpeed * Time.deltaTime);
            }




            float HdistanceToNextPoint = Vector3.Distance(headPosition, pathPoints[currentPointIndex].position+ Vector3.up * previewHeight);

            if (HdistanceToNextPoint <= 0.1f)
            {
                if (currentPointIndex != pathPoints.Length - 1)
                {
                    currentPointIndex++;
                    isMid = true;
                    midPoint = headPosition;
                }
                else {
                    isReach = true;
                }
                
            }

            float TdistanceToNextPoint = Vector3.Distance(tailPosition, midPoint);
            if (TdistanceToNextPoint <= 0.1f )
            {
                isMid = false;
            }


            lineRenderer.SetPosition(0, tailPosition);
            lineRenderer.SetPosition(1, midPoint);
            lineRenderer.SetPosition(2, headPosition);


        }

        
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
