using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class MapManager : MonoBehaviour
{
    
    // �洢�ؿ����ݵ���
    [System.Serializable]
    public class LevelData
    {
        public int levelNumber; // �ؿ����
        public bool unlocked;   // �Ƿ����
        public bool defeated;   // �Ƿ񱻴��
    }

    // �洢�������ݵ���
    [System.Serializable]
    public class CardData
    {
        public string cardName; // ��������
        public bool unlocked;   // �Ƿ����
        public bool inUse;      // �Ƿ�ʹ��
    }

    // �洢��ʼ���ݵ���
    [System.Serializable]
    public class InitialData
    {
        public List<LevelData> initialLevelDataList; // ��ʼ�ؿ������б�
    }

    // ��ʼ����
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

    // ����ؿ������б�
    public void SaveLevelDataList()
    {
        string filePath = Application.persistentDataPath + "/LevelDataList.json";
        string levelDataJson = JsonUtility.ToJson(new LevelDataListWrapper { levelDataList = initialData.initialLevelDataList });
        File.WriteAllText(filePath, levelDataJson);
    }

    // ���ļ����عؿ������б�
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

    // �������������б�
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


    // �� Start �����м���Ƿ��б�������ݣ����û����ʹ�ó�ʼ����
    void Start()
    {
        List<LevelData> loadedLevelDataList = LoadLevelDataList();
        SceneManager.LoadScene("BattleMap");

        if (loadedLevelDataList.Count == 0)
        {
            // ���û�б������ݣ���ʹ�ó�ʼ����
            Debug.Log("No saved data found. Using initial data.");
            SaveLevelDataList();
        }
    }

    public int GetFrontLevel()
    {
       
            int maxUnlockedLevelNumber = -1;

        // �����ؿ������б�
        foreach (LevelData levelData in initialData.initialLevelDataList)
        {
            // ����ؿ��ѽ���
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

    // �����࣬���ڰ�װ�б��Ա����л�
    [System.Serializable]
    private class LevelDataListWrapper
    {
        public List<LevelData> levelDataList;
    }

    // �����࣬���ڰ�װ�б��Ա����л�
    [System.Serializable]
    private class DeckDataListWrapper
    {
        public List<CardData> deckDataList;
    }
}