using MigalhaSystem.Extensions;
using MigalhaSystem.LifeSystem;
using System.Collections;
using System.Collections.Generic;
using Trigger.System2D;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionWeapon : PowerUps
{
    [SerializeField] Animator m_boomAnimator;
    [SerializeField] AnimationScriptableObject m_boomAnimation;
    [SerializeField] Timer m_timer;
    [SerializeField] DamageSettings m_damageSettings;
    [SerializeField] CircleTrigger2D m_circleTrigger;
    [SerializeField] UnityEvent m_onExplode;
    [SerializeField] AudioSourceController m_audio;
    
    private void OnEnable()
    {
        m_timer.OnTimerElapsed.AddListener(Explosion);
        m_onLevelUp.AddListener(ExplosionLevelUp);

    }

    private void OnDisable()
    {
        m_timer.OnTimerElapsed.RemoveListener(Explosion);
        m_onLevelUp.RemoveListener(ExplosionLevelUp);
    }

    void Update()
    {
        if (!CanApplyEffect()) return;
        m_timer.TimerElapse(Time.deltaTime);
    }

    void ExplosionLevelUp()
    {
        float area = m_currentLevel + 1;
        m_circleTrigger.SetTriggerRadius(area);
        m_timer.DecreaseStartTimerValue(1);
    }

    void Explosion()
    {
        m_onExplode?.Invoke();
        m_audio.Play();
        m_boomAnimation.Play(m_boomAnimator);
        bool isIn = m_circleTrigger.InTrigger(transform, out List<Collider2D> collider);
        if (!isIn) return;
        float currentDamageValue = m_damageSettings.GetDamageValue();
        foreach (Collider2D c in collider)
        {
            IDamage idamage = c.GetComponent<IDamage>();
            if (idamage is null) continue;
            idamage?.Damage(currentDamageValue);
        }
    }

    private void OnDrawGizmosSelected()
    {
        m_circleTrigger.DrawTrigger(transform);
    }
}