using System.Collections;
using System.Collections.Generic;
using MigalhaSystem.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class HealPowerUp : PowerUps
{
    PlayerLifeSystem m_playerLifeSystem;
    PlayerController m_playerController;
    [SerializeField] Timer m_timer;
    [SerializeField] UnityEvent m_OnHeal;
    

    private void OnEnable()
    {
        m_timer.OnTimerElapsed.AddListener(Heal);
    }
    
    private void OnDisable()
    {
        m_timer.OnTimerElapsed.RemoveListener(Heal);
    }

    protected override void Start()
    {
        base.Start();
        m_playerLifeSystem = m_playerManager.m_PlayerLifeSystem;
        m_playerController = m_playerManager.m_PlayerMove;
    }

    private void Update()
    {
        if (!CanApplyEffect()) return;
        m_timer.TimerElapse(Time.deltaTime);
    }

    protected override bool CanApplyEffect()
    {
        if (m_playerController.m_PlayerMove.IsMoving()) return false;
        return m_Active;
    }

    void Heal()
    {
        m_OnHeal?.Invoke();
        float healValue = 0.02f * m_currentLevel * m_playerLifeSystem.m_HpRange.m_MaxValue;
        m_playerLifeSystem.Heal(healValue);
    }
}
