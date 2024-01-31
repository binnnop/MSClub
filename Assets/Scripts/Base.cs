using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static cardInfo;
using TMPro;

public class Base : MonoBehaviour
{
    public GameObject buildingPrefab;  // ������Ԥ����
    public GameObject rangePrefab;
    public Transform canvas;  // Base �µ� Canvas

    public int cardCount;  // ͳ�ƿ��ƶѵ��Ĳ���
    public int extraCount=0;
    public float buildingHeight = 1f;  // �����Ĺ̶��߶�

    public GameObject cardHolder;
    public GameObject teleport;
    //public GameObject[] towerPrefabs;
    public Vector3 buildingMergin;
    public Vector3 cannonMergin;
    public CardManager manager;
    public MapManager game;
    public test test;
    public float shovelPrice=0.8f;

    public GameObject limitTips;
    public GameObject limitCount;
    public Material voidMaterial;
    public GameObject voidObject;
    public GameObject voidObject2;

    public bool isVoidState=false;

    //Ӣ�۰ڷ�
    public string[] heroTargetTags = { "Tower", "Fortress" };

    private void Start()
    {
        
        //cardInfo.OnCardDropped += OnCardDropped;
        test = GameObject.Find("test").GetComponent<test>();
        if (!test.isTestMode)
            game = GameObject.Find("MapManager").GetComponent<MapManager>();  
        
        teleport = transform.GetChild(1).gameObject;
        manager = GameObject.Find("Engine").GetComponent<CardManager>();
        voidMaterial = manager.voidMaterial;
        shovelPrice = manager.shovelPrice;

    }


    //���ز���
    public int GetCardCount()
    {
        return cardCount;
    }

    public void OnCardDropped(cardInfo card)
    {
        if (manager.currentMoney >= card.price && cardCount<manager.maxLayer)
        {
            GenerateBuilding(card.major);
            card.rectTransform.anchoredPosition = card.initialPosition;
            manager.currentMoney -= card.price;
            manager.UpdateMoneyText();
           
        }
        else { 
        
        }
        

    }

    public void ShowEffect(int price)
    {
        //if(type==topCardSuit)
        limitCount.SetActive(true);
        if (price <= manager.currentMoney && cardCount < manager.maxLayer)
        {
          
            Image i = limitCount.GetComponent<Image>();
            i.color = new Color(0,0,0, 0.5f);
            TextMeshProUGUI countText = limitCount.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            countText.text = cardCount + "/" + manager.maxLayer;
            teleport.SetActive(true);
        }
        
        else if (cardCount == manager.maxLayer)
        {
            //limitTips.SetActive(true);
            Image i = limitCount.GetComponent<Image>();
            i.color = new Color(0.937f,0.05f,0.05f,0.5f);
            TextMeshProUGUI countText = limitCount.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            countText.text = cardCount + "/" + manager.maxLayer;
        }
        

    }

    public void HideEffect()
    {
        teleport.SetActive(false);
        limitTips.SetActive(false);
        limitCount.SetActive(false);
    }

    public void ShowHeroEffect()
    {
        if (cardCount>0)
            teleport.SetActive(true);
    }

    public void GenerateBuilding(string major)
    {
        // ���㽨���ĸ߶ȣ�ÿ�����
        float buildingHeightOffset = (cardCount+extraCount) * buildingHeight;
        buildingPrefab = GetTower(major);
        
        GameObject building;
        if (major == "Cannon")
        {
            building = Instantiate(buildingPrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin + cannonMergin, Quaternion.identity,transform);
        }
        else
        {
            building = Instantiate(buildingPrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin, Quaternion.identity,transform);
        }
        if (major == "Sentry")
        {
           extraCount++ ;
        }
        cardCount++;
        print("Count:" + cardCount);
        AdjustHeroLocation();
        closeRange();

        if (major == "Leaf")
        {
            CheckLastThreeChildren();

        }
        if (major == "Leaf2")
        {
            StartCoroutine(CheckLastThreeChildren2());
        }


    }

    public void GenerateBuilding(string major,int price)
    {
        // ���㽨���ĸ߶ȣ�ÿ�����
        float buildingHeightOffset = (cardCount + extraCount) * buildingHeight;
        buildingPrefab = GetTower(major);

        GameObject building;
        if (major == "Cannon")
        {
            building = Instantiate(buildingPrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin + cannonMergin, Quaternion.identity, transform);
        }
        else
        {
            building = Instantiate(buildingPrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin, Quaternion.identity, transform);
            priceAdd x = building.GetComponent<priceAdd>();
            x.price = price;
        }
        if (major == "Sentry")
        {
            extraCount++;
        }
        cardCount++;
        print("Count:" + cardCount);
        AdjustHeroLocation();
        closeRange();

        if (major == "Leaf")
        {
            CheckLastThreeChildren();

        }
        if (major == "Leaf2")
        {
            StartCoroutine(CheckLastThreeChildren2());
        }


    }


    public void GenerateVoid(string major)
    {
        if (!isVoidState)
        {
            Base[] bases = FindObjectsOfType<Base>();
            foreach (Base baseObject in bases)
            {
                if(baseObject!=this)
                baseObject.HideVoid();
            }

            float buildingHeightOffset = (cardCount + extraCount) * buildingHeight;

            EmptyAI sourceTower = GetTower(major).GetComponent<EmptyAI>();
            buildingPrefab = sourceTower.model.gameObject;       
            GameObject building;
            GameObject range;
  
            if (major == "Sentry")
            {
                building = Instantiate(buildingPrefab, transform.position + new Vector3(0f, buildingHeightOffset+buildingHeight*0.5f, 0f) + buildingMergin , Quaternion.identity, transform);
            }
            else
            {
                building = Instantiate(buildingPrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin, Quaternion.identity, transform);
            }
            voidObject = building;
            building.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            if (major == "Sentry")
            {
                MeshRenderer[] meshes= building.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mesh in meshes)
                {
                    mesh.material = voidMaterial;
                }
            }
            else
            building.GetComponent<MeshRenderer>().material = voidMaterial;


            if (sourceTower.greenArea != null)
            {
                rangePrefab = GetTower(major).GetComponent<EmptyAI>().greenArea.gameObject;
                range = Instantiate(rangePrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin, Quaternion.identity, transform);
                voidObject2 = range;
               range.transform.localScale *= 0.7f;
            }
                



            closeRange();
            isVoidState = true;
        }
      

    }

    public void HideVoid()
    {
        if (isVoidState&&voidObject!=null)
        {
            Destroy(voidObject);
            if(voidObject2!=null)
            Destroy(voidObject2);
        }
        isVoidState =false;


    }

    public void GenerateBuilding(GameObject major)
    {
        // ���㽨���ĸ߶ȣ�ÿ�����
        float buildingHeightOffset = (cardCount + extraCount) * buildingHeight;
        buildingPrefab = major;

        GameObject building;
        if (major.name == "Cannon")
        {
            building = Instantiate(buildingPrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin + cannonMergin, Quaternion.identity, transform);
        }
        else
        {
            building = Instantiate(buildingPrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin, Quaternion.identity, transform);
        }
        if (major.name == "Sentry")
        {
            extraCount++;
        }
        cardCount++;
        print("Count:" + cardCount);
        AdjustHeroLocation();


        
    }



    public void Decrease()
    {
        int childCount = transform.childCount;
    
        if (cardCount > 0)
        {
            Transform lastChild = transform.GetChild(childCount - 1);
            if (lastChild.gameObject.tag == "Hero" || lastChild.gameObject.tag == "Model")
            {
                lastChild = transform.GetChild(childCount - 2);
            }
            if (lastChild.name == "Sentry(Clone)")
            {
                extraCount--;
            }
            print(lastChild.gameObject + "    Destroyed");
            Destroy(lastChild.gameObject);

           

            cardCount--;
            
            AdjustHeroLocation();
            closeRange();

        }
        else
        {
            Debug.Log("No more child objects to decrease.");
        }
    }

    public void Decrease(int count)
    {
        int beginIndex = 1;
        for (int i = 0; i < count; i++)
        {
            int childCount = transform.childCount;

            if (cardCount > 0)
            {
                Transform lastChild = transform.GetChild(childCount - beginIndex);

               

                if (lastChild.gameObject.tag == "Hero")
                {
                    lastChild = transform.GetChild(childCount - (beginIndex+1));
                }
                if (lastChild.name == "Sentry(Clone)")
                {
                    extraCount--;
                }
                print(lastChild.gameObject + "    Destroyed");
                Destroy(lastChild.gameObject);


                if (lastChild.CompareTag("Model"))
                {
                   
                    beginIndex += 1;

                    Transform BChild = transform.GetChild(childCount - beginIndex);
                    if (BChild.gameObject.tag == "Hero")
                    {
                       BChild = transform.GetChild(childCount - (beginIndex + 1));
                    }
                    if (BChild.name == "Sentry(Clone)")
                    {
                        extraCount--;
                    }
                    print(BChild.gameObject + "BB    Destroyed");
                    Destroy(BChild.gameObject);

                    if (BChild.CompareTag("Model"))
                    {
                        beginIndex += 1;

                        Transform CChild = transform.GetChild(childCount - beginIndex);
                        if (CChild.gameObject.tag == "Hero")
                        {
                            CChild = transform.GetChild(childCount - (beginIndex + 1));
                        }
                        if (CChild.name == "Sentry(Clone)")
                        {
                            extraCount--;
                        }
                        print(CChild.gameObject + "CC    Destroyed");
                        Destroy(CChild.gameObject);


                    }

                }


                beginIndex += 1;
                cardCount--;

                AdjustHeroLocation();
                closeRange();

            }
            else
            {
                Debug.Log("No more child objects to decrease.");
            }
        }
        
    }

    public void Shoveled()
    {
        int childCount = transform.childCount;
        int index = 1;
        manager.currentMoney += caculatingValue();
        manager.UpdateMoneyText();

        while (cardCount>0)
        {
           Transform lastChild = transform.GetChild(childCount - index);
            if (lastChild.tag == "Hero")
            {
                HeroController hero = GameObject.FindGameObjectWithTag("heroController").GetComponent<HeroController>();
                hero.born = false;         
            }
            else
                cardCount--;

            index++;
            Destroy(lastChild.gameObject);
            print("destroy work has already done");
          }

        extraCount = 0;
        closeRange();

    }


    public int caculatingValue()
    {
        float totalPrice=0;

        priceAdd[] childTransforms = GetComponentsInChildren<priceAdd>();

        foreach (priceAdd childTransform in childTransforms)
        {

            if (childTransform != null)
            {
                print(childTransform.price);
                totalPrice += childTransform.price;
            }
        }

        float f = totalPrice * shovelPrice;
        print(f);
        int truePrice = (int)f;
        
        return truePrice;
    }



    GameObject GetTower(string name)
    {/*
        if (game != null)
        {
            foreach (GameObject towerPrefab in game.towerPrefabs)
            {
                if (towerPrefab.name == name)
                {
                    return towerPrefab;
                }
            }
            return null;
        }
        else {
            */
            foreach (GameObject towerPrefab in test.towerPrefabs)
            {
                if (towerPrefab.name == name)
                {
                    return towerPrefab;
                }
            }
            return null;
     
    }

    bool CheckForTargetTag()
    {
        print("checking");
        // ��ȡ�������������������
        Transform[] childObjects = GetComponentsInChildren<Transform>(true);

        // ��������������
        foreach (Transform child in childObjects)
        {
            // ����������Ƿ����Ŀ���ǩ
            if (ArrayContainsTag(child.tag, heroTargetTags))
            {
                return true;
            }
        }

        // ���δ�ҵ�Ŀ���ǩ�������壬����false
        return false;
    }

    bool ArrayContainsTag(string tagToCheck, string[] tagArray)
    {
        foreach (string tag in tagArray)
        {
            if (tagToCheck == tag)
            {
                return true;
            }
        }
        return false;
    }

    public void SpawnHero(GameObject heroPrefab)
    {
        if (cardCount > 0) {
            float buildingHeightOffset = (cardCount + extraCount) * buildingHeight;
            Instantiate(heroPrefab, transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin, Quaternion.identity, transform);
            closeRange();
        }
       
    }
    public void MoveHero(string heroName)
    {
        if (cardCount > 0)
        {
            GameObject hero = GameObject.Find(heroName);
            float buildingHeightOffset = (cardCount + extraCount) * buildingHeight;
            hero.transform.position = transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin;
            hero.transform.SetParent(transform);
            Animator anim = hero.GetComponent<Animator>();
            anim.Play("Heroshow");
            closeRange();
        } 
        //print("Moved!heroPosition:"+heroPrefab.transform.position+"transPosition:"+ transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin);

    }


    public void AdjustHeroLocation()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Hero"))
            {
                float buildingHeightOffset = (cardCount+extraCount)* buildingHeight;
                child.position = transform.position + new Vector3(0f, buildingHeightOffset, 0f) + buildingMergin;
                closeRange();
                print("HeroAdjustComplete");
                return; // �ҵ�һ���Ϳ��Է����ˣ���Ϊ�������ֻ��һ��"Hero"
            }
        }
    }

    public void closeRange()
    {
        TowerRangeController range = manager.gameObject.GetComponent<TowerRangeController>();
        range.CloseAllGreenAreaVisibility();
        range.nowShining = null;
    
    }

    private void Update()
    {
        
    }

    private void mergeLeaf()
    { 

    }
    private void mergeLeaf2()
    {

    }



    private void CheckLastThreeChildren()
    {
        int childCount = transform.childCount;

        if (childCount >= 3)
        {
            bool isContinuousLeaf = true;
            int i = childCount - 1;
            int additionalCheckCount = 0;

            while (i >= childCount - 3)
            {
                GameObject childObject = transform.GetChild(i-additionalCheckCount).gameObject;
                print(i + "     " + childObject);

                if (childObject.CompareTag("Hero")||childObject.CompareTag("Model"))
                {           
                    print("����ץ���ˣ���Ҷ");
                    additionalCheckCount++;
                    continue;
                }

                LeafAI leafComponent = childObject.GetComponent<LeafAI>();

                if (leafComponent == null || leafComponent.level != 1)
                {
                    isContinuousLeaf = false;
                    print("leafComponent == null || leafComponent.level != level" + 1);
                    print(childObject.name);
                    break;
                }

                i--;
            }

            if (isContinuousLeaf)
            {
                Decrease(3);
                GenerateBuilding("Leaf2");
            }
        }
        
}

    IEnumerator CheckLastThreeChildren2()
    {
        yield return null;
        int childCount = transform.childCount;
       

        if (childCount >= 3)
        {
            bool isContinuousLeaf = true;
            int i = childCount - 1;
            int additionalCheckCount = 0;

            while (i >= childCount - 3)
            {
                GameObject childObject = transform.GetChild(i - additionalCheckCount).gameObject;
                print(i + "     " + childObject);

                if (childObject.CompareTag("Hero") || childObject.name.Contains("Model"))
                {
                    print("����ץ���ˣ���Ҷ");
                    additionalCheckCount++;
                    continue;
                }

                LeafAI leafComponent = childObject.GetComponent<LeafAI>();

                if (leafComponent == null || leafComponent.level != 2)
                {
                    isContinuousLeaf = false;
                    print("leafComponent == null || leafComponent.level != level" + 2);
                    break;
                }

                i--;
            }



            if (isContinuousLeaf)
            {
                Decrease(3);
               
                GenerateBuilding("Leaf3");
            }
        }

    }



}
