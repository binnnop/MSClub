using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ChefHeal : EnemySkill
{
    public float skillRadius = 5f;
    public GameObject Fx;
    public float skillEffectDuration = 3f;
    public int healAmount;

    override protected void PerformSkill()
    {
        Fx.SetActive(true);

        // 获取碰撞体内所有带有EnemyAI组件的物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, skillRadius);

        foreach (var collider in colliders)
        {
            EnemyAI enemyAI = collider.GetComponent<EnemyAI>();

            if (enemyAI != null)
            {
                // 增加HitPoint值
                enemyAI.HitPoint += healAmount;
                if (enemyAI.HitPoint > enemyAI.maxHitPoint)
                {
                    enemyAI.HitPoint = enemyAI.maxHitPoint;
                }
            }
        }

        // 开启特效
      

        // 等待一段时间
        StartCoroutine(HideSkillEffectAfterDelay(skillEffectDuration));
    }

    private IEnumerator HideSkillEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Fx.SetActive(false);
    }
}
