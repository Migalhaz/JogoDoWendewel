using MigalhaSystem.Pool;
using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] List<SpawnerSettings> m_spawnerSettings;
    private void Awake()
    {
        foreach (SpawnerSettings spawnerSettings in m_spawnerSettings)
        {
            spawnerSettings.m_Spawner.SetupTimer();
        }
    }

    private void Update()
    {
        foreach (SpawnerSettings spawnerSettings in m_spawnerSettings)
        {
            spawnerSettings.m_Spawner.TimerElapse(Time.deltaTime);
        }
    }

    public SpawnerSettings GetSpawnerSettings(string tag)
    {
        return  m_spawnerSettings.Find(x => x.CompareTag(tag));
    }
}

[System.Serializable]
public class SpawnerSettings
{
    [SerializeField] string m_spawnerTag;
    [SerializeField] Spawner m_spawner;

    public string m_SpawnerTag => m_spawnerTag;
    public Spawner m_Spawner => m_spawner;
    public bool CompareTag(string tag)
    {
        return tag.Equals(m_spawnerTag);
    }

    public void ActiveSpawn(bool active)
    {
        m_spawner.ActiveTimer(active);
        if (active)
        {
            m_spawner.SetupTimer();
        }
    }

    public void SpawnerLevelUp(SpawnerLevelUpSettings newSettings)
    {
        ActiveSpawn(true);
        m_spawner.SetStartTimer(newSettings.m_NewStartTimerRange);
        m_spawner.SetSpawnObjectsRange(newSettings.m_NewSpawnObjectsRange);
    }
}

[System.Serializable]
public class Spawner : Timer
{
    PoolManager m_poolManager;
    [Header("Pool Settings")]
    [SerializeField] List<PoolDataScriptableObject> m_poolData;
    [Header("Spawn Settings")]
    [SerializeField] IntRange m_minSpawnObjects = new(1, 1); 
    [SerializeField] List<Transform> m_spawnPoints;
    public List<PoolDataScriptableObject> m_PoolData => m_poolData;
    public List<Transform> m_SpawnPoints => m_spawnPoints;

    public void SetSpawnObjectsRange(IntRange newRange)
    {
        m_minSpawnObjects = newRange;
    }

    public override void TimerElapsedAction()
    {
        base.TimerElapsedAction();
        Spawn();
    }

    protected void Spawn()
    {
        m_poolManager ??= PoolManager.Instance;
        int spawnObject = m_minSpawnObjects.GetRandomValue(true);
        spawnObject = spawnObject >= m_spawnPoints.Count ? m_spawnPoints.Count : spawnObject;

        List<Transform> availableSpawnPoints = new();
        availableSpawnPoints.AddRange(m_spawnPoints);

        for (int i = 0; i < spawnObject; i++)
        {
            SpawnObject();
        }

        void SpawnObject()
        {
            PoolDataScriptableObject pool = m_PoolData.GetRandom();
            GameObject gameObject = m_poolManager.GetFreeGameObjectFromPool(pool);
            Transform currentSpawnPoint = availableSpawnPoints.GetRandom();
            gameObject.transform.position = currentSpawnPoint.position;
            availableSpawnPoints.Remove(currentSpawnPoint);
            ISpawnable[] spawnableComponents = gameObject.GetComponents<ISpawnable>();
            if (!SpawnableComponentsAvailable()) return;

            foreach (ISpawnable s in spawnableComponents)
            {
                s.SetupBySpawn();
            }

            bool SpawnableComponentsAvailable()
            {
                if (spawnableComponents == null) return false;
                if (spawnableComponents.Length <= 0) return false;
                return true;
            }
        }
    }
}

interface ISpawnable
{
    void SetupBySpawn();
}