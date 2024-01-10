using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class cardInfo : MonoBehaviour,IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{


    /// <summary>
    /// 事件系统
    /// </summary>
    public bool dragging = false;
    private bool selectMode = true;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector2 initialPosition;  // 保存初始位置

    //public delegate void CardDroppedHandler(cardInfo card);
    //public static event CardDroppedHandler OnCardDropped;

    public string major;
    public int raycastDis=1000;

    public CardManager cardManager;
    public int price;
    public TextMeshProUGUI moneyText;

    //提示文字
    public GameObject tip;
    private bool isHovered = false;
   

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        cardManager = GameObject.Find("Engine").GetComponent<CardManager>();
        initialPosition = rectTransform.anchoredPosition;
        HideTooltip();
    }

   
  


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Base[] bases = FindObjectsOfType<Base>();
            foreach (Base baseObject in bases)
            {
                baseObject.ShowEffect(price);
            }
            Fire[] fires = FindObjectsOfType<Fire>();
            foreach (Fire fireObject in fires)
            {
                fireObject.ShowEffect();
            }
            if (!dragging)
            {    
                dragging = true;
                selectMode = false;
                //开始拖拽状态的预览
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    { 
        canvasGroup.blocksRaycasts = true;

        int layerMask0 = 1 << LayerMask.NameToLayer("Fire");
        Ray ray0 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits0 = Physics.RaycastAll(ray0, Mathf.Infinity, layerMask0);
        if (hits0.Length > 0)
        {
            Fire targetFire = hits0[0].collider.GetComponentInParent<Fire>();
            if (targetFire != null)
            {
                if (cardManager.currentMoney >= price)
                {
                    cardManager.currentMoney -= price;
                    cardManager.UpdateMoneyText();
                    /*
                    if (major =="Fortress")
                    {
                        price += 50;
                        UpdateMoneyText();
                    }
                    */
                    Fire hitFire = targetFire.GetComponentInParent<Fire>();
                    hitFire.Atkbuff(this);
                }

                rectTransform.anchoredPosition = initialPosition;
                
            }
            else
            {
                checkBaseCollider();
            }
        }
        else checkBaseCollider();





        Base[] bases = FindObjectsOfType<Base>();
        foreach (Base baseObject in bases)
        {
            // 触发 showEffect 方法
            baseObject.HideEffect();
        }
        Fire[] fires = FindObjectsOfType<Fire>();
        foreach (Fire fireObject in fires)
        {
            fireObject.HideEffect();
        }
        cardManager.ArrangeCards();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = initialPosition;
        Base[] bases = FindObjectsOfType<Base>();
        foreach (Base baseObject in bases)
        {
            // 触发 showEffect 方法
            baseObject.HideEffect();
        }
        Fire[] fires = FindObjectsOfType<Fire>();
        foreach (Fire fireObject in fires)
        {
            fireObject.HideEffect();
        }
        selectMode = true;
        dragging = false;
        //cardManager.ArrangeCards();
    }


    //选取模式
    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void checkBaseCollider()
    {
        int layerMask = 1 << LayerMask.NameToLayer("base");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

        if (hits.Length > 0)
        {
            // 获取第一个碰撞到的基地
            Base targetBase = hits[0].collider.GetComponent<Base>();
            if (targetBase != null)
            {
                Base hitBase = targetBase.GetComponent<Base>();
                hitBase.OnCardDropped(this);
            }
            else
            {
                // 如果没有放在基地上，将卡牌回到原位
                rectTransform.anchoredPosition = initialPosition;
            }
        }
    }

    //拖拽中每帧更新位置
    private void Update()
    {
        //跟随鼠标移动
        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
        if (isHovered && Input.GetMouseButton(0))
        {
            HideTooltip();
        }
    }


    public void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = price.ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标悬停时显示说明文字
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标移开时隐藏说明文字
        HideTooltip();
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
