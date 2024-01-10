using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    public bool isDoubleSpeed = false;
    public Button doubleSpeedButton;
    public Image imageB;
    public float timeScale;
    public PauseGame pause;

    void Start()
    {
       
        imageB = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleDoubleSpeed();
        }
   

    }

   public void ToggleDoubleSpeed()
    {
        print("toggle!");
        isDoubleSpeed = !isDoubleSpeed;
        if (isDoubleSpeed)
        {
            pause.isPaused = false;
            Time.timeScale = 2f;
            imageB.color = new Color(1, 0, 0, 1);
        }

        else
        {
            Time.timeScale = 1.0f;
            imageB.color = new Color(0, 0, 0, 1);
        }
           

    }



}
