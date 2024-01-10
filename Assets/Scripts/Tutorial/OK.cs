using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK : MonoBehaviour
{
    public void continueGame()
    {
        Time.timeScale = 1.0f;
        transform.parent.gameObject.SetActive(false);
    }
}
