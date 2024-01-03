using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MigalhaSystem.Extensions;

namespace MigalhaSystem.LifeSystem
{
    [System.Serializable]
    public abstract class GenericLifeSystem : MonoBehaviour
    {
        [Header("Life System")]
        [SerializeField] protected bool m_alive = true;
        [SerializeField] protected float m_startHp;
        [SerializeField] protected IntRange m_hpRange;
        [SerializeField] protected float m_currentHp;
        public IntRange m_HpRange => m_hpRange;
        protected virtual void Awake()
        {
            m_currentHp = m_startHp;
        }

        public virtual bool IsAlive()
        {
            if (m_currentHp <= m_hpRange.m_MinValue)
            {
                m_alive = false;
            }
            return m_alive;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (m_startHp >= m_hpRange.m_MaxValue)
            {
                m_startHp = m_hpRange.m_MaxValue;
            }
            if (m_startHp <= m_hpRange.m_MinValue)
            {
                m_startHp = m_hpRange.m_MinValue;
            }
        }
#endif
    }

    [System.Serializable]
    public abstract class BasicLifeSystem : GenericLifeSystem, IDie
    {
        public virtual void Damage(float damage)
        {
            if (!IsAlive()) return;

            m_currentHp -= damage;
            Events().OnHpChange?.Invoke();
            Events().OnTakeDamage?.Invoke();
            if (!IsAlive())
            {
                Death();
            }
        }

        public virtual void Death()
        {
            m_currentHp = m_hpRange.m_MinValue;
            Events().OnHpMin?.Invoke();
            Events().OnDie?.Invoke();
        }

        public abstract LifeSystemEvents Events();
    }

    [System.Serializable]
    public class LifeSystemEvents
    {
        [SerializeField] protected UnityEvent onHpMin;
        [SerializeField] protected UnityEvent onHpMax;
        [SerializeField] protected UnityEvent onHpChange;
        [SerializeField] protected UnityEvent onTakeDamage;
        [SerializeField] protected UnityEvent onDie;

        public UnityEvent OnHpMin => onHpMin;
        public UnityEvent OnHpMax => onHpMax;
        public UnityEvent OnHpChange => onHpChange;
        public UnityEvent OnTakeDamage => onTakeDamage;
        public UnityEvent OnDie => onDie;
    }

    [System.Serializable]
    public class PlayerLifeSystemEvents : LifeSystemEvents
    {
        [SerializeField] protected UnityEvent onHeal;
        public UnityEvent OnHeal => onHeal;
    }

    interface IDamage
    {
        public void Damage(float damage);
    }

    interface IHeal
    {
        public void Heal(float heal);
    }

    interface IDie : IDamage
    {
        public void Death();
    }
}