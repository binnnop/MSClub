using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeroPanel : MonoBehaviour
{
    public GameObject activityButton;
    public baseHeroPivot hero;
    public GameObject levelHolder;
    public GameObject towerHolder;
    public TextMeshProUGUI friendshipLevel;


    private void OnEnable()
    {
         activityButton.SetActive(hero.canActivity);
        if (levelHolder != null)
        {
            levelHolder.SetActive(true);
            foreach (Transform child in levelHolder.transform)
            {
                child.gameObject.SetActive(true);
            }
            
        }
        if (towerHolder != null)
        {
            levelHolder.SetActive(true);
            foreach (Transform child in towerHolder.transform)
            {
                child.gameObject.SetActive(true);
            }

        }
        updateText();
        
    }


    private void OnDisable()
    {


        if (levelHolder != null)
        {
            levelHolder.SetActive(false);
            foreach (Transform child in levelHolder.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        if (towerHolder != null)
        {
            levelHolder.SetActive(false);
            foreach (Transform child in towerHolder.transform)
            {
                child.gameObject.SetActive(false);
            }
        }



    }

    public void updateText()
    {
        Manager manager = GameObject.Find("Manager").GetComponent<Manager>();   
        switch (hero.heroName)
        {
            case "Fire":
                friendshipLevel.text = manager.friendship[0].ToString() + "/7";
                break;
            case "See":
                friendshipLevel.text = manager.friendship[1].ToString() + "/7";
                break;
            case "Recruit":
                friendshipLevel.text = manager.friendship[2].ToString() + "/7";
                break;
            case "Arch":
                friendshipLevel.text = manager.friendship[3].ToString() + "/7";
                break;
        }
    }
}
