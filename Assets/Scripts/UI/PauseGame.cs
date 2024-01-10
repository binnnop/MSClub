using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public  bool isPaused = false;
    public SpeedController doubleSpeed;
    private void Start()
    {
        doubleSpeed = GameObject.Find("SpeedControl").GetComponent<SpeedController>();
       
    }

    void Update()
    {
        // ¼àÌý°´Å¥µã»÷
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }

    }

    public void TogglePause()
    {
        print("pausedToggle");
        // ÇÐ»»ÔÝÍ£×´Ì¬
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            doubleSpeed.isDoubleSpeed = false;
        }
            
        else
            Time.timeScale = 1f;
        
        
    }

    
}
