using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    
    public GameObject heroObject;
    public GameObject UIObject1;
    public GameObject UIObject2;
    public GameObject UIObject3;
    public GameObject UIObject4;

    public float[] intervals;
    public GameObject[] tips;


    

    void Start()
    {
        heroObject.SetActive(false);
        UIObject1.SetActive(false);
        UIObject2.SetActive(false);
        UIObject3.SetActive(false);
        UIObject4.SetActive(false);
        StartCoroutine(ShowTips1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //开始转镜头
    IEnumerator ShowTips1()
     {
       tips[0].SetActive(true);
        yield return new WaitForSeconds(intervals[0]);
        tips[0].SetActive(false);
        StartCoroutine(ShowTips2());
    }

    //引擎操作 （带按钮）
    IEnumerator ShowTips2()
    {
        tips[1].SetActive(true);
        yield return new WaitForSeconds(intervals[1]);
        StartCoroutine(ShowTips3());

    }

    //底座（idle）
    IEnumerator ShowTips3()
    {
        tips[2].SetActive(true);
        yield return new WaitForSeconds(intervals[2]);
        tips[2].SetActive(false);
        StartCoroutine(ShowTips4());
    }

    //敌人来了（带按钮）
    IEnumerator ShowTips4()
    {
        tips[3].SetActive(true);
        yield return new WaitForSeconds(intervals[3]);
        StartCoroutine(ShowTips5());
    }

    //夏露露：
    IEnumerator ShowTips5()
    {
        heroObject.SetActive(true);
        tips[4].SetActive(true);
        yield return new WaitForSeconds(intervals[4]);
        tips[4].SetActive(false);
        StartCoroutine(ShowTips6());
    }
       
    
    IEnumerator ShowTips6()
    {
     
        tips[5].SetActive(true);
        yield return new WaitForSeconds(intervals[5]);
       
        StartCoroutine(ShowTips7());

     }
    IEnumerator ShowTips7()
    {
        UIObject1.SetActive(true);
        UIObject2.SetActive(true);
        UIObject3.SetActive(true);
        UIObject4.SetActive(true);
        tips[6].SetActive(true);
        yield return new WaitForSeconds(intervals[6]);
        tips[6].SetActive(false);
        yield return new WaitForSeconds(intervals[7]);
        StartCoroutine(ShowTips8());
    }
    IEnumerator ShowTips8()
    {
        tips[7].SetActive(true);
        yield return new WaitForSeconds(intervals[8]);
        tips[7].SetActive(false);
    
    }


}


