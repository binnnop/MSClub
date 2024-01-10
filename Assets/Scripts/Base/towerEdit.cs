using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class towerEdit : MonoBehaviour
{
    Manager manager;
    public GameObject[] aim;
    public string[] towerList;
    public GameObject tip;
    public float fadeSpeed = 1.5f;
    public bool playRed = false;
    public bool playBlack = false;

    void OnEnable()
    {   
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        Initial();
        print("OnEable");
    }

    private void Update()
    {
        if (playRed)
            ToRed();
        if (playBlack)
            ToBlack();

    }

    public void OnClickCard(string cardName)
    {

        if (manager.equippedTower.Contains(cardName))
        {
            getAimByName(cardName).SetActive(false);
            manager.equippedTower.Remove(cardName);
           
            if (manager.equippedTower.Count <4)
            {
                tip.SetActive(true);
                playBlack = true;
                playRed = false;
            }
        }

        else if (manager.equippedTower.Count < 4) {
            getAimByName(cardName).SetActive(true);
            manager.equippedTower.Add(cardName);
            if (manager.equippedTower.Count == 4)
            {
                tip.SetActive(false);
                playRed = true;
                playBlack = false;
            }
        }
    }

    List<Image>  getAllImage()
    {
        List<Image> imageComponents = new List<Image>();

        for (int i = 0; i < aim.Length; i++)
        {
            Image x = aim[i].GetComponent<Image>();
            if (!imageComponents.Contains(x))
                imageComponents.Add(x);
        }

        return imageComponents;
    }

    GameObject getAimByName(string name)
    { 
        switch (name)
        {
            case "Fortress":
                return aim[0];        
            case "Cannon":
                return aim[1];
            case "MageTower":
                return aim[2];             
            case "Supporter":
                return aim[3];                
            case "Mini":
                return aim[5];
            case "Sentry":
                return aim[7];
            case "Gold":
                return aim[8];
            case "Blaze":
                return aim[10];   
        }
        return null;
    }


    private void ToRed()
    {
        List<Image> imageComponents = getAllImage();
        foreach (Image transition in imageComponents)
        {
            if (transition != null)
            {
                transition.color = Color.Lerp(transition.color, Color.red, fadeSpeed * Time.deltaTime);
                if (transition.color.r ==1)
                {
                    playRed = false;
                }
            }
        }
        
    }
    private void ToBlack()
    {
        List<Image> imageComponents = getAllImage();
        foreach (Image transition in imageComponents)
        {
            if (transition != null)
            {
                transition.color = Color.Lerp(transition.color, Color.black, fadeSpeed * Time.deltaTime);
                if (transition.color.r ==0)
                {
                    playBlack = false;               
                }
            }
        }
    }

    private void Initial()
    {
        List<string> unlockedTowerList = manager.unlockedTowerList;
        for (int i = 0; i < unlockedTowerList.Count; i++)
        {
            getAimByName(unlockedTowerList[i]).transform.parent.parent.gameObject.SetActive(true);
            print(getAimByName(unlockedTowerList[i]).transform.parent.parent.gameObject);
        }

        for (int i=0;i<towerList.Length;i++)
        {         
            getAimByName(towerList[i]).SetActive(manager.equippedTower.Contains(towerList[i])); 
        }
    }

}
