using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator m_animator;
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] PlayerController m_playerController;

    [Header("Animation Settings")]
    [SerializeField] AnimationScriptableObject m_playerIdleAnimation;
    [SerializeField] AnimationScriptableObject m_playerWalkingAnimation;

    GameManager m_gameManager;
    void Start()
    {
        m_gameManager = GameManager.Instance;
        m_playerIdleAnimation.Play(m_animator);
    }

    void Update()
    {
        if (m_gameManager.m_Paused) return;
        ControlAnimation();
        ControlSprite();
    }

    void ControlSprite()
    {
        float lookingDirection = m_playerController.m_PlayerMove.m_LookingDirectionX;
        if (lookingDirection > 0)
        {
            m_spriteRenderer.flipX = false;
        }
        if (lookingDirection < 0)
        {
            m_spriteRenderer.flipX = true;
        }
    }

    void ControlAnimation()
    {
        if (m_playerController.m_PlayerMove.IsMoving())
        {
            PlayWalkAnimation();
        }
        else
        {
            PlayIdleAnimation();
        }
    }


    void PlayWalkAnimation()
    {
        if (!m_playerWalkingAnimation.IsAnimationPlaying(m_animator))
        {
            m_playerWalkingAnimation.Play(m_animator);
        }
    }

    void PlayIdleAnimation()
    {
        if (!m_playerIdleAnimation.IsAnimationPlaying(m_animator))
        {
            m_playerIdleAnimation.Play(m_animator);
        }
    }
}
