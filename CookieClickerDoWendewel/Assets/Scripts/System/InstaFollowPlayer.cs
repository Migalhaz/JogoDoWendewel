using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaFollowPlayer : MonoBehaviour
{
    [SerializeField] PlayerInstaFollow m_moveSettings;
    void Update()
    {
        m_moveSettings.Move();
    }
}
