using MigalhaSystem.Extensions;
using MigalhaSystem.LifeSystem;
using System.Collections;
using System.Collections.Generic;
using Trigger.System2D;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] PlayerFollower m_enemyMove;
    [SerializeField] DamageSettings m_enemyDamageSettings;
    [SerializeField] CircleTrigger2D m_hitbox;
    [SerializeField] XpDrop m_drop;

    private void FixedUpdate()
    {
        CheckHitBox();
        m_enemyMove.Move();
    }

    void CheckHitBox()
    {
        bool hit = m_hitbox.InTrigger(transform, out List<Collider2D> colliders);
        if (!hit) return;
        float damage = m_enemyDamageSettings.GetDamageValue();
        foreach (Collider2D c in colliders)
        {
            c.GetComponent<IDamage>().Damage(damage);
        }
    }

    public void PickDrop()
    {
        float xpAmount = m_drop.m_currentXpDrop;
        PlayerManager.Instance.m_XpManager.AddXp(xpAmount);
    }

    private void OnDrawGizmos()
    {
        m_hitbox.DrawTrigger(transform);
    }
}

[System.Serializable]
public class DamageSettings
{
    [SerializeField] FloatRange m_damageRange = new FloatRange(1, 1);
    [SerializeField, Min(1)] float m_criticalMultiplier = 1;
    [SerializeField, Range(0, 100)] float m_criticalChance = 10;
    
    public float GetDamageValue()
    {
        float currentDamage = m_damageRange.GetRandomValue();
        bool criticalHit = Random.value <= (m_criticalChance * 0.01f);
        currentDamage = criticalHit ? currentDamage * m_criticalMultiplier : currentDamage;
        return currentDamage;
    }
}

[System.Serializable]
public struct XpDrop
{
    [SerializeField] IntRange m_xpRange;
    public float m_currentXpDrop => m_xpRange.GetRandomValue();
}
