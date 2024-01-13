using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PreviewPath : MonoBehaviour
{
    public Transform[] pathPoints;  // 存放路径点的数组
    public float moveSpeed = 5f;    // 移动速度
    public float previewHeight = 1f; // 光线预览的高度
    public float previewLength = 5f; // 光线预览的长度

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
        lineRenderer.positionCount = 2; // 初始设置线段只有两个点，即起点和终点

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
        //头部运动
        if (!isReach)
        {
            Vector3 Hdirection = (pathPoints[currentPointIndex].position + Vector3.up * previewHeight - headPosition).normalized;
            headPosition += (Hdirection * moveSpeed * Time.deltaTime);
        }


        //尾部运动
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
        if (isGo)//出发后，要么朝点前进，要么抄最后的额外点前进
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


        //头部判断
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

        //尾部判断
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
        // 如果已经到达最后一个路径点，则停止移动
        if (isReach)
        {
            return;
        }

        point.transform.LookAt(pathPoints[currentPointIndex].position);
        // 计算当前点到下一个点的方向
        Vector3 direction = (pathPoints[currentPointIndex].position - point.transform.position).normalized;

        // 移动物体
        point.transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // 判断是否到达下一个路径点
        float distanceToNextPoint = Vector3.Distance(point.transform.position, pathPoints[currentPointIndex].position);
        if (distanceToNextPoint <= 0.1f)  // 调整这个阈值以适应你的需求
        {
            // 物体到达一个目标点后，向下一个目标点前进
           point.transform.position = pathPoints[currentPointIndex].position;
            //currentPointIndex++;
        }
    }

}
