using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMultipleLink : MonoBehaviour
{
    public List<TriggerDown> triggers;
    public bool isFixed = false;
    public List<GameObject> needToHide;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFixed)
        {
            if (CheckList())
            {
                foreach (GameObject obj in needToHide)
                {
                    obj.SetActive(false);
                }
            }
        
        }
    }
    bool CheckList()
    {
        foreach (TriggerDown trig in triggers)
        {
            if (trig.index != 2)
            {
                return false;


            }
            else if (!trig.fillFix)
                return false;
        }
        return true;
    }
}
