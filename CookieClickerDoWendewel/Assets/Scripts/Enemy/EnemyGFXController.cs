using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFXController : MonoBehaviour
{
    Transform m_playerTransform;
    Transform m_PlayerTransform
    {
        get
        {
            if (m_playerTransform == null)
            {
                m_playerTransform = PlayerManager.Instance.transform;
            }
            return m_playerTransform;
        }
    }
    [SerializeField] SpriteRenderer m_spriteRenderer;
    void OnEnable()
    {
        m_spriteRenderer.color = Color.white;
    }

    void Update()
    {
        SpriteDirection();
    }

    void SpriteDirection()
    {
        m_spriteRenderer.flipX = FlipX();
        bool FlipX()
        {
            return transform.position.x >= m_PlayerTransform.position.x;
        }
    }

    public void Damage()
    {
        StartCoroutine(DamageFeedback());
    }

    IEnumerator DamageFeedback()
    {
        m_spriteRenderer.color = Color.red;
        yield return MigalhazHelper.GetWaitForSeconds(0.1f);
        m_spriteRenderer.color = Color.white;
    }
}
