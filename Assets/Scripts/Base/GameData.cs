using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[System.Serializable]
public class CharacterData
{
    public int bondLevel;
}


[System.Serializable]
public class LevelData
{
    public string levelName;
    public int remainingDays;
}
*/

[System.Serializable]
public class GameData
{
    public int currentDay;
    public List<int> friendship;
    public int currentActivity;
    public int currentBattle;
    public List<string> levelList;
    public List<string> equipment;
    //public List<LevelData> levels;
    
}
