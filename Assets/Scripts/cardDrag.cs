using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cardDrag : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerUpHandler
{ 
    public bool dragging = false;
    private bool selectMode = true;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 initialPosition;  // 保存初始位置

    public delegate void CardDroppedHandler(cardDrag card);
    public event CardDroppedHandler OnCardDropped;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.anchoredPosition;
    }

    //拖拽模式
    public void OnBeginDrag(PointerEventData eventData)
    {
    if (eventData.button == PointerEventData.InputButton.Left)
    {
        if (!dragging)
        {
            Debug.Log("按住鼠标左键");
            dragging = true;
            selectMode = false;
            //开始拖拽状态的预览
         
        }
    }
    }

    public void OnPointerUp(PointerEventData eventData) {

        canvasGroup.blocksRaycasts = true;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(rectTransform.position, rectTransform.sizeDelta, 0f);

        bool droppedOnBase = false;  // 标记卡牌是否放在了基地上

        foreach (Collider2D collider in hitColliders)
        {
            Base baseObj = collider.GetComponent<Base>();
            if (baseObj != null)
            {
                // 触发与基地相关的效果
                OnCardDropped?.Invoke(this);  // 调用事件
                droppedOnBase = true;
                break;
            }
        }

        if (!droppedOnBase)
        {
            rectTransform.anchoredPosition = initialPosition;
        }

    }







public void OnDrag(PointerEventData eventData)
{
}
public void OnEndDrag(PointerEventData eventData)
{
    Debug.Log("松开鼠标左键");
    selectMode = true;
    EndThisDrag();
}

//选取模式
public void OnPointerClick(PointerEventData eventData)
{
   
}

//拖拽中每帧更新位置
private void Update()
{
    if (dragging)
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
}

//取消拖拽，返回原来状态
private void EndThisDrag()
{
    Debug.Log("取消拖拽");
    dragging = false;
    
}
}