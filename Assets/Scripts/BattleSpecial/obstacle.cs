using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NavMeshObstacle navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        navMeshObstacle.carving = true;
        navMeshObstacle.carveOnlyStationary = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
