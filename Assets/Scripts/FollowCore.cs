using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCore : MonoBehaviour
{
    public Transform targetObject;
    public Vector3 virgin;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            // 获取目标物体的屏幕坐标
            //Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(targetObject.position);

            //transform.position = targetScreenPos;
            transform.position = targetObject.position+virgin;
        }
    }
}
