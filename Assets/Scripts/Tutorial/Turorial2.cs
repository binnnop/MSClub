using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turorial2 : MonoBehaviour
{
    public float[] intervals;
    public GameObject[] tips;
    public int index = 0;
    public List<AudioClip> audios;
    public AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audios[0];
        audioSource.Play();
        StartCoroutine(ShowTips());

    }

    // Update is called once per frame
    void Update()
    {

    }

    //开始转镜头
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
