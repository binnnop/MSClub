using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    public float moveSpeed = 5f;  // �ƶ��ٶ�
    public float moveDistance = 10f;  // �ƶ�����

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

        // ���������ǰ�˶������Ҿ��볬�����ƶ����룬�л�����
        if (movingForward &&  initialPosition.z-transform.position.z >= moveDistance)
        {
            movingForward = false;
        }
        // �����������˶������Ҿ���ص��˳�ʼλ�ã��л�����
        else if (!movingForward && transform.position.z >= initialPosition.z)
        {
            movingForward = true;
        }

        // ���ݷ����ƶ�
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
