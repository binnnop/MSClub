using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapInside : MonoBehaviour
{
    public string levelName;
    public int levelIndex;

    public GameObject equipArray;
    MapManager mapManager;
    public bool[] equipState;

    public Image battleImage;
    public GameObject batttleButtonObj;
    public Button battleButton;
    public Color poorColor;
    public Color originalColor;

    public Transform VOID; // 你的 VOID 物体
    public Transform cardPanel;
    //public TextMeshProUGUI voidTextPrefab; // VOID 下的 Text 组件的预制体


    void Start()
    {
        battleButton = batttleButtonObj.GetComponent<Button>();
        battleImage = batttleButtonObj.GetComponent<Image>();
        originalColor = battleImage.color;
    }
    private void OnEnable()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.deck.Clear();
        mapManager.deckList = equipArray;
        equipState = new bool[] { false,false,false,false,false, false };
        GenerateVoidObjects();


    }

    // Update is called once per frame
    void Update()
    {
        if (CheckForTrue(equipState))
        {
            battleImage.color = originalColor;
            battleButton.enabled = true;
        }
        else {
            battleImage.color =poorColor;
            battleButton.enabled =false;

        }
    }
    public void battle()
    {
        if (CheckForTrue(equipState))
        {
            mapManager.isBattle = true;
            mapManager.nowBattle = levelIndex;
            SceneManager.LoadScene(levelName);
        }
        
    }


    bool CheckForTrue(bool[] array)
    {
        // 遍历数组
        for (int i = 0; i < array.Length; i++)
        {
            // 如果找到true值，返回true
            if (array[i])
            {
                return true;
            }
        }

        // 如果没有找到true值，返回false
        return false;
    }

    public void hide()
    {
        gameObject.SetActive(false);
        RemoveVoidObjects();
    }
    public void GenerateVoidObjects()
    {
        // 获取所有子物体
        foreach (Transform childTransform in cardPanel)
        {
            // 检查子物体是否有 MapCard 组件
            mapCard mapCard = childTransform.GetComponent<mapCard>();

            if (mapCard != null)
            {
                // 检查 MapCard.index 是否小于 MapManager.GetFrontLevel()
                if (mapCard.unlockLevel > mapManager.GetFrontLevel())
                {
                    Transform voidObject = Instantiate(VOID, cardPanel);
                    voidObject.position = childTransform.position;
                    //voidObject.transform.SetParent(cardPanel);
                   // voidObject.GetComponent<RectTransform>().anchoredPosition=childTransform.GetComponent<Re>
                    TextMeshProUGUI voidText = voidObject.GetChild(0).GetComponent<TextMeshProUGUI>();
                    voidText.text = "第" + mapCard.unlockLevel + "关解锁";
                }
            }
        }
    }

    public void RemoveVoidObjects()
    {
        foreach (Transform childTransform in cardPanel)
        {
            if (childTransform.name.Contains("VOID"))
            {
               Destroy( childTransform.gameObject);
            }
        }
    }
}
