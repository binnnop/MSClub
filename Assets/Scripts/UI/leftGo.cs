using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class leftGo : MonoBehaviour
{
    Animator anim;
    public Button b;

    void Start()
    {
        anim = GetComponent<Animator>();
        b = transform.GetChild(3).GetComponent<Button>();
        if (b != null)
        {
            b.onClick.AddListener(left);
        }
    }

    
    void Update()
    {
        
    }

    void pause()
    {
        anim.speed = 0;   
        b.enabled = true;
    }

    void left()
    {
        transform.GetChild(3).gameObject.SetActive(false);
        anim.speed = 1;
    }
}
