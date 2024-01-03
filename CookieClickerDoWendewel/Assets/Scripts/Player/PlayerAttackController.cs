using System.Collections;
using System.Collections.Generic;
using Trigger.System2D;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] CircleTrigger2D m_circleTrigger;
    [SerializeField] List<PowerUps> m_powerUps;
    List<PowerUps> m_maxPowerUps;
    bool m_canAttack;
    GameObject m_nearestEnemy;

    public List<PowerUps> m_PowerUps => m_powerUps;
    public List<PowerUps> m_MaxPowerUps => m_maxPowerUps;

    public bool m_CanAttack => m_canAttack;
    public GameObject m_NearestEnemy => m_nearestEnemy;

    private void Awake()
    {
        m_maxPowerUps = new();
    }

    void Update()
    {
        SetupAttack();
    }

    void SetupAttack()
    {
        m_canAttack = m_circleTrigger.InTrigger(transform, out List<Collider2D> collider);

        if (m_canAttack)
        {
            m_nearestEnemy = GetNearestEnemy(collider);
        }
    }

    GameObject GetNearestEnemy(List<Collider2D> enemies)
    {
        GameObject nearestEnemy = enemies[0].gameObject;
        foreach (Collider2D c in enemies)
        {
            nearestEnemy = CheckNearEnemy(nearestEnemy, c.gameObject);
        }


        GameObject CheckNearEnemy(GameObject enemy1, GameObject enemy2)
        {
            float enemy1Distance = Vector2.Distance(enemy1.transform.position, transform.position);
            float enemy2Distance = Vector2.Distance(enemy2.transform.position, transform.position);

            if (enemy1Distance < enemy2Distance)
            {
                return enemy1;
            }
            if (enemy2Distance < enemy1Distance)
            {
                return enemy2;
            }
            return Random.value < 0.5f ? enemy1 : enemy2;
        }
        return nearestEnemy;
    }

    public PowerUps GetPowerUp(PowerUpData powerUpData)
    {
        return m_powerUps.Find(x => x.CheckPowerUpData(powerUpData));
    }

    public void AddMaxPowerUp(PowerUps powerUp)
    {
        m_maxPowerUps.Add(powerUp);
    }

    private void OnDrawGizmosSelected()
    {
        m_circleTrigger.DrawTrigger(transform);
    }
}
