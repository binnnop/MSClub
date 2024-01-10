using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preBuild : MonoBehaviour
{
    public Base notToGenerate;
    public GameObject mini;

    void Start()
    {
        GameObject[] baseObjects = GameObject.FindGameObjectsWithTag("base");

        
        foreach (GameObject baseObject in baseObjects)
        {
            Base home = baseObject.transform.GetComponent<Base>();
            
            if (home != notToGenerate)
            {
                home.GenerateBuilding(mini);
                home.GenerateBuilding(mini);
            }
  
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
