using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squeeze : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的对象是否是敌人
        if (other.CompareTag("Enemy"))
        {
            // 摧毁敌人游戏对象
            Destroy(other.gameObject);
        }
    }
}
