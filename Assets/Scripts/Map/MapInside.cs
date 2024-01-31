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

    public Transform VOID; // ��� VOID ����
    public Transform cardPanel;
    //public TextMeshProUGUI voidTextPrefab; // VOID �µ� Text �����Ԥ����


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
        // ��������
        for (int i = 0; i < array.Length; i++)
        {
            // ����ҵ�trueֵ������true
            if (array[i])
            {
                return true;
            }
        }

        // ���û���ҵ�trueֵ������false
        return false;
    }

    public void hide()
    {
        gameObject.SetActive(false);
        RemoveVoidObjects();
    }
    public void GenerateVoidObjects()
    {
        // ��ȡ����������
        foreach (Transform childTransform in cardPanel)
        {
            // ����������Ƿ��� MapCard ���
            mapCard mapCard = childTransform.GetComponent<mapCard>();

            if (mapCard != null)
            {
                // ��� MapCard.index �Ƿ�С�� MapManager.GetFrontLevel()
                if (mapCard.unlockLevel > mapManager.GetFrontLevel())
                {
                    Transform voidObject = Instantiate(VOID, cardPanel);
                    voidObject.position = childTransform.position;
                    //voidObject.transform.SetParent(cardPanel);
                   // voidObject.GetComponent<RectTransform>().anchoredPosition=childTransform.GetComponent<Re>
                    TextMeshProUGUI voidText = voidObject.GetChild(0).GetComponent<TextMeshProUGUI>();
                    voidText.text = "��" + mapCard.unlockLevel + "�ؽ���";
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
