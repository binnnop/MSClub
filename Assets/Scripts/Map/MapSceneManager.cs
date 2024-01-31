using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class MapSceneManager : MonoBehaviour
{
    public MapManager mapManager;
    public Transform beacon;

    public LayerMask mapObjectLayer; // �����MapLevel�������ڵĲ���ӵ�����
    public bool isPopupOpen = false; // ����״̬��־
    public bool isHovered = false;
    private GameObject currentHoveredObject = null; // ��ǰ��ͣ������

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
        // ��ȡ AudioSource �����ȷ���ڽű�����һ�� AudioSource ���
        AudioSource audioSource = GetComponent<AudioSource>();

        // �����ؿ�������������
        foreach (Transform childTransform in IEtransform)
        {
            // ����������
            childTransform.gameObject.SetActive(true);

            // ����Ŀ��λ��
            Vector3 startPosition = childTransform.position;
            Vector3 targetPosition = startPosition + new Vector3(0.0f, 1.0f, 0.0f); // ����ʵ���������

            // ������Ч
            /*
            if (audioSource != null)
            {
                audioSource.Play(); // ������Ч
            }
            */
            // ���������������ƶ�
            float elapsedTime = 0f;
            while (elapsedTime < showPathSpeed)
            {
                childTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / showPathSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ȷ��Ŀ��λ�ñ���ȷ����
            childTransform.position = targetPosition;      
            yield return new WaitForSeconds(0.1f); // �������ʱ��
        }

        // ���� beacon Ŀ��λ��
        Transform maxUnlockedLevelTransform = transform.GetChild(IEindex);
        Transform lastChild = maxUnlockedLevelTransform.GetChild(maxUnlockedLevelTransform.childCount - 1);
        Vector3 beaconStartPosition = beacon.position;
        Vector3 beaconTargetPosition = lastChild.position;

        // ���� beacon �������ƶ�
        float beaconElapsedTime = 0f;
        while (beaconElapsedTime < showPathSpeed)
        {
            beacon.position = Vector3.Lerp(beaconStartPosition, beaconTargetPosition, beaconElapsedTime / showPathSpeed);
            beaconElapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ�� beacon Ŀ��λ�ñ���ȷ����
        beacon.position = beaconTargetPosition;
        mapManager.isBattle = false;
        mapManager.initialData.initialLevelDataList[IEindex].unlocked = true;
    }



    private void Initialize(List<MapManager.LevelData> levelDataList)
    {
        int maxUnlockedLevelNumber = -1;

        // �����ؿ������б�
        foreach (MapManager.LevelData levelData in levelDataList)
        {
            // ����ؿ��ѽ���
            if (levelData.unlocked)
            {
                
                if (levelData.levelNumber > maxUnlockedLevelNumber)
                {
                    maxUnlockedLevelNumber = levelData.levelNumber;
                }

                // �Թؿ�����һЩ����...
            }
            else
            {
                // ��ȡ�ؿ������
                int levelNumber = levelData.levelNumber;

                // ��ȡ�ؿ���������
                Transform levelTransform = transform.GetChild(levelNumber);

                // �����ؿ�������������
                foreach (Transform childTransform in levelTransform)
                {
                    // �½�һ�ξ���
                    Vector3 newPosition = childTransform.position;
                    newPosition.y -= 1.0f; // �½��ľ��룬����ʵ���������
                    childTransform.position = newPosition;

                    // ����������
                    childTransform.gameObject.SetActive(false);
                }
            }


        }

        // �ƶ��ű굽����ѽ����ؿ���λ��
        if (maxUnlockedLevelNumber != -1)
        {
            Transform maxUnlockedLevelTransform = transform.GetChild(maxUnlockedLevelNumber);
            Transform x = maxUnlockedLevelTransform.GetChild(maxUnlockedLevelTransform.childCount - 1);

            beacon.position = x.position;
        }
    }


    void Update()
    {
        // �������״̬�£���ִ�м��
        if (isPopupOpen)
            return;

        // �������Ƿ���ͣ��MapLevel������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, mapObjectLayer);

        // ��������ײ�����л�ȡӵ��MapLevel����ĵ�һ������
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

    // ����Outline
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

    // �ر�Outline
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

    // �򿪵���
    void OpenPopup(MapLevel mapLevel)
    {
        // ������Ӵ򿪵������߼�
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
            // ��������Ƿ���� MapInside ���������û�б�����
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
