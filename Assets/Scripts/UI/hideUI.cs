using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideUI : MonoBehaviour
{
    public GameObject[] needToHide;
    TimelineController timeline;

    void Start()
    {
       timeline = GameObject.Find("Engine").GetComponent<TimelineController>();
   
    }

    // Update is called once per frame
    void hide()
    {
        needToHide= timeline.needToHide;

        foreach (GameObject uiObject in needToHide)
        {
            uiObject.SetActive(false);
        }

        Time.timeScale = 0;

    }
}
