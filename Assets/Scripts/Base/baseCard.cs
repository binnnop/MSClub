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
