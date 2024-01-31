using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class MapLevel : MonoBehaviour
{
    public GameObject popUp;
    public Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPopup()
    {
        popUp.SetActive(true);
    }
    public void DisableOutline()
    {
        if (outline.enabled)
        {
            outline.enabled = false;
        }
    }
}
