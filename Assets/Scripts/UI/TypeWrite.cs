using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeWrite : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string fullText;
    public float typingSpeed = 0.1f;
    private bool isTyping = false;
    public GameObject continueButton;
    public GameObject self;
    private float startTime;
    public bool isLoopMode;
    public float loopWait;
    public bool forcePause = false;

    private bool targetCondition;
    public bool condition_1;//判断场景有没有英雄

    void OnEnable()
    {
        Initialize();  
    }

    // Update is called once per frame
    void Update()
    {
        if (isTyping)
        {
            float elapsedTime = Time.time - startTime;
            textComponent.text = fullText.Substring(0, Mathf.Min(fullText.Length, Mathf.FloorToInt(elapsedTime / typingSpeed)));
            if (textComponent.text.Length >= fullText.Length)
            {
                isTyping = false;
                if (isLoopMode)
                {
                    Invoke("StartTyping", loopWait);
                }

                if (condition_1)
                {
                    Time.timeScale = 0f;
                }
                else if (continueButton != null)
                {
                    continueButton.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }
        if (condition_1)
        {
            GameObject i;
            i = GameObject.FindGameObjectWithTag("Hero");
            if (i != null)
            {
                Time.timeScale = 1f;
                self.SetActive(false);
            }
            
        }



    }


    public void Initialize()
    {
        forcePause = false;
        textComponent = GetComponent<TextMeshProUGUI>();
        if (isLoopMode)
            fullText = "可参加活动";
        else
            fullText = textComponent.text;
        StartTyping();
        startTime = Time.time;

       

    }

    public void PauseTyping()
    {
        isTyping = false;
        forcePause = true;
        textComponent.text = "";
    }

    public void StartTyping()
    {
        if (!forcePause)
        {
            isTyping = true;
            startTime = Time.time;
            textComponent.text = "";
 
        }
      
    }

    

}
