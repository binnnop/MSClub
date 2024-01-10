using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class shovel : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler
{

    public LayerMask validTowerLayer; // 有效炮塔的层

    public bool dragging = false;
    public GameObject tip;
    public GameObject priceImage;
    public TextMeshProUGUI priceText;
    private bool isHovered = false;

    CardManager cardManager;
    public RectTransform rectTransform;
    public Vector2 initialPosition;  // 保存初始位置
    void Start()
    {
        
        cardManager = GameObject.Find("Engine").GetComponent<CardManager>();
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        HideTooltip();

    }

    void Update()
    {

        if (dragging)
        {
            print("drag");
            Vector3 mousePos = Input.mousePosition;
            rectTransform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);

            int layerMask = 1 << LayerMask.NameToLayer("base");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

            if (hits.Length > 0)
            {
                
                Base targetBase = hits[0].collider.GetComponent<Base>();
                if (targetBase != null && targetBase.cardCount > 0)
                {
                    priceImage.SetActive(true);
                    priceText.text = "返还金额：" + targetBase.caculatingValue();
                }
                else
                {
                    priceImage.SetActive(false);
                }
            }
        }

        if (isHovered && Input.GetMouseButton(0))
        {
            HideTooltip();
        }



    }
    

    void showEffect()
    {
        print("really showed");
        Base[] bases = FindObjectsOfType<Base>();
        foreach (Base baseObject in bases)
        {
            // 触发 showEffect 方法
            baseObject.ShowHeroEffect();
        }

    }


    void hideEffect()
    {
        Base[] bases = FindObjectsOfType<Base>();
        foreach (Base baseObject in bases)
        {
            // 触发 showEffect 方法
            baseObject.HideEffect();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Base[] bases = FindObjectsOfType<Base>();
            foreach (Base baseObject in bases)
            {
                baseObject.ShowHeroEffect();
            }

            if (!dragging)
            {
                dragging = true;
            }
        }

        

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        int layerMask = 1 << LayerMask.NameToLayer("base");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

        if (hits.Length > 0)
        {
            // 获取第一个碰撞到的基地
            Base targetBase = hits[0].collider.GetComponent<Base>();
            if (targetBase != null && targetBase.cardCount > 0)
            {
                Base hitBase = targetBase.GetComponent<Base>();
                hitBase.Shoveled();     
            }
            else
            {
                rectTransform.anchoredPosition = initialPosition;
            }

        }
        priceImage.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = initialPosition;
        Base[] bases = FindObjectsOfType<Base>();
        foreach (Base baseObject in bases)
        {
            baseObject.HideEffect();
        }
        Fire[] fires = FindObjectsOfType<Fire>();
        foreach (Fire fireObject in fires)
        {
            fireObject.HideEffect();
        }
        dragging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    void ShowTooltip()
    {
        if (tip != null)
        {
            tip.SetActive(true);
            isHovered = true;
        }
    }

    void HideTooltip()
    {
        if (tip != null)
        {
            tip.SetActive(false);
            isHovered = false;
        }
    }

}
