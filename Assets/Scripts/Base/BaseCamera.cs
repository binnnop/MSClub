using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCamera : MonoBehaviour
{
    public Animator cameraAnimator; 
    public string zoomInAnimation ; 
    //public string zoomOutAnimation ; 

    public bool isZooming = false; // 是否正在进行拉近/拉远的动画
    public bool isZoomPaused = false; // 是否已经暂停了拉近/拉远的动画
    public Transform  heroCanva;
    public Transform hero;
    //private Transform xText;

    //处理填充动画
    public float fillSpeed = 0.5f;
    public float minFillAmount = 0.1f;
    private bool isDisappearing = false;
    private List<Image> fillImages;

    public GameObject levelHolder;
    public GameObject nowHero;

    void Start()
    {
        cameraAnimator = GetComponent<Animator>();
        fillImages = new List<Image>();
    }

    void Update()
    {
        CheckMouseClick();
        UpdateAnimationState();
        if (fillImages.Count != 0)
        {
            for (int i = 0; i < fillImages.Count; i++)
            {
                UpdateFillAmount(fillImages[i]);
            }
        }
       // UpdateFillAmount();
    }

    void CheckMouseClick()
    {

        if (Input.GetMouseButtonDown(0) && !isZooming)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("baseHero"))
            {
                //进入
                //xText = null;
                hero = hit.transform;
                nowHero = hit.transform.gameObject;
                baseHeroPivot baseHeroPivot = hit.collider.GetComponentInChildren<baseHeroPivot>();
                heroCanva = hit.collider.transform.Find("canvas");
                
                fillImages.Add(heroCanva.Find("back").GetComponent<Image>());
                fillImages.Add(heroCanva.Find("description").GetComponent<Image>());
                Transform x = heroCanva.Find("Activity");
                if (x != null)
                {
                    fillImages.Add(x.GetComponent<Image>()); 
                }

                zoomInAnimation = baseHeroPivot.animationName;

                if (baseHeroPivot != null)
                {
                    if (!isZoomPaused)
                        StartZoomInAnimation();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && isZoomPaused)
        {
            //出来
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Manager manager = GameObject.Find("Manager").GetComponent<Manager>();
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject==nowHero && manager.equippedTower.Count==4)
            {
                nowHero = null;
                reSetCamera();
            }

            /*
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("baseHero"))
            {
                reSetCamera();
            }
            */

        }

    }

    public void reSetCamera()
    {
        ResumeZoomAnimation();
        startAllTyping();
    }




    void UpdateAnimationState()
    {
       
    }

    void StartZoomInAnimation()
    {
        cameraAnimator.Play(zoomInAnimation);

     
        pauseAllTyping();
        isZooming = true;
        isZoomPaused = false;
        isDisappearing = true;
    }



    public void PauseZoomAnimation()
    {
        cameraAnimator.speed = 0f;
        isZoomPaused = true;
        showHeroUI();
    }

    void ResumeZoomAnimation()
    {
        cameraAnimator.speed = 1f;
        isZoomPaused = false;
        isZooming = false;
        isDisappearing = false;

        hideHeroUI();

    }

    void UpdateFillAmount(Image currentFillImage)
    {
       
        if (isDisappearing)
        {
            currentFillImage.fillAmount -= fillSpeed * Time.deltaTime;
        }
        else
        {
            currentFillImage.fillAmount += fillSpeed * Time.deltaTime;
            if (currentFillImage.fillAmount ==1)
            {
                fillImages.Clear();
                print(fillImages.Count);
            }
            
        }
    }

    public void showHeroUI()
    {
        Transform heroUI = hero.Find("RealCanvas");
        if (heroUI != null)
        {
            heroUI.gameObject.SetActive(true);
        }
    }
    public void hideHeroUI()
    {
        if (hero != null) {
            Transform heroUI = hero.Find("RealCanvas");
            if (heroUI != null)
            {
                heroUI.gameObject.SetActive(false);
            }
        }
        
    }

    void pauseAllTyping()
    {
        TypeWrite[] typeWriters = FindObjectsOfType<TypeWrite>();

        foreach (TypeWrite typeWriter in typeWriters)
        {
            if (typeWriter != null)
            {
                typeWriter.PauseTyping();
            }
        }

    }
    void startAllTyping()
    {
        TypeWrite[] typeWriters = FindObjectsOfType<TypeWrite>();

        foreach (TypeWrite typeWriter in typeWriters)
        {
            if (typeWriter != null)
            {
                typeWriter.Initialize();
            }
        }

    }

}
