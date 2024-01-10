using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weigh : MonoBehaviour
{
    public Transform[] leftItems;
    public Transform[] rightItems;
    Animator anim;

    private int previousBalanceState = 0; // 0: Balanced, 1: Left Heavy, -1: Right Heavy
    private bool isAnimating = false;


    private void Start()
    {
       anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAnimating)
        {
            int leftWeight = CalculateWeight(leftItems);
            int rightWeight = CalculateWeight(rightItems);

            if (leftWeight > rightWeight && previousBalanceState != 1)
            {
                anim.Play("weigh");
                previousBalanceState = 1;
            }
            else if (leftWeight < rightWeight && previousBalanceState != -1)
            {
                anim.Play("weighRight");
                previousBalanceState = -1;
            }
            else if (leftWeight == rightWeight && previousBalanceState != 0)
            {
                if (previousBalanceState == 1)
                    anim.Play("breakLeft");
                else if (previousBalanceState == -1)
                    anim.Play("breakRight");

                previousBalanceState = 0;
            }
        }
    }



    // 情况1：天平向左下沉
    public IEnumerator Weigh()
    {
        isAnimating = true;     
        yield return new WaitForSeconds(2.0f); // 用于模拟动画时间
        isAnimating = false;
    }

    // 情况2：天平向右下沉
    public IEnumerator WeighRight()
    {
        isAnimating = true;
        Debug.Log("Weighing right...");
        // 播放右下沉动画的代码...
        yield return new WaitForSeconds(2.0f); // 用于模拟动画时间
        Debug.Log("Weighing right completed.");
        isAnimating = false;
    }

    // 情况3：从向左下沉恢复到平衡
    public IEnumerator BreakLeft()
    {
        isAnimating = true;
        Debug.Log("Breaking left...");
        // 播放恢复平衡的动画代码...
        yield return new WaitForSeconds(2.0f); // 用于模拟动画时间
        Debug.Log("Breaking left completed.");
        isAnimating = false;
    }

    // 情况4：从向右下沉恢复到平衡
    public IEnumerator BreakRight()
    {
        isAnimating = true;
        Debug.Log("Breaking right...");
        // 播放恢复平衡的动画代码...
        yield return new WaitForSeconds(2.0f); // 用于模拟动画时间
        Debug.Log("Breaking right completed.");
        isAnimating = false;
    }

    

    int CalculateWeight(Transform[] items)
    {
        int totalWeight = 0;
        foreach (Transform item in items)
        {
            totalWeight += item.GetComponent<Base>().cardCount; // 假设Base是item上的组件
            foreach (Transform child in item)
            {         
                if (child.CompareTag("Hero"))
                {
                    totalWeight++;
                }             
            }
        }
        return totalWeight;
    }



}
