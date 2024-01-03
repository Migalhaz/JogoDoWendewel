using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public abstract class MoveSystem
{
    [Header("Components")]
    [SerializeField] protected Transform m_transform;
    

    [Header("Move Settings")]
    [SerializeField, Min(0)] protected float m_moveSpeed;
    protected Vector3 m_moveDirection;

    public Transform m_Transform => m_transform;
    public float m_MoveSpeed => m_moveSpeed;
    public Vector3 m_MoveDirection => m_moveDirection;

    public abstract Vector3 MoveFinalVector();

    public abstract void Move();
}

[System.Serializable]
public class MoveInDirection : MoveSystem
{
    public void SetDirection(Vector3 direction)
    {
        m_moveDirection = direction;
    }
    public override void Move()
    {
        m_transform.Translate(MoveFinalVector(), Space.World);
    }

    public override Vector3 MoveFinalVector()
    {
        return m_moveSpeed * Time.deltaTime * m_moveDirection;
    }
}

public abstract class RigMoveSystem : MoveSystem
{
    [Header("Rigidbody2D")]
    [SerializeField] Rigidbody2D m_rig;

    public override void Move()
    {
        m_rig.velocity = MoveFinalVector();
    }
}

public abstract class FollowerMove : RigMoveSystem
{
    public override Vector3 MoveFinalVector()
    {
        m_moveDirection = (Target().position - m_transform.position).normalized;
        return m_moveSpeed * m_moveDirection;
    }
    protected abstract Transform Target();
}

[System.Serializable]
public class PlayerInstaFollow : MoveSystem
{
    [SerializeField] Vector3 m_offset;
    Transform m_playerTransform;
    public override void Move()
    {
        m_transform.position = MoveFinalVector();
    }

    public override Vector3 MoveFinalVector()
    {
        m_playerTransform ??= PlayerManager.Instance.transform;
        return m_playerTransform.position + m_offset;
    }
}

[System.Serializable]
public class PlayerLateFollow : PlayerInstaFollow
{
    [SerializeField, Range(0, 1)] float m_strenght;
    public override void Move()
    {
        m_transform.position = Vector3.Lerp(m_transform.position, MoveFinalVector(), m_strenght * m_moveSpeed * DeltaTime());
    }

    public float DeltaTime()
    {
        return Time.deltaTime;
    }
}

[System.Serializable]
public class PlayerFollower : FollowerMove
{
    Transform m_playerTransform;
    protected override Transform Target()
    {
        m_playerTransform ??= PlayerManager.Instance.transform;
        return m_playerTransform;
    }
}

[System.Serializable]
public class PlayerMove : RigMoveSystem
{
    float m_lookingDirectionX;
    public float m_LookingDirectionX => m_lookingDirectionX;

    public override Vector3 MoveFinalVector()
    {
        return m_moveSpeed * m_moveDirection;
    }

    public void SetVector3(Vector3 inputVector)
    {
        float xInput = inputVector.x;
        m_moveDirection = inputVector;
        if (xInput != 0)
        {
            m_lookingDirectionX = xInput;
        }
    }

    public bool IsMoving()
    {
        return MoveFinalVector().magnitude > 0;
    }


}