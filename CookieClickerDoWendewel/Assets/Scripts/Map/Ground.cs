using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Transform m_playerTransform;
    [SerializeField, Min(0)] float m_distanceToTp;
    void Start()
    {
        m_playerTransform = PlayerManager.Instance.transform;
    }

    void Update()
    {
        MoveGround();
    }

    void MoveGround()
    {
        Vector3 finalPosition = transform.position;
        Vector3 playerLocalPosition = m_playerTransform.position - transform.position;
        float playerAbsXPosition = Mathf.Abs(playerLocalPosition.x);
        float playerAbsYPosition = Mathf.Abs(playerLocalPosition.y);

        if (playerAbsXPosition >= m_distanceToTp)
        {
            finalPosition.x = m_playerTransform.position.x;
            transform.position = finalPosition;
        }

        if (playerAbsYPosition >= m_distanceToTp)
        {
            finalPosition.y = m_playerTransform.position.y;
            transform.position = finalPosition;
        }
    }
}
