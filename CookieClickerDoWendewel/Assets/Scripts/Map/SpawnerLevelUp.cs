using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLevelUp : MonoBehaviour
{
    [SerializeField] SpawnerController m_spawnerController;
    XpManager m_xpManager;
    [SerializeField] List<LevelUpSpawner> m_levelUpSpawner;

    private void Start()
    {
        m_xpManager = PlayerManager.Instance.m_XpManager;
        m_xpManager.m_OnLevelUp.AddListener(LevelUpSpawners);
    }

    void LevelUpSpawners()
    {
        foreach (LevelUpSpawner levelUpSpawner in m_levelUpSpawner)
        {
            levelUpSpawner.LevelUp(m_spawnerController, m_xpManager.m_CurrentPlayerLevel);
        }
    }
}

[System.Serializable]
public class LevelUpSpawner 
{
    [SerializeField] string m_spawnerTag;
    public string m_SpawnerTag => m_spawnerTag;
    [SerializeField] List<SpawnerLevelUpSettings> m_spawnerLevelUpSettings;
    public List<SpawnerLevelUpSettings> m_SpawnerLevelUpSettings => m_spawnerLevelUpSettings;
    public void LevelUp(SpawnerController spawnerController, int playerCurrentLevel)
    {
        SpawnerSettings spawnerSettings = spawnerController.GetSpawnerSettings(m_spawnerTag);
        foreach (SpawnerLevelUpSettings s in m_spawnerLevelUpSettings)
        {
            if (s.LevelUp(playerCurrentLevel))
            {
                spawnerSettings.SpawnerLevelUp(s);
            }
        }
    }
}



[System.Serializable]
public class SpawnerLevelUpSettings
{
    
    [SerializeField, Min(2)] int m_playerLevelToUpdate;
    [SerializeField] FloatRange m_newStartTimerRange;
    [SerializeField] IntRange m_newSpawnObjectsRange;
    public FloatRange m_NewStartTimerRange => m_newStartTimerRange;
    public IntRange m_NewSpawnObjectsRange => m_newSpawnObjectsRange;

    public bool LevelUp(int currentPlayerLevel)
    {
        return m_playerLevelToUpdate == currentPlayerLevel;
    }
}
