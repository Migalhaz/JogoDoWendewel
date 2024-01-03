using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateFollowPlayer : MonoBehaviour
{
    [SerializeField] PlayerLateFollow m_moveSystem;
    [SerializeField] UpdateMethod m_updateMethod;
    void Update()
    {
        if (m_updateMethod == UpdateMethod.Update)
        {
            m_moveSystem.Move();
        }
    }

    private void FixedUpdate()
    {
        if (m_updateMethod == UpdateMethod.FixedUpdate)
        {
            m_moveSystem.Move();
        }
    }

    private void LateUpdate()
    {
        if (m_updateMethod == UpdateMethod.LateUpdate)
        {
            m_moveSystem.Move();
        }
    }
}
