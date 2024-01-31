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

    public List<AudioClip> audios;
    public AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        audioSource.clip = audios[0];
        audioSource.Play();
       tips[0].SetActive(true);
        yield return new WaitForSeconds(intervals[0]);
        tips[0].SetActive(false);
        StartCoroutine(ShowTips2());
    }

    //引擎操作 （带按钮）
    IEnumerator ShowTips2()
    {
        audioSource.clip = audios[1];
        audioSource.Play();
        tips[1].SetActive(true);
        yield return new WaitForSeconds(intervals[1]);
        StartCoroutine(ShowTips3());

    }

    //底座（idle）
    IEnumerator ShowTips3()
    {
        audioSource.clip = audios[2];
        audioSource.Play();
        tips[2].SetActive(true);
        yield return new WaitForSeconds(intervals[2]);
        tips[2].SetActive(false);
        StartCoroutine(ShowTips4());
    }

    IEnumerator ShowTips4()
    {
        tips[3].SetActive(true);
        yield return new WaitForSeconds(intervals[3]);
        tips[3].SetActive(false);
        StartCoroutine(ShowTips5());
    }


    //敌人来了（带按钮）
    IEnumerator ShowTips5()
    {
        tips[4].SetActive(true);
        yield return new WaitForSeconds(intervals[4]);
        StartCoroutine(ShowTips6());
    }

    //夏露露：
    IEnumerator ShowTips6()
    {
        audioSource.clip = audios[3];
        audioSource.Play();
        heroObject.SetActive(true);
        tips[5].SetActive(true);
        yield return new WaitForSeconds(intervals[5]);
        tips[5].SetActive(false);
        StartCoroutine(ShowTips7());
    }
       
    
    IEnumerator ShowTips7()
    {
     
        tips[6].SetActive(true);
        yield return new WaitForSeconds(intervals[6]);     
        StartCoroutine(ShowTips8());

     }
    IEnumerator ShowTips8()
    {
        audioSource.clip = audios[4];
        audioSource.Play();
        tips[7].SetActive(true);
        yield return new WaitForSeconds(intervals[7]);
        tips[7].SetActive(false);
        yield return new WaitForSeconds(intervals[8]);
        StartCoroutine(ShowTips9());
    }
    IEnumerator ShowTips9()
    {
        CardManager engine = GameObject.Find("Engine").GetComponent<CardManager>();
        engine.GenerateCard("MageTower");
        engine.GenerateCard("Supporter");
        engine.ArrangeCards();

        UIObject1.SetActive(true);
        UIObject2.SetActive(true);
        UIObject3.SetActive(true);
        UIObject4.SetActive(true);
        tips[8].SetActive(true);
        yield return new WaitForSeconds(intervals[9]);
        tips[8].SetActive(false);
    
    }


}


