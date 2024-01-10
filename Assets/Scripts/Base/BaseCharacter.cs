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

        // 如果射线与"baseHero"标签的物体相交
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("baseHero"))
        {
            // 如果鼠标移入新的角色，重置填充量为0
            if (!isMouseOver || currentFillImage == null)
            {
                isMouseOver = true;
                Transform canva = hit.transform.Find("canvas");
                Transform descriptionTransform = canva.transform.Find("description");

                // 如果找到，隐藏其他描述物体，显示当前描述物体，并将填充量重置为0
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
        // 如果鼠标在角色上
        if (isMouseOver && currentFillImage != null)
        {
            // 逐渐增加填充量
            currentFillImage.fillAmount += fillSpeed * Time.deltaTime;
        }
        else if (currentFillImage != null)
        {
            // 逐渐减少填充量
            currentFillImage.fillAmount -= fillSpeed * Time.deltaTime;

            // 如果填充量低于最小值，隐藏描述image
            if (currentFillImage.fillAmount <= minFillAmount)
            {
                currentFillImage.gameObject.SetActive(false);
                currentFillImage = null;
            }
        }
    }

    
}
