using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSelfAfterStart : MonoBehaviour
{
    public float delayInSeconds = 5f; // �������ص��ӳ�ʱ��

    void Start()
    {
        // ��ָ��ʱ������HideSelf����
        Invoke("HideSelf", delayInSeconds);
    }

    void HideSelf()
    {
        // �����Լ������Ը�����Ҫ�޸ģ�
        gameObject.SetActive(false);
    }
}
