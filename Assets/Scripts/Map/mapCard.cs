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
        // �����ͣʱ��ʾ˵������      
            ShowTooltip();
   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ����ƿ�ʱ����˵������
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

        // ���ҵ�һ��ֵΪfalse��Ԫ�ص����
        int indexOfFalse = -1;
        for (int i = 0; i < equipState.Length; i++)
        {
            if (!equipState[i])
            {
                indexOfFalse = i;
                break;
            }
        }

        // �������ֵΪfalse��Ԫ��
        if (indexOfFalse != -1)
        {
            // ��ȡ��Ӧ��������
            RectTransform targetTransform = mapInside.equipArray.transform.GetChild(indexOfFalse).GetComponent<RectTransform>();

            // �����ƶ���Ŀ��λ��
            Vector3 targetPosition = targetTransform.position;

            // �ƶ���Ŀ��λ��
            Utransform.SetParent(Utransform.parent.parent.parent);
            StartCoroutine(MoveToPosition(targetPosition));

            // ��ֵΪfalse��Ԫ�ظ�Ϊtrue
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
