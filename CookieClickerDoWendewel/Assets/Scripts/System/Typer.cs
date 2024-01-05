using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MigalhaSystem.Extensions;

public class Typer : MonoBehaviour
{
    TextMeshProUGUI m_text;
    [SerializeField] AudioSourceController m_audioSourceController;
    string m_finalText;
    [SerializeField, Min(0)] float m_timeInterval;
    void Awake()
    {
        TryGetComponent(out m_text);
        m_finalText = m_text.text;
    }

    private void OnEnable()
    {
        StartCoroutine(Type());
    }

    private void OnDisable()
    {
        m_text.text = m_finalText;
    }

    IEnumerator Type()
    {
        m_text.text = "";
        string msg = "";
        foreach (char c in m_finalText)
        {
            msg += c;
            m_text.text = msg;
            m_audioSourceController.Play();
            yield return MigalhazHelper.GetWaitForSeconds(m_timeInterval);
        }
        m_text.text = m_finalText;
    }
}
