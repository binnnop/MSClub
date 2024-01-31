using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class cardInfo : MonoBehaviour,IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{


    /// <summary>
    /// �¼�ϵͳ
    /// </summary>
    public bool dragging = false;
    private bool selectMode = true;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector2 initialPosition;  // �����ʼλ��

    private cameraRotation cameraMove;

    //public delegate void CardDroppedHandler(cardInfo card);
    //public static event CardDroppedHandler OnCardDropped;

    public string major;
    public int raycastDis=1000;

    public CardManager cardManager;
    public int price;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI nameText;

    //��ʾ����
    public GameObject tip;
    public Image cardImage;
    public Color fullColor;
    public Color poorColor;
    public Color nameColor;
    private bool isFull=false;

    private bool isHovered = false;
    public Animator greenLight;
   

    void Start()
    {
  
        fullColor=new Color(1, 1, 1, 1);
        poorColor = new Color(1, 1, 1, 0.5f);

        cameraMove = GameObject.Find("Main Camera").GetComponent<cameraRotation>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        cardManager = GameObject.Find("Engine").GetComponent<CardManager>();
        cardImage = GetComponent<Image>();
        cardImage.color = poorColor;
        nameColor=nameText.color;

        initialPosition = rectTransform.anchoredPosition;
        HideTooltip();
    }

   
  


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left&&isFull)
        {
            cameraMove.isDrag = true;
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
                //��ʼ��ק״̬��Ԥ��
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
            // ���� showEffect ����
            baseObject.HideEffect();
            baseObject.HideVoid();
            cardImage.color = GetNowColor();
            moneyText.color = new Color(0, 0, 0, 1);
            nameText.color = nameColor;
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
        cameraMove.isDrag = false;
        rectTransform.anchoredPosition = initialPosition;
        Base[] bases = FindObjectsOfType<Base>();
        foreach (Base baseObject in bases)
        {
            // ���� showEffect ����
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


    //ѡȡģʽ
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
            // ��ȡ��һ����ײ���Ļ���
            Base targetBase = hits[0].collider.GetComponent<Base>();
            if (targetBase != null)
            {
                Base hitBase = targetBase.GetComponent<Base>();
                hitBase.OnCardDropped(this);
            }
            else
            {
                // ���û�з��ڻ����ϣ������ƻص�ԭλ
                rectTransform.anchoredPosition = initialPosition;
            }
        }
    }

    Color GetNowColor()
    {
        if (isFull)
            return fullColor;
        else
            return poorColor;
    }

    public void checkBaseVoid()
    {
        int layerMask = 1 << LayerMask.NameToLayer("base");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

        if (hits.Length > 0)
        {
            // ��ȡ��һ����ײ���Ļ���
            Base targetBase = hits[0].collider.GetComponent<Base>();
            if (targetBase != null && !targetBase.isVoidState)
            {
                Base hitBase = targetBase.GetComponent<Base>();
                hitBase.GenerateVoid(major);
                cardImage.color = new Color(1, 1, 1, 0);
                moneyText.color = new Color(0, 0, 0, 0);
                nameText.color = new Color(0, 0, 0, 0);
            }
            else if(targetBase == null )
            {
                print("�յ��Ǹ������ˣ�" + hits[0].collider.gameObject);
                Base[] bases = FindObjectsOfType<Base>();
                foreach (Base baseObject in bases)
                {
                    cardImage.color = GetNowColor();
                    moneyText.color = new Color(0, 0, 0, 1);
                    nameText.color = nameColor;

                    baseObject.HideVoid();
                    //print("hide");
                }
            }
        }
        else {
            //print("�������и��֣�");
            Base[] bases = FindObjectsOfType<Base>();
            foreach (Base baseObject in bases)
            {
                cardImage.color = GetNowColor();
                moneyText.color = new Color(0, 0, 0, 1);
                nameText.color = nameColor;
                baseObject.HideVoid();
                //print("hide");
            }


        }
    }

    //��ק��ÿ֡����λ��
    private void Update()
    {
        //��������ƶ�
        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            checkBaseVoid();
        }
        if (isHovered && Input.GetMouseButton(0))
        {
            HideTooltip();
        }
        if (price <= cardManager.currentMoney && !isFull)
        {
            cardImage.color = fullColor;
            isFull = true;
           //greenLight.Play("cardOK");
        }
        else if(price>cardManager.currentMoney&& isFull)
        {
            cardImage.color =poorColor;
            isFull = false;
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
        // �����ͣʱ��ʾ˵������
        cameraMove.isDrag = true;
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ����ƿ�ʱ����˵������
        cameraMove.isDrag = false;
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
