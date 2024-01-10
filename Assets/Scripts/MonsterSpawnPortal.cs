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
        public bool generatePreview; // 新增的成员变量
        //表格数据：名称 出生点 时间间隔 道路      还差：波次信息
    }

    public List<List<MonsterSpawnData>> allLevelsSpawnData = new List<List<MonsterSpawnData>>();

    // List of CSV files to be loaded
    public string[] levelDataFileNames = { "level1.csv", "level2.csv", "level3.csv" };

    public List<Vector3> pathPoints = new List<Vector3>(); // 路径上的点列表
    private LineRenderer lineRenderer;
    public float speed = 5f; // 光线移动速度
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
                // 读取CSV文件
                string[] lines = File.ReadAllLines(filePath);

                // 处理每个CSV文件的数据
                List<MonsterSpawnData> levelSpawnData = new List<MonsterSpawnData>();
                ProcessCSVData(lines, levelSpawnData);

                // 将每个关卡的生成数据添加到列表中
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
            // 分割CSV行
            string[] values = lines[i].Split(',');

            // 创建怪物生成数据对象
            MonsterSpawnData spawnData = new MonsterSpawnData
            {
                monsterPrefab = values[0],
                spawnPointIndex = int.Parse(values[1]),
                spawnInterval = float.Parse(values[2]),
                routeIndex = int.Parse(values[3]),
                generatePreview = bool.Parse(values[4]) 
            };

            // 将对象添加到列表中
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
                // 生成道路预览的代码
                Vector3 previewSpawnPoint = GetSpawnPointByIndex(spawnData.spawnPointIndex);
                GameObject previewRoute = GetRouteByIndex(spawnData.routeIndex);
                // 在此生成道路预览
                yield return new WaitForSeconds(3f);

                // 清理道路预览
            }

            yield return new WaitForSeconds(spawnData.spawnInterval);

            Vector3 spawnPoint = GetSpawnPointByIndex(spawnData.spawnPointIndex);
            GameObject route = GetRouteByIndex(spawnData.routeIndex);
            GameObject monster = Instantiate(Resources.Load(spawnData.monsterPrefab) as GameObject, spawnPoint, Quaternion.identity);

            EnemyAI monsterAI;
            if (spawnData.monsterPrefab == "气球铁匠")
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
            return Vector3.zero; // 或者返回一个默认的位置
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