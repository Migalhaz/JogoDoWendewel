using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SateliteWeapon : PowerUps
{
    [SerializeField] PlayerInstaFollow m_moveSystem;
    [SerializeField, Min(0)] float m_rotateSpeed;
    [SerializeField] List<Satellite> m_satellites;

    private void OnEnable()
    {
        m_onLevelUp.AddListener(CheckLevelUp);
    }

    private void OnDisable()
    {
        m_onLevelUp.RemoveListener(CheckLevelUp);
    }

    private void Update()
    {
        if (CanApplyEffect())
        {
            m_moveSystem.Move();
            m_moveSystem.m_Transform.Rotate(AngleToRotate());
        }
    }

    protected override bool CanApplyEffect()
    {
        return m_Active;
    }

    void CheckLevelUp()
    {
        m_satellites.ForEach(x => x.Active(m_currentLevel));
    }

    Vector3 AngleToRotate()
    {
        return m_rotateSpeed * Time.deltaTime * Vector3.forward;
    }
}

[System.Serializable]
public class Satellite
{
    [SerializeField, Range(1, 5)] int m_levelToActivate = 1;
    [SerializeField] List<GameObject> m_satellites;

    public void Active(int currentLevel)
    {
        if (currentLevel == m_levelToActivate)
        {
            EnableSatellites();
        }
        else
        {
            DisableSatellites();
        }
    }

    void EnableSatellites()
    {
        m_satellites.ForEach(x => x.SetActive(true));
    }

    void DisableSatellites()
    {
        m_satellites.ForEach(x => x.SetActive(false));
    }
}