using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class MapManager : MonoBehaviour
{
    
    // 存储关卡数据的类
    [System.Serializable]
    public class LevelData
    {
        public int levelNumber; // 关卡序号
        public bool unlocked;   // 是否解锁
        public bool defeated;   // 是否被打败
    }

    // 存储卡牌数据的类
    [System.Serializable]
    public class CardData
    {
        public string cardName; // 卡牌名称
        public bool unlocked;   // 是否解锁
        public bool inUse;      // 是否使用
    }

    // 存储初始数据的类
    [System.Serializable]
    public class InitialData
    {
        public List<LevelData> initialLevelDataList; // 初始关卡数据列表
    }

    // 初始数据
    public InitialData initialData;
    private static MapManager instance;
    public List<string> deck;
    public GameObject deckList;

    public bool isBattle = false;
    public int nowBattle;

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

    // 保存关卡数据列表
    public void SaveLevelDataList()
    {
        string filePath = Application.persistentDataPath + "/LevelDataList.json";
        string levelDataJson = JsonUtility.ToJson(new LevelDataListWrapper { levelDataList = initialData.initialLevelDataList });
        File.WriteAllText(filePath, levelDataJson);
    }

    // 从文件加载关卡数据列表
    public List<LevelData> LoadLevelDataList()
    {
        string filePath = Application.persistentDataPath + "/LevelDataList.json";

        if (File.Exists(filePath))
        {
            string levelDataJson = File.ReadAllText(filePath);
            LevelDataListWrapper wrapper = JsonUtility.FromJson<LevelDataListWrapper>(levelDataJson);
            initialData.initialLevelDataList = wrapper.levelDataList;
            return initialData.initialLevelDataList;
        }
        else
        {
            return initialData.initialLevelDataList;
        }
    }

    // 保存套牌数据列表
    public void SaveDeckDataList(List<CardData> deckDataList)
    {
        string key = "DeckDataList";
        string deckDataJson = JsonUtility.ToJson(new DeckDataListWrapper { deckDataList = deckDataList });
        PlayerPrefs.SetString(key, deckDataJson);
    }



    public void backToMap()
    {
        initialData.initialLevelDataList[nowBattle].defeated = true;
        SceneManager.LoadScene("BattleMap");
    
    }


    // 在 Start 方法中检查是否有保存的数据，如果没有则使用初始数据
    void Start()
    {
        List<LevelData> loadedLevelDataList = LoadLevelDataList();
        SceneManager.LoadScene("BattleMap");

        if (loadedLevelDataList.Count == 0)
        {
            // 如果没有保存数据，则使用初始数据
            Debug.Log("No saved data found. Using initial data.");
            SaveLevelDataList();
        }
    }

    public int GetFrontLevel()
    {
       
            int maxUnlockedLevelNumber = -1;

        // 遍历关卡数据列表
        foreach (LevelData levelData in initialData.initialLevelDataList)
        {
            // 如果关卡已解锁
            if (levelData.unlocked)
            {

                if (levelData.levelNumber > maxUnlockedLevelNumber)
                {
                    maxUnlockedLevelNumber = levelData.levelNumber;
                }


            }

        }

        return maxUnlockedLevelNumber;   

    }

    // 辅助类，用于包装列表以便序列化
    [System.Serializable]
    private class LevelDataListWrapper
    {
        public List<LevelData> levelDataList;
    }

    // 辅助类，用于包装列表以便序列化
    [System.Serializable]
    private class DeckDataListWrapper
    {
        public List<CardData> deckDataList;
    }
}