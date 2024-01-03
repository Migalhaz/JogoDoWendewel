using MigalhaSystem.Extensions;
using MigalhaSystem.Pool;
using System.Collections;
using System.Collections.Generic;
using Trigger.System2D;
using UnityEngine;

public class XpPoint : MonoBehaviour, ISpawnable
{

    [SerializeField] BoxTrigger2D m_boxTrigger;
    [SerializeField] XpDrop m_drop;
    XpManager m_xpManager;
    [SerializeField] PoolDataScriptableObject m_poolDataScriptableObject;
    [SerializeField] Timer m_lifeTime;
    PoolManager m_poolManager;
    [SerializeField, Min(0)] float m_rotateSpeed;
    [SerializeField] AudioClipController m_audio;
    void Start()
    {
        m_xpManager = PlayerManager.Instance.m_XpManager;
        m_poolManager = PoolManager.Instance;
    }

    void Update()
    {
        m_lifeTime.TimerElapse(Time.deltaTime);
        transform.Rotate(RotateFinalSpeed());
    }

    Vector3 RotateFinalSpeed()
    {
        return m_rotateSpeed * Time.deltaTime * Vector3.forward;
    }

    private void OnEnable()
    {
        m_lifeTime.OnTimerElapsed.AddListener(ReturnPool);
    }

    private void OnDisable()
    {
        m_lifeTime.OnTimerElapsed.RemoveListener(ReturnPool);
    }

    void ReturnPool()
    {
        m_poolManager.ReturnUsingGameObjectToPool(m_poolDataScriptableObject, gameObject);
    }

    private void FixedUpdate()
    {
        bool hit = m_boxTrigger.InTrigger(transform);
        if (hit)
        {
            m_xpManager.AddXp(m_drop.m_currentXpDrop);
            m_audio.Play(transform.position);
            ReturnPool();
        }
    }

    private void OnDrawGizmos()
    {
        m_boxTrigger.DrawTrigger(transform);
    }

    public void SetupBySpawn()
    {
        m_lifeTime.SetupTimer();
    }
}
