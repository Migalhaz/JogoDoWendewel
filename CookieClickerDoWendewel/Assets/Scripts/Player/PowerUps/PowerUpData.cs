using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up Data", menuName = "Scriptable Object/Power Up/New Power Up Data")]
public class PowerUpData : ScriptableObject
{
    [SerializeField] string m_powerUpName;
    [SerializeField, TextArea(3, 10)] string m_powerUpDescription;
    [SerializeField] Sprite m_powerUpIcon;
    public string m_PowerUpName => m_powerUpName;
    public string m_PowerUpDescription => m_powerUpDescription;
    public Sprite m_PowerUpIcon => m_powerUpIcon;
}
