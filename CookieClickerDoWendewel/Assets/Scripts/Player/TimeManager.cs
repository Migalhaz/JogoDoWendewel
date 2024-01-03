using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    float m_minutes;
    float m_seconds;
    [SerializeField] TMPro.TextMeshProUGUI m_tmpro;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_seconds += Time.deltaTime;
        if (m_seconds >= 59.4f)
        {
            m_seconds = 0;
            m_minutes++;
        }
        m_tmpro.text = (m_minutes.ToString("00") + ":" + m_seconds.ToString("00"));
    }
}
