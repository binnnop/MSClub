using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class MapSceneManager : MonoBehaviour
{
    public MapManager mapManager;
    public Transform beacon;

    public LayerMask mapObjectLayer; // 将你的MapLevel物体所在的层添加到这里
    public bool isPopupOpen = false; // 弹窗状态标志
    public bool isHovered = false;
    private GameObject currentHoveredObject = null; // 当前悬停的物体

    public float showPathSpeed = 1f;

    private Transform IEtransform;
    private int IEindex;

    void Start()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        List<MapManager.LevelData> levelDataList = mapManager.initialData.initialLevelDataList;
        IEtransform = null;
        IEindex = 0;

        Initialize(levelDataList);

        if (mapManager.isBattle)
        {
            int unlockIndex = mapManager.nowBattle+1;

            if (!levelDataList[unlockIndex].unlocked)
            {
                IEindex = unlockIndex;
                IEtransform = transform.GetChild(unlockIndex);
                StartCoroutine("showNewPath");     
            }
        }
  

    }


    IEnumerator showNewPath()
    {
        // 获取 AudioSource 组件，确保在脚本中有一个 AudioSource 组件
        AudioSource audioSource = GetComponent<AudioSource>();

        // 遍历关卡的所有子物体
        foreach (Transform childTransform in IEtransform)
        {
            // 激活子物体
            childTransform.gameObject.SetActive(true);

            // 计算目标位置
            Vector3 startPosition = childTransform.position;
            Vector3 targetPosition = startPosition + new Vector3(0.0f, 1.0f, 0.0f); // 根据实际情况调整

            // 播放音效
            /*
            if (audioSource != null)
            {
                audioSource.Play(); // 播放音效
            }
            */
            // 进行匀速上升的移动
            float elapsedTime = 0f;
            while (elapsedTime < showPathSpeed)
            {
                childTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / showPathSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 确保目标位置被正确设置
            childTransform.position = targetPosition;      
            yield return new WaitForSeconds(0.1f); // 调整间隔时间
        }

        // 计算 beacon 目标位置
        Transform maxUnlockedLevelTransform = transform.GetChild(IEindex);
        Transform lastChild = maxUnlockedLevelTransform.GetChild(maxUnlockedLevelTransform.childCount - 1);
        Vector3 beaconStartPosition = beacon.position;
        Vector3 beaconTargetPosition = lastChild.position;

        // 进行 beacon 的匀速移动
        float beaconElapsedTime = 0f;
        while (beaconElapsedTime < showPathSpeed)
        {
            beacon.position = Vector3.Lerp(beaconStartPosition, beaconTargetPosition, beaconElapsedTime / showPathSpeed);
            beaconElapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保 beacon 目标位置被正确设置
        beacon.position = beaconTargetPosition;
        mapManager.isBattle = false;
        mapManager.initialData.initialLevelDataList[IEindex].unlocked = true;
    }



    private void Initialize(List<MapManager.LevelData> levelDataList)
    {
        int maxUnlockedLevelNumber = -1;

        // 遍历关卡数据列表
        foreach (MapManager.LevelData levelData in levelDataList)
        {
            // 如果关卡已解锁
            if (levelData.unlocked)
            {
                
                if (levelData.levelNumber > maxUnlockedLevelNumber)
                {
                    maxUnlockedLevelNumber = levelData.levelNumber;
                }

                // 对关卡进行一些操作...
            }
            else
            {
                // 获取关卡的序号
                int levelNumber = levelData.levelNumber;

                // 获取关卡的子物体
                Transform levelTransform = transform.GetChild(levelNumber);

                // 遍历关卡的所有子物体
                foreach (Transform childTransform in levelTransform)
                {
                    // 下降一段距离
                    Vector3 newPosition = childTransform.position;
                    newPosition.y -= 1.0f; // 下降的距离，根据实际情况调整
                    childTransform.position = newPosition;

                    // 隐藏子物体
                    childTransform.gameObject.SetActive(false);
                }
            }


        }

        // 移动信标到最大已解锁关卡的位置
        if (maxUnlockedLevelNumber != -1)
        {
            Transform maxUnlockedLevelTransform = transform.GetChild(maxUnlockedLevelNumber);
            Transform x = maxUnlockedLevelTransform.GetChild(maxUnlockedLevelTransform.childCount - 1);

            beacon.position = x.position;
        }
    }


    void Update()
    {
        // 如果弹窗状态下，不执行检查
        if (isPopupOpen)
            return;

        // 检查鼠标是否悬停在MapLevel物体上
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, mapObjectLayer);

        // 从所有碰撞物体中获取拥有MapLevel组件的第一个物体
        GameObject HoveredObject = GetFirstMapLevelObject(hits);

        if (HoveredObject == null)
        {
            DisableAllOutlines();
            isHovered = false;
        }
        else {
            EnableOutline(HoveredObject);
            isHovered = true;
        }

        
        if (Input.GetMouseButtonDown(0) && HoveredObject != null)
        {
            MapLevel mapLevel = HoveredObject.GetComponent<MapLevel>();
            if (mapLevel != null)
            {
                OpenPopup(mapLevel);
                DisableAllOutlines();
            }
        }
    }


    GameObject GetFirstMapLevelObject(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            MapLevel mapLevel = hitObject.GetComponent<MapLevel>();

            if (mapLevel != null)
            {
                return hitObject;
            }
        }

        return null;
    }

    // 开启Outline
    void EnableOutline(GameObject obj)
    {
        Outline outline;
        outline= obj.GetComponent<Outline>();
        print(obj);
        if (outline != null&&!outline.enabled)
        {
            outline.enabled = true;
        }
    }

    // 关闭Outline
    void DisableOutline()
    {
        if (currentHoveredObject != null)
        {
            Outline outline = currentHoveredObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    // 打开弹窗
    void OpenPopup(MapLevel mapLevel)
    {
        // 这里添加打开弹窗的逻辑
        mapLevel.OpenPopup();
        isPopupOpen = true;
    }

    void DisableAllOutlines()
    {
        MapLevel[] mapLevels = FindObjectsOfType<MapLevel>();
        foreach (var mapLevel in mapLevels)
        {
            mapLevel.DisableOutline();
        }
    }

    public void recover()
    {
        isPopupOpen = false;
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            // 检查物体是否带有 MapInside 组件，并且没有被隐藏
            MapInside mapInsideComponent = obj.GetComponent<MapInside>();
            if (mapInsideComponent != null && obj.activeSelf)
            {
                obj.GetComponent<MapInside>().hide();
            }
        }
    }

    public void saveData()
    {
        mapManager.SaveLevelDataList();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
