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
    private Vector2 initialPosition;  // �����ʼλ��

    public delegate void CardDroppedHandler(cardDrag card);
    public event CardDroppedHandler OnCardDropped;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.anchoredPosition;
    }

    //��קģʽ
    public void OnBeginDrag(PointerEventData eventData)
    {
    if (eventData.button == PointerEventData.InputButton.Left)
    {
        if (!dragging)
        {
            Debug.Log("��ס������");
            dragging = true;
            selectMode = false;
            //��ʼ��ק״̬��Ԥ��
         
        }
    }
    }

    public void OnPointerUp(PointerEventData eventData) {

        canvasGroup.blocksRaycasts = true;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(rectTransform.position, rectTransform.sizeDelta, 0f);

        bool droppedOnBase = false;  // ��ǿ����Ƿ�����˻�����

        foreach (Collider2D collider in hitColliders)
        {
            Base baseObj = collider.GetComponent<Base>();
            if (baseObj != null)
            {
                // �����������ص�Ч��
                OnCardDropped?.Invoke(this);  // �����¼�
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
    Debug.Log("�ɿ�������");
    selectMode = true;
    EndThisDrag();
}

//ѡȡģʽ
public void OnPointerClick(PointerEventData eventData)
{
   
}

//��ק��ÿ֡����λ��
private void Update()
{
    if (dragging)
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
}

//ȡ����ק������ԭ��״̬
private void EndThisDrag()
{
    Debug.Log("ȡ����ק");
    dragging = false;
    
}
}