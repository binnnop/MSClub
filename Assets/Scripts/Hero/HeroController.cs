using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class HeroController : MonoBehaviour,IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public LayerMask validTowerLayer; // 有效炮塔的层
    private bool isActive = false;
    private GameObject heroEffect;
    private GameObject selectedTower;

    public GameObject hero;
    public bool born;
    private Button heroButton;
    public string heroName;
    public TextMeshProUGUI text;

    public bool dragging = false;
    public GameObject tip;
    private bool isHovered = false;

    CardManager cardManager;
    public RectTransform rectTransform;
    public Vector2 initialPosition;  // 保存初始位置



    void Start()
    {
        born = false;
        heroButton = GetComponent<Button>();
        heroButton.onClick.AddListener(ToggleHeroState);
        cardManager = GameObject.Find("Engine").GetComponent<CardManager>();
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        HideTooltip();

    }

    // Update is called once per frame
    void Update()
    {
        updateATK();

        if (dragging)
        {
            print("drag");
            Vector3 mousePos = Input.mousePosition;
            rectTransform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
        
        if (isHovered && Input.GetMouseButton(0))
        {
            HideTooltip();
        }
        


    }
    public void ToggleHeroState()
    {
        isActive = !isActive;
        if (isActive)
        {
            showEffect();
        }
        if (!isActive)
        {
            hideEffect();
        }
    }

    void activePause()
    {
        isActive = !isActive;
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


    void updateATK()
    {
        GameObject Man = GameObject.Find(heroName);
        if (Man != null)
        {
            int atk = Man.GetComponent<TowerAI>().towerAtk;
            int buff = Man.GetComponent<TowerAI>().heroRobotIncrease;
            text.text =(atk+buff).ToString();
        }
       else
            text.text = "10" ;
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
            if (targetBase != null&&targetBase.cardCount>0)
            {
                Base hitBase = targetBase.GetComponent<Base>();
                if (!born)
                {
                    hitBase.SpawnHero(hero);
                    born = true;
                }
                else
                {
                    hitBase.MoveHero(heroName);
                }
            }
            else
            {
                rectTransform.anchoredPosition = initialPosition;
            }
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
