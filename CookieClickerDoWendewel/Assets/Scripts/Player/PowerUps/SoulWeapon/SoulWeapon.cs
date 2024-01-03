using MigalhaSystem.Extensions;
using MigalhaSystem.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulWeapon : PowerUps
{
    [SerializeField] List<Soul> m_souls;

    void Update()
    {
        if (!CanApplyEffect()) return;
        foreach (Soul soul in m_souls)
        {
            if (!soul.CheckActive(m_currentLevel))
            {
                continue;
            }
            soul.TimerElapse(Time.deltaTime);
        }
    }
}

[System.Serializable]
public class Soul : Timer
{
    [Header("Weapon Settings")]
    [SerializeField, Range(0, 5)] int m_levelToActive;

    bool m_active;
    public bool m_Active => m_active;

    [SerializeField] Transform m_firePoint;
    [SerializeField] PoolDataScriptableObject m_bulletPoolData;
    PoolManager m_poolManager;

    public bool CheckActive(int currentLevel)
    {
        m_active = currentLevel == m_levelToActive;
        m_firePoint.gameObject.SetActive(m_active);
        return m_active;
    }

    public override void TimerElapsedAction()
    {
        base.TimerElapsedAction();
        Shoot();
    }

    void Shoot()
    {
        m_poolManager ??= PoolManager.Instance;
        GameObject bullet = m_poolManager.GetFreeGameObjectFromPool(m_bulletPoolData);
        bullet.transform.position = m_firePoint.position;
        ISpawnable[] spawnable = bullet.GetComponents<ISpawnable>();

        if (spawnable == null) return;
        if (spawnable.Length <= 0) return;

        foreach (ISpawnable s in spawnable)
        {
            s.SetupBySpawn();
        }
    }
}