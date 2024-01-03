using System.Collections;
using System.Collections.Generic;
using MigalhaSystem;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] PlayerController m_playerMove;
    [SerializeField] PlayerAttackController m_playerAttackController;
    [SerializeField] PlayerLifeSystem m_playerLifeSystem;
    [SerializeField] XpManager m_xpManager;
    public PlayerController m_PlayerMove => m_playerMove;
    public PlayerAttackController m_PlayerAttackController => m_playerAttackController;
    public PlayerLifeSystem m_PlayerLifeSystem => m_playerLifeSystem;
    public XpManager m_XpManager => m_xpManager;

}
