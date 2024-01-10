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



    // ���1����ƽ�����³�
    public IEnumerator Weigh()
    {
        isAnimating = true;     
        yield return new WaitForSeconds(2.0f); // ����ģ�⶯��ʱ��
        isAnimating = false;
    }

    // ���2����ƽ�����³�
    public IEnumerator WeighRight()
    {
        isAnimating = true;
        Debug.Log("Weighing right...");
        // �������³������Ĵ���...
        yield return new WaitForSeconds(2.0f); // ����ģ�⶯��ʱ��
        Debug.Log("Weighing right completed.");
        isAnimating = false;
    }

    // ���3���������³��ָ���ƽ��
    public IEnumerator BreakLeft()
    {
        isAnimating = true;
        Debug.Log("Breaking left...");
        // ���Żָ�ƽ��Ķ�������...
        yield return new WaitForSeconds(2.0f); // ����ģ�⶯��ʱ��
        Debug.Log("Breaking left completed.");
        isAnimating = false;
    }

    // ���4���������³��ָ���ƽ��
    public IEnumerator BreakRight()
    {
        isAnimating = true;
        Debug.Log("Breaking right...");
        // ���Żָ�ƽ��Ķ�������...
        yield return new WaitForSeconds(2.0f); // ����ģ�⶯��ʱ��
        Debug.Log("Breaking right completed.");
        isAnimating = false;
    }

    

    int CalculateWeight(Transform[] items)
    {
        int totalWeight = 0;
        foreach (Transform item in items)
        {
            totalWeight += item.GetComponent<Base>().cardCount; // ����Base��item�ϵ����
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
