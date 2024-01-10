using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    public string levelID;
    public bool isMain;


    void Start()
    {
        Button button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(LoadSceneOnClick);
        }
        else
        {
            Debug.LogError("该脚本需要挂载到Button组件上");
        }
    }
    private void LoadSceneOnClick()
    {
        Manager manager = GameObject.Find("Manager").GetComponent<Manager>();
        manager.EnterLevel(this);
       
    }
}
