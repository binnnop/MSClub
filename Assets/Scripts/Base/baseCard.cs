using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class baseCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject tip;
    private bool isHovered = false;

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
}
