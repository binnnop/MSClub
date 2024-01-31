using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectDelay : MonoBehaviour
{
    public GameObject[] objectsToShow; // ��������
    public int[] delay; 
    private int currentIndex = 0; 

    void Start()
    {
        Invoke("ShowNextObject", delay[currentIndex]);
    }

    void ShowNextObject()
    {

        // ��ȡ��ǰ��������ӳ�ʱ��
        GameObject currentObject = objectsToShow[currentIndex];

        currentObject.SetActive(true);
        currentIndex++;
        if (currentIndex < objectsToShow.Length)
        {
            
            Invoke("ShowNextObject", delay[currentIndex]);
            
        }
    }
}
