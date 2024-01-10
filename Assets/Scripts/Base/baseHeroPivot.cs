using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseHeroPivot : MonoBehaviour
{
    public string heroName;
    public string animationName;
    public bool canActivity = false;
    public GameObject activityTips;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showActivity()
    {
        activityTips.SetActive(true);
        canActivity = true;
    }
    public void hideActivity()
    {
        activityTips.SetActive(false);
        canActivity = false;
    }
}
