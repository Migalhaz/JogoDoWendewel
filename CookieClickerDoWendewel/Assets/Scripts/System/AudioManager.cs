using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem;
public class AudioManager : Singleton<AudioManager>
{
    public bool m_AudioActive { get; private set; } = true;
    [SerializeField] AudioSource m_music;
    [SerializeField] AudioSource m_buttonSound;

    public bool CanPlay()
    {
        return m_AudioActive;
    }

    public void SwitchAudioActive()
    {
        SetAudioActive(!m_AudioActive);
    }
    public void SetAudioActive(bool active)
    {
        if (active)
        {
            m_music.Play();
        }
        else
        {
            m_music.Stop();
        }
        m_AudioActive = active;
    }

    public void PlayAudioSource(AudioSource audioSource)
    {
        if (!CanPlay()) return;
        audioSource.Play();
    }

    public void PlayClipAtPoint(AudioClip audioClip, Vector3 pos, float volume = 1)
    {
        if (!CanPlay()) return;
        AudioSource.PlayClipAtPoint(audioClip, pos, volume);
    }

    public void PlayButtonSound()
    {
        PlayAudioSource(m_buttonSound);
    }

}

public abstract class AudioController
{
    AudioManager m_audioManager;
    protected AudioManager m_AudioManager
    {
        get
        {
            if (m_audioManager is null)
            {
                m_audioManager = AudioManager.Instance;
            }
            return m_audioManager;
        }
    }
}

[System.Serializable]
public class EnemyAudio : AudioController
{
    [SerializeField] AudioSource m_spawnSFX;
    [SerializeField] AudioClip m_deathSFX;

    public void PlaySpawnSFX()
    {
        m_AudioManager.PlayAudioSource(m_spawnSFX);
    }

    public void PlayDeathSFX(Vector3 enemyPosition)
    {
        m_AudioManager.PlayClipAtPoint(m_deathSFX, enemyPosition);
    }
}

[System.Serializable]
public class AudioSourceController : AudioController
{
    [SerializeField] AudioSource m_audio;

    public void Play()
    {
        m_AudioManager.PlayAudioSource(m_audio);
    }
}

[System.Serializable]
public class AudioClipController : AudioController
{
    [SerializeField] AudioClip m_audio;
    [SerializeField, Range(0, 1)] float m_volume = 1;

    public void Play(Vector3 pos)
    {
        m_AudioManager.PlayClipAtPoint(m_audio, pos, m_volume);
    }
}

[System.Serializable]
public class PlayerHealthAudio : AudioController
{
    [SerializeField] AudioSource m_healAudio;
    [SerializeField] AudioSource m_damageAudio;
    [SerializeField] AudioSource m_deathAudio;

    public void PlayHealSFX()
    {
        m_AudioManager.PlayAudioSource(m_healAudio);
    }
    
    public void PlayDamageSFX()
    {
        m_AudioManager.PlayAudioSource(m_damageAudio);
    }
    
    public void PlayDeathSFX()
    {
        m_AudioManager.PlayAudioSource(m_deathAudio);
    }
}