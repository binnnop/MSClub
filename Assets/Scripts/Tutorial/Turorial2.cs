using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turorial2 : MonoBehaviour
{
    public float[] intervals;
    public GameObject[] tips;
    public int index = 0;


    void Start()
    {    
        StartCoroutine(ShowTips());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //��ʼת��ͷ
    IEnumerator ShowTips()
    {     
        yield return new WaitForSeconds(intervals[index*2]);
        tips[index].SetActive(true);
        yield return new WaitForSeconds(intervals[index*2+1]);
        tips[index].SetActive(false);
        index++;
        if(index<tips.Length)
        StartCoroutine(ShowTips());
    }
    



}
