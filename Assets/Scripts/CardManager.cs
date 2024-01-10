using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static cardInfo;
using TMPro;

public class CardManager : MonoBehaviour
{
    public Text cardInfoText;
    public Transform cardHolder;
    //public GameObject[] cardPrefabs; 
    public int maxCards = 8;
    public float cardGenerationInterval = 10f;
    public float nowCardGenerationInterval = 10f;
    public float cardSpacing = 1f;

    private float timeUntilNextCard;
    private bool isGenerating = true;
    public Text countdownText;

    public  float minCardGenerationInterval = 4f;
    public bool sideType;

    public int initialMoney = 100;
    public int incomePerSecond = 10;
     int incomePerFortress = 1;
    public int currentMoney;
    private float timer;
    public TextMeshProUGUI moneyText;
    public int maxLayer;
    Manager manager;
    test test;
    public float shovelPrice = 0.8f;

    void Start()
    {
        nowCardGenerationInterval = cardGenerationInterval;
        test = GameObject.Find("test").GetComponent<test>();
        if(!test.isTestMode)
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        
        if (manager != null)
        {
            GenerateCard(manager.equippedTower[0]);
            GenerateCard(manager.equippedTower[1]);
            GenerateCard(manager.equippedTower[2]);
            GenerateCard(manager.equippedTower[3]);
            ArrangeCards();
        }
        else {
            GenerateCard(test.equippedTower[0]);
            GenerateCard(test.equippedTower[1]);
            GenerateCard(test.equippedTower[2]);
            GenerateCard(test.equippedTower[3]);
            ArrangeCards();
        }

        currentMoney = initialMoney;
        UpdateMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f) // 每秒更新一次
        {
            GameObject[] fortressObjects = GameObject.FindGameObjectsWithTag("Fortress");
            int fortressCount = fortressObjects.Length;
            int totalIncome = incomePerSecond + (fortressCount* incomePerFortress);
            currentMoney += totalIncome;
            UpdateMoneyText();
            timer = 0f;
        }
    }

  

 
    void GenerateCard(string type)
    {

        // 根据卡牌类型和花色生成卡牌对象
        //GameObject cardPrefab = GetCardPrefab(type);
        if (!test.isTestMode)
        {
            foreach (GameObject cardPrefab in manager.cardPrefabs)
            {
                if (cardPrefab.name.Contains(type))
                {
                    GameObject newCard = Instantiate(cardPrefab, cardHolder);
                }
            }
        }
        else {
            foreach (GameObject cardPrefab in test.cardPrefabs)
            {
                if (cardPrefab.name.Contains(type))
                {
                    GameObject newCard = Instantiate(cardPrefab, cardHolder);
                }
            }


        }
        

    }

    public void ArrangeCards()
    {
        // 重新排列卡牌的位置
        int cardCount = cardHolder.childCount;
        for (int i = 0; i < cardCount; i++)
        {
            Transform card = cardHolder.GetChild(i);
            float yPos = i * cardSpacing;
            if(sideType)
            card.localPosition = new Vector3(0f, -yPos, 0f);
            else
                card.localPosition = new Vector3(yPos, 0f, 0f);
        }
    }

    T GetRandomEnumValue<T>()
    {
        System.Array enumValues = System.Enum.GetValues(typeof(T));
        System.Random random = new System.Random();
        T randomEnumValue = (T)enumValues.GetValue(random.Next(enumValues.Length));
        return randomEnumValue;
    }

   

    

    public void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text =  currentMoney.ToString();
        }
    }

}
