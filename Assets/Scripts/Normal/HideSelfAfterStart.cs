using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSelfAfterStart : MonoBehaviour
{
    public float delayInSeconds = 5f; // 设置隐藏的延迟时间

    void Start()
    {
        // 在指定时间后调用HideSelf方法
        Invoke("HideSelf", delayInSeconds);
    }

    void HideSelf()
    {
        // 隐藏自己（可以根据需要修改）
        gameObject.SetActive(false);
    }
}
