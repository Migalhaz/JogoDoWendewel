using MigalhaSystem.LifeSystem;
using MigalhaSystem.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeSystem : BasicLifeSystem, ISpawnable
{
    [SerializeField] EnemyController m_enemyController;
    [SerializeField] PoolDataScriptableObject m_enemyPoolData;
    PoolManager m_poolManager;
    PoolManager m_PoolManager
    {
        get
        {
            if (m_poolManager == null)
            {
                m_poolManager = PoolManager.Instance;
            }
            return m_poolManager;
        }
    }
    [SerializeField] LifeSystemEvents m_events;

    [Header("SFX")]
    [SerializeField] EnemyAudio m_enemyAudio;
    public override LifeSystemEvents Events()
    {
        return m_events;
    }
    public void ReturnToPool()
    {
        m_PoolManager.ReturnUsingGameObjectToPool(m_enemyPoolData, gameObject);

    }

    public override void Death()
    {
        base.Death();
        m_enemyAudio.PlayDeathSFX(transform.position);
        m_enemyController.PickDrop();
        ReturnToPool();
    }

    public void SetupBySpawn()
    {
        m_alive = true;
        m_currentHp = m_startHp;
        m_enemyAudio.PlaySpawnSFX();
    }
}
