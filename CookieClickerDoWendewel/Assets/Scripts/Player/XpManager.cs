using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XpManager : MonoBehaviour
{
    int m_currentPlayerLevel;
    public int m_CurrentPlayerLevel => m_currentPlayerLevel;
    public float m_XpToNextLevel
    {
        get
        {
            return m_currentPlayerLevel * 10f;
        }
    }
    float m_currentXp;
    public float m_xpPercentage
    {
        get
        {
            return m_currentXp / m_XpToNextLevel;
        }
    }
    [Header("Events")]
    [SerializeField] UnityEvent m_onLevelUp;
    [SerializeField] List<OnReachLevel> m_onReachLevels;
    public UnityEvent m_OnLevelUp => m_onLevelUp;
    [Header("UI Settings")]
    [SerializeField] UnityEngine.UI.Image m_fillImage;
    [SerializeField] TMPro.TextMeshProUGUI m_xpText;

    private void Awake()
    {
        m_currentPlayerLevel = 1;
        m_currentXp = 0;
    }
    private void Update()
    {
        m_fillImage.fillAmount = m_xpPercentage;
        m_xpText.text = $"Level Atual: {m_currentPlayerLevel}";
    }

    public void AddXp(float xpValue)
    {
        m_currentXp += xpValue;
        CheckXpAmount();

        void CheckXpAmount()
        {
            if (m_currentXp >= m_XpToNextLevel)
            {
                LevelUp();
                CheckXpAmount();
            }
        }
        void LevelUp()
        {
            m_currentXp -= m_XpToNextLevel;
            m_onLevelUp?.Invoke();
            m_currentPlayerLevel++;
            if (ListIsAvailable(out OnReachLevel levelEvent))
            {
                levelEvent?.InvokeEvent();
            }

            bool ListIsAvailable(out OnReachLevel onReachLevel)
            {
                onReachLevel = null;
                if (m_onReachLevels == null) return false;
                onReachLevel = m_onReachLevels.Find(x => x.InLevel(m_currentPlayerLevel));
                if (onReachLevel == null) return false;
                return m_onReachLevels.Count > 0;
            }
        }
    }
       
}


[System.Serializable]
public class OnReachLevel
{
    [SerializeField, Min(2)] int m_levelToReach = 2;
    [SerializeField] UnityEvent m_onReach = new();
    public UnityEvent m_OnReach => m_onReach;
    public bool InLevel(int newLevel)
    {
        return m_levelToReach == newLevel;
    }

    public void InvokeEvent()
    {
        if (m_onReach == null) return;
        m_onReach?.Invoke();
    }
}