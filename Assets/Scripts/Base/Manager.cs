using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]
public class DailyUnlockData
{
    public string day;
    public List<string> unlockedLevels = new List<string>();
}


public class Manager : MonoBehaviour
{
    private static Manager instance;

    public int totalDays = 32;
    public int currentDay = 1;
    public List<int> friendship;
    public List<string> equippedTower;

    public int initialBattlePoints = 1;
    public int currentBattlePoints;
    public int currentActivityPoints;

    public List<string> availableCharacters = new List<string>();
    public Text dialogueText;

    public baseHeroPivot[] heroes;
    public Vector2[] activityTable;
    public List<DailyUnlockData> dailyUnlockDataList = new List<DailyUnlockData>();


    public TextMeshProUGUI dayText;
    public TextMeshProUGUI activityPointText;
    public TextMeshProUGUI battlePointText;

    public GameData data;
    private string savePath;

    //��������
    private bool isTransitioning = false;
    public GameObject transitionImage;
    public float fadeSpeed = 1.5f;
    public bool sceneStarting = false;
    public static bool sceneEnding = false;
    public static bool isLoading = false;
    private Image transition;
    public float waitTime;

    public bool isBattling = false;
    public string currentExitLevel;

    public float levelOffsetX;
    public float levelOffsetY;
    public List<string> levelList;
    public GameObject[] towerPrefabs;
    public GameObject[] cardPrefabs;
    public Material[] skyboxMaterials;

    public List<string> unlockedTowerList;
    public List<GameObject> unlockTower;

    // ����ģʽ
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        savePath = Application.persistentDataPath + "/saveData.json";
        transition = transitionImage.GetComponent<Image>();
        unlockedTowerList = new List<string>{ "Fortress", "Cannon", "MageTower", "Supporter" };
        StartDay();
    }
    void Update()
    {
        #region �жϺ�����������״̬
        if (sceneStarting)
        {
            StartScene();
        }

        if (sceneEnding)
        {
            EndScene();
        }
        #endregion
    }

    void StartDay()
    {
        initialPoints();
        ShowAvailableCharacters();
        Invoke("ShowNewTower", 1f);
        addLevel();
        Invoke("sortLevel", 1f);
        updateText();
        ChangeSkybox();
    }

    void LoadToStart()
    {
        #region Ѱ�ҳ����е����Ͻ����֡�baseHeroPivot
        dayText = GameObject.Find("Day").GetComponent<TextMeshProUGUI>();
        activityPointText = GameObject.Find("ActivityPoint").GetComponent<TextMeshProUGUI>();
        battlePointText = GameObject.Find("BattlePoint").GetComponent<TextMeshProUGUI>();
        heroes[0] = GameObject.Find("moon1").transform.GetComponentInChildren<baseHeroPivot>();
        heroes[1] = GameObject.Find("�����").transform.GetComponentInChildren<baseHeroPivot>();
        heroes[2] = GameObject.Find("��ļ").transform.GetComponentInChildren<baseHeroPivot>();
        heroes[3] = GameObject.Find("����").transform.GetComponentInChildren<baseHeroPivot>();
        #endregion

        if (currentActivityPoints>0)
        ShowAvailableCharacters();
        updateText();
        loadLevel();
        Invoke("sortLevel", 0.1f);
        //����ս��״̬
        if (isBattling)
        {
            isBattling = false;
            currentBattlePoints--;
            updateText();
        }
        Invoke("HandleBattlePointsAndLevels", 0.1f);
        isLoading = false;
        ChangeSkybox();
    }


    #region  ��ʾӢ�۵ġ��ɲμӻ��ͼ��
    void ShowAvailableCharacters()
    {
        updateText();
        int firstHero = Mathf.FloorToInt(activityTable[currentDay-1].x);
        int secondHero = Mathf.FloorToInt(activityTable[currentDay-1].y);
        heroes[firstHero].showActivity();
        heroes[secondHero].showActivity();

    }

    void invokeShow()
    {
        unlockTower[4].SetActive(true);
    }

    void ShowNewTower()
    {
        print(currentDay);
        if (currentDay ==1)
        {
            Invoke("invokeShow", 1f);
        }
        if (currentDay == 2)
        {
            unlockTower[0].SetActive(true);
            unlockedTowerList.Add("Mini");
        }
        if (currentDay == 3)
        {
            unlockTower[1].SetActive(true);
            unlockedTowerList.Add("Gold");
        }
        if (currentDay == 4)
        {
            unlockTower[2].SetActive(true);
            unlockedTowerList.Add("Sentry");
        }
        if (currentDay == 5)
        {
            print("day5");
            unlockTower[3].SetActive(true);
            unlockedTowerList.Add("Blaze");
        }
        if (currentDay == 6)
        { 
            unlockTower[5].SetActive(true);
        }

    }

    public void HideAvailableCharacters()
    {
        int firstHero = Mathf.FloorToInt(activityTable[currentDay - 1].x);
        int secondHero = Mathf.FloorToInt(activityTable[currentDay - 1].y);
        heroes[firstHero].hideActivity();
        heroes[secondHero].hideActivity();
    }
    #endregion

    public void ConsumeActivityPoints(int heroIndex)
    {
        if (currentActivityPoints > 0 && friendship[heroIndex] < 7)
        {
            currentActivityPoints--;
            friendship[heroIndex]++;
            HideAvailableCharacters();
            updateText();
            BaseCamera camera = GameObject.Find("Main Camera").GetComponent<BaseCamera>();
            if (camera.isZoomPaused)
            {
                camera.nowHero = null;
                camera.reSetCamera();
            }
        }    
        if (currentBattlePoints == 0 && currentActivityPoints == 0)
        {
            EndDay();
        }
    }

    public void HandleBattlePointsAndLevels()
    { 
        if (currentBattlePoints == 0 && currentActivityPoints == 0)
        {
            EndDay();
        }
    }

    void EndDay()
    {   
        if (currentDay < totalDays)
        {
            transitionImage.SetActive(true);
            BaseCamera camera = GameObject.Find("Main Camera").GetComponent<BaseCamera>();
            camera.reSetCamera();
            sceneEnding = true;
            currentDay++;
            StartDay();
        }
        else
        {
            Debug.Log("Game Over");
        }
    }


    void updateText()
    {
        dayText.text = "DAY" + currentDay.ToString();
        activityPointText.text = currentActivityPoints.ToString();
        battlePointText.text = currentBattlePoints.ToString();
        HeroPanel[] heroPanels = FindObjectsOfType<HeroPanel>();
        foreach (HeroPanel heroPanel in heroPanels)
        {
            heroPanel.updateText();
        }
    }

    void initialPoints()
    {
        currentActivityPoints = 1;
        currentBattlePoints = 99;
        if (friendship.Count == 0)
        {
           friendship.Add(0);
           friendship.Add(0);
           friendship.Add(0);
           friendship.Add(0);
        }
        if (equippedTower.Count == 0)
        {
            equippedTower.Add("Fortress");
            equippedTower.Add("Cannon");
            equippedTower.Add("MageTower");
            equippedTower.Add("Supporter");
        }
    }

    public void EnterLevel(LevelInfo levelInfo)
    {
        string levelName = levelInfo.levelID;
        if (currentBattlePoints > 0)
        {
            GameObject levelHolder = GameObject.Find("levelHolder");
            LevelInfo[] levelButtons = levelHolder.GetComponentsInChildren<LevelInfo>();
            foreach (var level in levelButtons)
            {
                if (level.levelID != levelName && level.isMain)
                {
                    Debug.Log("������������߹ؿ���");
                    return; 
                }
            }
            currentExitLevel = levelName;
            saveData("battle");
            SceneManager.LoadScene(levelName);
        }
        else 
        {
            print("û��ս������");
        }
    
    }







    //�ļ���д

    public void saveData()
    {
        if (data.friendship.Count == 0)
        {
            data.friendship.Add(0);
            data.friendship.Add(0);
            data.friendship.Add(0);
            data.friendship.Add(0);
        }
        data.friendship[0] = friendship[0];
        data.friendship[1] = friendship[1];
        data.friendship[2] = friendship[2];
        data.friendship[3] = friendship[3];
        data.currentDay = currentDay;
        data.currentActivity = currentActivityPoints;
        data.currentBattle = currentBattlePoints;
        data.levelList = levelList;
        data.equipment = equippedTower;
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, jsonData);
    }

    public void saveData(string name)
    {
        if (data.friendship.Count == 0)
        {
            data.friendship.Add(0);
            data.friendship.Add(0);
            data.friendship.Add(0);
            data.friendship.Add(0);
        }
       
        data.friendship[0] = friendship[0];
        data.friendship[1] = friendship[1];
        data.friendship[2] = friendship[2];
        data.friendship[3] = friendship[3];
        data.currentDay = currentDay;
        data.currentActivity = currentActivityPoints;
        data.currentBattle = currentBattlePoints;
        data.levelList = levelList;
        data.equipment = equippedTower;
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/"+name+".json", jsonData);
    }

    public void LoadGameData()
    {

            string jsonData = File.ReadAllText(savePath);
            data= JsonUtility.FromJson<GameData>(jsonData);
            transitionImage.SetActive(true);
            sceneEnding = true;
            isLoading = true;
            //HideAvailableCharacters();
            //�ȿ���
    
    }
    public void LoadGameData(string name)
    {


            string jsonData = File.ReadAllText(Application.persistentDataPath + "/" + name + ".json");
            data = JsonUtility.FromJson<GameData>(jsonData);
            transitionImage.SetActive(true);
            sceneEnding = true;
            isLoading = true;
            //HideAvailablefriendship();
            //�ȿ���

    }

    #region  ������������
    void EndScene()
    {
        FadeToBlack();
        if (transition.color.a > 0.999)
        {
            sceneEnding = false;
            if (isLoading)
            {
                currentDay = data.currentDay;
                currentActivityPoints = data.currentActivity;
                currentBattlePoints = data.currentBattle;
                levelList = data.levelList;
                friendship = data.friendship;
                equippedTower = data.equipment;
                SceneManager.LoadScene("basement");
                Invoke("LoadToStart", 0.5f);
            }
            Invoke("waitScene", 1f);
            
        }
    }

    void waitScene()
    {
        sceneStarting = true;
    }

    void StartScene()
    {
        FadeToClear();
        if (transition.color.a <= 0.01)
        {
            transition.color = Color.clear;
            sceneStarting = false;
            transitionImage.SetActive(false);
        }
    }

    private void FadeToBlack()
    {
        transition.color = Color.Lerp(transition.color, Color.black, fadeSpeed * Time.deltaTime);
    }
    private void FadeToClear()
    {
        transition.color = Color.Lerp(transition.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    #endregion  ������������

    #region �ؿ�չʾ�����ء�����

    void addLevel()
    {

        BaseCamera camera = GameObject.Find("Main Camera").GetComponent<BaseCamera>();
        GameObject levelHolder = camera.levelHolder;
        List<string> unlockedLevels = dailyUnlockDataList[currentDay - 1].unlockedLevels;//ȡ�õ����б�

        for (int i=0;i<unlockedLevels.Count;i++ )
        {
            if(!levelList.Contains(unlockedLevels[i]))
            levelList.Add(unlockedLevels[i]);
        }

        foreach (Transform child in levelHolder.transform)
        {
            Destroy(child.gameObject);
        }


        foreach (string levelName in levelList)
        {
            GameObject levelPrefab = Resources.Load<GameObject>("Level/"+levelName);

            if (levelPrefab != null)
            {
                GameObject levelInstance = Instantiate(levelPrefab, levelHolder.transform);
                levelInstance.SetActive(false);
            }
            else
            {
                Debug.LogError("Ԥ�������ʧ��:" +levelName);
            }
        }

    }

    void loadLevel()
    {

        BaseCamera camera = GameObject.Find("Main Camera").GetComponent<BaseCamera>();
        GameObject levelHolder = camera.levelHolder;

        if (isBattling)
        {
            
            for (int i = 0; i < levelList.Count; i++)
            {
                if (levelList[i] == currentExitLevel)
                {
                    levelList.Remove(levelList[i]);
                }
            }

        }

        foreach (Transform child in levelHolder.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < levelList.Count; i++)
        {
            GameObject levelPrefab = Resources.Load<GameObject>("Level/" + levelList[i]);

            if (levelPrefab != null)
            {
                GameObject levelInstance = Instantiate(levelPrefab, levelHolder.transform);
                levelInstance.SetActive(false);
            }
            else
            {
                Debug.LogError("Ԥ�������ʧ��:" + levelList[i]);
            }
        }

    }

    void sortLevel()
    {
        BaseCamera camera = GameObject.Find("Main Camera").GetComponent<BaseCamera>();
        GameObject levelHolder = camera.levelHolder;
        List<Transform> levelTransforms = new List<Transform>();

        foreach (Transform child in levelHolder.transform)
        {
            levelTransforms.Add(child);
        }
        
        // �����Ƿ������߹ؿ��Լ���levelHolder�е������������
        levelTransforms.Sort((a, b) =>
        {
            LevelInfo levelInfoA = a.GetComponent<LevelInfo>();
            LevelInfo levelInfoB = b.GetComponent<LevelInfo>();

            // �����߹ؿ�����ǰ��
            if (levelInfoA != null && levelInfoA.isMain)
            {
                return -1;
            }
            else if (levelInfoB != null && levelInfoB.isMain)
            {
                return 1;
            }
            else
            {
                // ������levelHolder�е������������
                return a.GetSiblingIndex().CompareTo(b.GetSiblingIndex());
            }
        });
        
        // ���ؿ���������
        ArrangeLevels(levelTransforms);

    }

    private void ArrangeLevels(List<Transform> levelTransforms)
    {
        int rowSize = 5;

        for (int i = 0; i < levelTransforms.Count; i++)
        {
            Transform levelTransform = levelTransforms[i];
            int row = i / rowSize;
            int col = i % rowSize;

            Vector3 localScale = levelTransform.localScale;
            Vector3 newPosition = new Vector3(col * levelOffsetX * localScale.x, row * levelOffsetY* localScale.y, 0f);
            levelTransform.localPosition = newPosition;
        }
    }

    #endregion


    private void ChangeSkybox()
    {
        // ����Ƿ����㹻����պв��ʿɹ�����
        if (skyboxMaterials.Length > 0)
        {
            // ѭ��ʹ����պв���
            int qcurrentDay = currentDay % skyboxMaterials.Length;

            // ���õ�ǰ��պ�
            RenderSettings.skybox = skyboxMaterials[qcurrentDay];
        }
        else
        {
            Debug.LogError("û��������պв��ʣ�");
        }
    }
}
