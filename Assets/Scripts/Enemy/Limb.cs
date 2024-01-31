using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    public float damageThreshold = 50f; // 血量阈值，低于这个值将触发断肢
    public float limbForce = 10f; // 断肢时施加的力的大小

    public bool limbAdded = false; // 标记是否已经添加刚体
    public GameObject limb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
