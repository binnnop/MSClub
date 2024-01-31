using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class mapCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tip;
    public MapInside mapInside;
    public MapManager mapManager;

    private bool isHovered = false;
    public float moveSpeed = 5f;
    public bool isChosen = false;

    public Transform view;
    public Vector3 initialPosition;
    public Vector3 UinitialPosition;

    public RectTransform Utransform;
    public int layIndex;
    public int unlockLevel;

    private Vector2 anchoredInitialPosition;
    

    void Start()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        Utransform = GetComponent<RectTransform>();

        view = Utransform.parent;
        // initialPosition = transform.position;
        anchoredInitialPosition = Utransform.anchoredPosition;

        //print("initial:" + transform.position + "      " + initialPosition);
        print("initial:"+Utransform.position + "                    " + UinitialPosition);
    }
    private void OnEnable()
    {
        if (isChosen)
        {
            MoveBack();
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

    void Update() {

        if (isHovered && Input.GetMouseButton(0))
        {
            tip.SetActive(false);
        }
    }

    public void Onclick() {

        if (!isChosen)
        {
            MoveToNextFalseElement();
        }
        else {

            MoveBack();
        }
        
    
    
    }

    public void MoveToNextFalseElement()
    {
        bool[] equipState = mapInside.equipState;

        // 查找第一个值为false的元素的序号
        int indexOfFalse = -1;
        for (int i = 0; i < equipState.Length; i++)
        {
            if (!equipState[i])
            {
                indexOfFalse = i;
                break;
            }
        }

        // 如果存在值为false的元素
        if (indexOfFalse != -1)
        {
            // 获取对应的子物体
            RectTransform targetTransform = mapInside.equipArray.transform.GetChild(indexOfFalse).GetComponent<RectTransform>();

            // 计算移动的目标位置
            Vector3 targetPosition = targetTransform.position;

            // 移动到目标位置
            Utransform.SetParent(Utransform.parent.parent.parent);
            StartCoroutine(MoveToPosition(targetPosition));

            // 将值为false的元素改为true
            mapInside.equipState[indexOfFalse]=true;

            isChosen = true;
            mapManager.deck.Add(gameObject.name);

            layIndex = indexOfFalse;
        }
    }

    public void MoveBack()
    {
        mapInside.equipState[layIndex] = false;

        Utransform.SetParent(view);
        //transform.position = initialPosition;
        Utransform.anchoredPosition = anchoredInitialPosition;
        //print(transform.position + "               " + initialPosition);
        print(Utransform.position + "                                    " + UinitialPosition);
        isChosen = false;
        mapManager.deck.Remove(gameObject.name);

    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(Utransform.position, targetPosition) > 0.01f)
        {
            Utransform.position = Vector3.MoveTowards(Utransform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        print("just out");
    }
}
