using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerBreaker : MonoBehaviour
{
    private Animator bossAnimator;
    private float timer = 0f;
    public float animationInterval = 30f;
    public Base[] bases;

    void Start()
    {
        bossAnimator = transform.GetChild(0).GetComponent<Animator>();

        // Æô¶¯¶¨Ê±Æ÷
        InvokeRepeating("PlayDownAnimation", 0f, animationInterval);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
    private void PlayDownAnimation()
    {
        bossAnimator.Play("down");
        timer = 0f;
        Invoke("DecreaseAllBases",1.5f);
       
    }

    private void DecreaseAllBases()
    {
        foreach (Base b in bases)
        {
            b.Decrease();
        }
    }
}
