using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squeeze : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �����ײ�Ķ����Ƿ��ǵ���
        if (other.CompareTag("Enemy"))
        {
            // �ݻٵ�����Ϸ����
            Destroy(other.gameObject);
        }
    }
}
