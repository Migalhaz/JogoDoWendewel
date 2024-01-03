using MigalhaSystem.Extensions;
using MigalhaSystem.LifeSystem;
using MigalhaSystem.Pool;
using System.Collections;
using System.Collections.Generic;
using Trigger.System2D;
using UnityEngine;

public class SoulBullet : PlayerBullet, ISpawnable
{
    [SerializeField] Timer m_lifeTime;
    PoolManager m_poolManager;
    [SerializeField] MoveInDirection m_moveSystem;
    [SerializeField] PoolDataScriptableObject m_soulPool;

    void OnEnable()
    {
        m_lifeTime.ActiveTimer(true);
        m_lifeTime.OnTimerElapsed.AddListener(BulletDeath);
        m_onBulletCollision.AddListener(BulletDeath);
    }

    private void OnDisable()
    {
        m_lifeTime.ActiveTimer(false);
        m_lifeTime.OnTimerElapsed.RemoveListener(BulletDeath);
        m_onBulletCollision.RemoveListener(BulletDeath);

    }

    protected override void Update()
    {
        m_moveSystem.Move();
        m_lifeTime.TimerElapse(Time.deltaTime);
        base.Update();
    }

    void BulletDeath()
    {
        m_lifeTime.SetupTimer();
        m_poolManager.ReturnUsingGameObjectToPool(m_soulPool, gameObject);
    }

    public void SetupBySpawn()
    {
        m_lifeTime.SetupTimer();
        m_poolManager = PoolManager.Instance;
        PlayerAttackController m_playerAttackController = PlayerManager.Instance.m_PlayerAttackController;
        if (!m_playerAttackController.m_CanAttack)
        {
            BulletDeath();
            return;
        }

        Transform target = m_playerAttackController.m_NearestEnemy.transform;
        if (target == null)
        {
            BulletDeath();
            return;
        }
        Vector3 direction = (target.position - transform.position).normalized;
        m_moveSystem.SetDirection(direction);
    }
}
