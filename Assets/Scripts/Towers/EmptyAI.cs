using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class EmptyAI : MonoBehaviour
{
    public GameObject greenArea;
    public Transform model;

    public void SetGreenAreaVisibility(bool visible)
    {
        // ���� GreenArea �Ŀɼ���
        if (greenArea != null)
        {
            greenArea.SetActive(visible);
            Outline outline = model.GetComponent<Outline>();
            outline.enabled = visible;
        }

    }

}
