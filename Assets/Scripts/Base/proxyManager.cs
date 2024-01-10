using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proxyManager : MonoBehaviour
{
    Manager manager;
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    public void consume(int index)
    {
        manager.ConsumeActivityPoints(index);
    }
    public void battle()
    {
        manager.HandleBattlePointsAndLevels();
    }
    public void save()
    {
        manager.saveData();
    }
    public void load()
    {
        manager.LoadGameData();
    }
}
