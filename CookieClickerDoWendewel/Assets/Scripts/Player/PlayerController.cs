using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInputManager m_playerInputManager;
    [SerializeField] PlayerMove m_playerMove;
    public PlayerMove m_PlayerMove => m_playerMove;

    private void Update()
    {
        if (GameManager.Instance.m_Paused) return;
        m_playerMove.SetVector3(m_playerInputManager.m_MoveInputDirection);
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.m_Paused) return;
        m_playerMove.Move();
    }
}
