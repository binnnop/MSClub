using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyAI enemy = GetComponent<EnemyAI>();
        GameObject burnParticle = Instantiate(Resources.Load("Fire") as GameObject, enemy.transform.position + new Vector3(0, 1, 0), enemy.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        burnParticle.transform.localScale = enemy.transform.localScale;
        burnParticle.transform.parent = enemy.transform;
        enemy.StartBurning();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
