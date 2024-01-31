using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectDelay : MonoBehaviour
{
    public GameObject[] objectsToShow; // 物体数组
    public int[] delay; 
    private int currentIndex = 0; 

    void Start()
    {
        Invoke("ShowNextObject", delay[currentIndex]);
    }

    void ShowNextObject()
    {

        // 获取当前物体和其延迟时间
        GameObject currentObject = objectsToShow[currentIndex];

        currentObject.SetActive(true);
        currentIndex++;
        if (currentIndex < objectsToShow.Length)
        {
            
            Invoke("ShowNextObject", delay[currentIndex]);
            
        }
    }
}
