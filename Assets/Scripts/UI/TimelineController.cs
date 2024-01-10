using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
    public MonsterSpawnPortal filePortal;
    public Image fillImage;  // 拖放Slider组件到这个字段上
    public float totalTime;
    public coreController core;
    public GameObject winButton;
    public GameObject loseButton;
    public bool isGameOver=false;
    public GameObject[] needToHide;

    public bool isBossFight;
    public EnemyAI boss;


    void Start()
    {
        core = GameObject.Find("CORE").GetComponent<coreController>();
    }
    private void Update()
    {
        if (core.nowHP <= 0)
        {
            loseButton.SetActive(true);     
        }
        if (isGameOver)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");     
            if (enemies.Length == 0)
            {
                winButton.SetActive(true);      
                isGameOver = false;
            }
           
        }
        if (isBossFight && boss == null)
        {
            winButton.SetActive(true);
            
        }
    }

    public void StartTimeLine(float x) {
        totalTime = x;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        float currentTime = 0f;

        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            fillImage.fillAmount = currentTime / totalTime;
            yield return null;
        }

        isGameOver=true;
        Debug.Log("游戏结束");
    }

    void hide()
    {

        foreach (GameObject uiObject in needToHide)
        {
            uiObject.SetActive(false);
        }

        Time.timeScale = 0;
              
       }

}
