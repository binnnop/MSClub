using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCollision : MonoBehaviour
{
    public float animScale=0.1f;
    private LightningAI targetTower;
    private GameObject targetLine;

    void OnTriggerEnter(Collider other)
    {
        // �����ײ���˹������˺�
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>().hurt(5);
            GameObject blood = Instantiate(Resources.Load("Lightning") as GameObject,other.transform.position + new Vector3(0, 0.5f, 0), other.transform.rotation);
            
            ParticleSystem particleSystem = blood.GetComponent<ParticleSystem>();
            //float scale = Mathf.Lerp(180f, 1000f, (maxHitPoint - 5) / 95);
            var mainModule = particleSystem.main;
            /*
            mainModule.startSize = animScale;
            particleSystem.Stop();
            particleSystem.Play();
            */
            //Destroy(blood, mainModule.duration);
        }
    }
    public void SetTargetTower(LightningAI tower,GameObject line)
    {
        targetTower = tower;
        targetLine = line;
    }

    void Update()
    {
        // ��ÿһ֡�������ĵ����Ƿ���ڣ���������ڣ�������ײ��
        if (targetTower == null)
        {
            Destroy(targetLine);
            Destroy(gameObject);   
        }
    }

}
