using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCharacter : MonoBehaviour
{
    public float fillSpeed = 0.5f;
    public float minFillAmount = 0.1f; 
    private bool isMouseOver = false;
    private Image currentFillImage;
    void Start()
    {
        GameObject[] descriptionObjects = GameObject.FindGameObjectsWithTag("description");
        foreach (GameObject obj in descriptionObjects)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckMousePosition();
        UpdateFillAmount();
    }
    void CheckMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ���������"baseHero"��ǩ�������ཻ
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("baseHero"))
        {
            // �����������µĽ�ɫ�����������Ϊ0
            if (!isMouseOver || currentFillImage == null)
            {
                isMouseOver = true;
                Transform canva = hit.transform.Find("canvas");
                Transform descriptionTransform = canva.transform.Find("description");

                // ����ҵ������������������壬��ʾ��ǰ�������壬�������������Ϊ0
                if (descriptionTransform != null)
                {
                    GameObject[] descriptionObjects = GameObject.FindGameObjectsWithTag("description");
                    foreach (GameObject obj in descriptionObjects)
                    {
                        obj.SetActive(false);
                    }

                    descriptionTransform.gameObject.SetActive(true);
                    currentFillImage = descriptionTransform.GetComponent<Image>();
                    if (currentFillImage != null)
                    {
                        currentFillImage.fillAmount = 0f;
                    }
                }
            }
        }
        else
        {
            isMouseOver = false;
        }


     
    }
    void UpdateFillAmount()
    {
        // �������ڽ�ɫ��
        if (isMouseOver && currentFillImage != null)
        {
            // �����������
            currentFillImage.fillAmount += fillSpeed * Time.deltaTime;
        }
        else if (currentFillImage != null)
        {
            // �𽥼��������
            currentFillImage.fillAmount -= fillSpeed * Time.deltaTime;

            // ��������������Сֵ����������image
            if (currentFillImage.fillAmount <= minFillAmount)
            {
                currentFillImage.gameObject.SetActive(false);
                currentFillImage = null;
            }
        }
    }

    
}
