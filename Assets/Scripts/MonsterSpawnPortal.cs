using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class MonsterSpawnPortal : MonoBehaviour
{
    public Vector3[] spawnPoints;
    public GameObject[] routes;
    public GameObject[] entrance;
    public float totalTime = 0;
    public TimelineController timeLine;

    [System.Serializable]
    public class MonsterSpawnData
    {
        public string monsterPrefab;
        public int spawnPointIndex;
        public float spawnInterval;
        public int routeIndex;
        public bool generatePreview; // �����ĳ�Ա����
        //������ݣ����� ������ ʱ���� ��·      ���������Ϣ
    }

    public List<List<MonsterSpawnData>> allLevelsSpawnData = new List<List<MonsterSpawnData>>();

    // List of CSV files to be loaded
    public string[] levelDataFileNames = { "level1.csv", "level2.csv", "level3.csv" };

    public List<Vector3> pathPoints = new List<Vector3>(); // ·���ϵĵ��б�
    private LineRenderer lineRenderer;
    public float speed = 5f; // �����ƶ��ٶ�
    public int pathIndex = 0; 

    void Start()
    {
        timeLine = GameObject.Find("Engine").GetComponent<TimelineController>();
        LoadAllLevelData();
        timeLine.StartTimeLine(GetMaxTotalTime());
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pathPoints.Count;

        // Start a coroutine for each level
        for (int i = 0; i < allLevelsSpawnData.Count; i++)
        {
            StartCoroutine(SpawnMonsterRoutine(allLevelsSpawnData[i]));
        }
    }

    void LoadAllLevelData()
    {
        allLevelsSpawnData.Clear();

        foreach (string fileName in levelDataFileNames)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

            if (File.Exists(filePath))
            {
                // ��ȡCSV�ļ�
                string[] lines = File.ReadAllLines(filePath);

                // ����ÿ��CSV�ļ�������
                List<MonsterSpawnData> levelSpawnData = new List<MonsterSpawnData>();
                ProcessCSVData(lines, levelSpawnData);

                // ��ÿ���ؿ�������������ӵ��б���
                allLevelsSpawnData.Add(levelSpawnData);
            }
            else
            {
                Debug.LogError("Level data file not found: " + filePath);
            }
        }
    }

    void ProcessCSVData(string[] lines, List<MonsterSpawnData> levelSpawnData)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            // �ָ�CSV��
            string[] values = lines[i].Split(',');

            // ���������������ݶ���
            MonsterSpawnData spawnData = new MonsterSpawnData
            {
                monsterPrefab = values[0],
                spawnPointIndex = int.Parse(values[1]),
                spawnInterval = float.Parse(values[2]),
                routeIndex = int.Parse(values[3]),
                generatePreview = bool.Parse(values[4]) 
            };

            // ��������ӵ��б���
            levelSpawnData.Add(spawnData);
            totalTime += float.Parse(values[2]);
        }
    }

    IEnumerator SpawnMonsterRoutine(List<MonsterSpawnData> spawnDataList)
    {
        

        foreach (var spawnData in spawnDataList)
        {
            if (spawnData.generatePreview)
            {
                // ���ɵ�·Ԥ���Ĵ���
                Vector3 previewSpawnPoint = GetSpawnPointByIndex(spawnData.spawnPointIndex);
                GameObject previewRoute = GetRouteByIndex(spawnData.routeIndex);
                // �ڴ����ɵ�·Ԥ��
                yield return new WaitForSeconds(3f);

                // �����·Ԥ��
            }

            yield return new WaitForSeconds(spawnData.spawnInterval);

            Vector3 spawnPoint = GetSpawnPointByIndex(spawnData.spawnPointIndex);
            GameObject route = GetRouteByIndex(spawnData.routeIndex);
            GameObject monster = Instantiate(Resources.Load(spawnData.monsterPrefab) as GameObject, spawnPoint, Quaternion.identity);

            EnemyAI monsterAI;
            if (spawnData.monsterPrefab == "��������")
            {
                monsterAI = monster.transform.GetChild(0).GetComponent<EnemyAI>();
            }
            else
                monsterAI = monster.GetComponent<EnemyAI>();

            monsterAI.route = route;
            monsterAI.InitializeMonster();
        }
    }

    Vector3 GetSpawnPointByIndex(int spawnPointIndex)
    {
        if (spawnPointIndex >= 0 && spawnPointIndex < spawnPoints.Length)
        {
            // return spawnPoints[spawnPointIndex];
            return entrance[spawnPointIndex].transform.position;
        }
        else
        {
            Debug.LogError("Invalid spawn point index: " + spawnPointIndex);
            return Vector3.zero; // ���߷���һ��Ĭ�ϵ�λ��
        }
    }

    GameObject GetRouteByIndex(int routeIndex)
    {
            return routes[routeIndex];
    }

    public float GetMaxTotalTime()
    {
        float maxTotalTime = 0;

        foreach (var levelSpawnData in allLevelsSpawnData)
        {
            float levelTotalTime = 0;

            foreach (var spawnData in levelSpawnData)
            {
                levelTotalTime += spawnData.spawnInterval;
            }

            if (levelTotalTime > maxTotalTime)
            {
                maxTotalTime = levelTotalTime;
            }
        }

        return maxTotalTime;
    }
}