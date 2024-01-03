using JetBrains.Annotations;
using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PowerUps : MonoBehaviour
{
    [SerializeField] PowerUpData m_powerUpData;
    public PowerUpData m_PowerUpData => m_powerUpData;
    protected PlayerManager m_playerManager;
    protected PlayerAttackController m_playerAttackController;
    public bool m_Active
    {
        get
        {
            return m_currentLevel >= 1;
        }
    }
    [SerializeField] protected IntRange m_levelRange = new IntRange(0, 5);
    [SerializeField] protected int m_startLevel;
    protected int m_currentLevel;
    [SerializeField] protected UnityEvent m_onLevelUp;

    private void Awake()
    {
        m_currentLevel = m_startLevel;
    }

    protected virtual void Start()
    {
        m_playerManager = PlayerManager.Instance;
        m_playerAttackController = m_playerManager.m_PlayerAttackController;
    }

    protected virtual bool CanApplyEffect()
    {
        if (!m_playerAttackController.m_CanAttack) return false;
        if (!m_Active) return false;

        return true;
    }

    [ContextMenu("Level Up!")]
    public void LevelUp()
    {
        m_currentLevel++;
        m_onLevelUp?.Invoke();
        if (m_currentLevel >= m_levelRange.m_MaxValue)
        {
            m_playerAttackController.AddMaxPowerUp(this);
            m_currentLevel = m_levelRange.m_MaxValue;
            return;
        }
    }

    public bool CheckPowerUpData(PowerUpData powerUpData)
    {
        return m_powerUpData == powerUpData;
    }

    public bool InMaxLevel()
    {
        return m_currentLevel >= m_levelRange.m_MaxValue;
    }

    private void OnValidate()
    {
        if (m_startLevel <= m_levelRange.m_MinValue)
        {
            m_startLevel = m_levelRange.m_MinValue;
        }
        if (m_startLevel > m_levelRange.m_MaxValue)
        {
            m_startLevel = m_levelRange.m_MaxValue;
        }
    }
}