using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Update()
    {
        // 监听按钮点击

        if (Input.GetKeyDown(KeyCode.R))
        {

            ReloadScene();
        }

    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 重新加载当前场景
        SceneManager.LoadScene(currentSceneName);
    }
    public void loadScene(string name)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }

    public void BackToBasement()
    {
        Time.timeScale = 1;
        /*
        Manager manager = GameObject.Find("Manager").GetComponent<Manager>();
        manager.isBattling = true;
        manager.LoadGameData("battle");
        */
        MapManager mapManager= GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.backToMap();

    }

    public void retreat()
    {
        Time.timeScale = 1;
        /*
        Manager manager = GameObject.Find("Manager").GetComponent<Manager>();
        manager.LoadGameData("battle");
        */
        MapManager mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.isBattle = false;
        mapManager.backToMap();
    }

}
