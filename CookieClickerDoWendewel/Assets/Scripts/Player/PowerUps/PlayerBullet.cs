using MigalhaSystem.LifeSystem;
using MigalhaSystem.Pool;
using System.Collections;
using System.Collections.Generic;
using Trigger.System2D;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] CircleTrigger2D m_circleTrigger;
    [SerializeField] DamageSettings m_damageSettings;
    [SerializeField] protected UnityEvent m_onBulletCollision;
    [SerializeField] AudioSourceController m_collisionAudio;
    protected virtual void Update()
    {
        ColliderCheck();
    }

    protected virtual void ColliderCheck()
    {
        bool inTrigger = m_circleTrigger.InTrigger(transform, out List<Collider2D> colliders);
        if (!inTrigger) return;
        foreach (Collider2D c in colliders)
        {
            IDie lifeSystem = c.GetComponent<IDie>();
            lifeSystem.Damage(m_damageSettings.GetDamageValue());
        }
        BulletCollision();
    }

    protected virtual void BulletCollision()
    {
        m_onBulletCollision?.Invoke();
        m_collisionAudio.Play();
    }
    private void OnDrawGizmos()
    {
        m_circleTrigger.DrawTrigger(transform);
    }
}
