using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    public GameObject objectToGenerate; // ָ��Ҫ���ɵ�����
    public int numberOfObjectsToGenerate = 5; // ָ��Ҫ���ɵ���������
    public float spawnInterval = 2f; // ָ�����������ʱ�������룩

    private int generatedObjectsCount = 0;
    private float timer = 0f;

    public Transform[] pathPoints;


    void Update()
    {
        // ����Ѿ��������㹻���������壬��ֹͣ����
        if (generatedObjectsCount >= numberOfObjectsToGenerate)
        {
            Debug.Log("�������㹻���������壬ֹͣ���ɡ�");
            return;
        }

        // ���¼�ʱ��
        timer += Time.deltaTime;

        // �����ʱ���������ɼ�����������岢���ü�ʱ��
        if (timer >= spawnInterval)
        {
            GenerateObject();
            timer = 0f;
        }
    }
    private void GenerateObject()
    {
        // ��������
        GameObject line=Instantiate(objectToGenerate, transform.position, Quaternion.identity);
        line.GetComponent<PreviewPath>().pathPoints=pathPoints;
        // �������ɵ���������
        generatedObjectsCount++;

        Debug.Log("�������� #" + generatedObjectsCount);
    }
}
