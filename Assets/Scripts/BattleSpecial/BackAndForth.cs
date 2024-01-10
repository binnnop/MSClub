using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    public float moveSpeed = 5f;  // 移动速度
    public float moveDistance = 10f;  // 移动距离

    private bool movingForward = true;
    private Vector3 initialPosition;
    public Transform panel;
    public Transform self;

    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float moveDelta = moveSpeed * Time.deltaTime;

        // 如果正在向前运动，并且距离超过了移动距离，切换方向
        if (movingForward &&  initialPosition.z-transform.position.z >= moveDistance)
        {
            movingForward = false;
        }
        // 如果正在向后运动，并且距离回到了初始位置，切换方向
        else if (!movingForward && transform.position.z >= initialPosition.z)
        {
            movingForward = true;
        }

        // 根据方向移动
        if (movingForward)
        {
            transform.Translate(Vector3.back * moveDelta);
            panel.transform.Translate(Vector3.back * moveDelta);
        }
        else
        {
            transform.Translate(Vector3.forward  * moveDelta);
            panel.transform.Translate(Vector3.forward * moveDelta);
        }
    }
}
