using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    GameManager m_gameManager;
    Vector3 m_moveInputDirection;
    [SerializeField] KeyCode m_pauseKey = KeyCode.Escape;
    public Vector3 m_MoveInputDirection => m_moveInputDirection;
    private void Start()
    {
        m_gameManager = GameManager.Instance;
    }
    void Update()
    {
        PauseInput();
        if (m_gameManager.m_Paused) return;

        
        MoveInput();
    }

    void PauseInput()
    {
        if (!CanPause()) return;
        if (Input.GetKeyDown(m_pauseKey))
        {
            m_gameManager.PauseGame(!m_gameManager.m_Paused);
        }

        bool CanPause()
        {
            if (m_gameManager.IsCanvasActive("LevelUpCanvas")) return false;
            if (m_gameManager.IsCanvasActive("DeathCanvas")) return false;
            return true;
        }
    }

    void MoveInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        m_moveInputDirection.Set(x, y, 0);
    }
}
