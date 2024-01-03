using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem.LifeSystem;
using UnityEngine.UI;
using MigalhaSystem.Extensions;

public class PlayerLifeSystem : BasicLifeSystem, IHeal
{
    public float m_HpPercentage
    {
        get
        {
            return (m_currentHp / m_hpRange.m_MaxValue);
        }
    }
    bool m_canTakeDamage;
    [SerializeField, Min(0)] float m_iFrameSeconds;

    [Header("UI Settings")]
    [SerializeField] Image m_fillImage;
    [SerializeField] Color m_fullHpColor = Color.green;
    [SerializeField] Color m_lowHpColor = Color.red;
    [SerializeField] SpriteRenderer m_playerSpriteRenderer;

    [Header("Events")]
    [SerializeField] PlayerLifeSystemEvents m_events;

    [Header("SFX")]
    [SerializeField] PlayerHealthAudio m_audio;
    public override LifeSystemEvents Events()
    {
        return m_events;
    }

    private void OnEnable()
    {
        m_events.OnHpChange.AddListener(UpdateUI);
    }

    private void OnDisable()
    {
        m_events.OnHpChange.RemoveListener(UpdateUI);
    }

    protected override void Awake()
    {
        base.Awake();
        m_canTakeDamage = true;
    }

    public override void Death()
    {
        m_audio.PlayDeathSFX();
        base.Death();
    }

    public override void Damage(float damage)
    {
        if (!m_canTakeDamage) return;
        m_audio.PlayDamageSFX();
        StartCoroutine(IFrames());
        base.Damage(damage);
    }

    void UpdateUI()
    {
        m_fillImage.fillAmount = m_HpPercentage;
        m_fillImage.color = Color.Lerp(m_lowHpColor, m_fullHpColor, m_HpPercentage);
    }

    public void HealByPercentage(float percentage)
    {
        Heal(m_HpRange.m_MaxValue * percentage * 0.01f);
    }

    public void Heal(float heal)
    {
        if (!IsAlive()) return;
        PlayerLifeSystemEvents playerLifeSystemEvents = (PlayerLifeSystemEvents) Events();
        

        m_currentHp += heal;
        if (m_currentHp >= m_hpRange.m_MaxValue)
        {
            m_currentHp = m_hpRange.m_MaxValue;
        }
        else
        {
            m_audio.PlayHealSFX();
        }
        playerLifeSystemEvents.OnHpChange?.Invoke();
        playerLifeSystemEvents.OnHeal?.Invoke();
    }

    IEnumerator IFrames()
    {
        m_canTakeDamage = false;
        float iframeMultiplier = 5;
        Color cacheColor = m_playerSpriteRenderer.color;
        for (int i = 0; i < m_iFrameSeconds * iframeMultiplier; i++)
        {
            m_playerSpriteRenderer.color = Color.clear;
            yield return MigalhazHelper.GetWaitForSeconds(0.1f);
            m_playerSpriteRenderer.color = cacheColor;
            yield return MigalhazHelper.GetWaitForSeconds(0.1f);
        }
        m_canTakeDamage = true;
    }

    public void IncreaseLife(int increaseValue)
    {
        int maxLife = m_hpRange.m_MaxValue;
        m_hpRange.ChangeMaxValue(maxLife + increaseValue);
        m_currentHp += increaseValue;
    }
}