using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MigalhaSystem.Singleton<GameManager>
{
    [SerializeField] List<CanvasSwitcher> m_canvasSwitcher;
    bool m_paused;
    public bool m_Paused => m_paused;

    public List<string> m_basicUi { get; private set; } = new List<string>() { "XpBarCanvas", "LifeBarCanvas" };
    private void Start()
    {
        PauseTime(false);

        foreach (CanvasSwitcher canvas in m_canvasSwitcher)
        {
            if (canvas.m_StartActive)
            {
                canvas.EnableCanvas();
            }
            else
            {
                canvas.DisableCanvas();
            }
        }
    }

    public void PauseTime(bool pause)
    {
        m_paused = pause;
        float timeScale = m_paused ? 0 : 1;
        Time.timeScale = timeScale;
    }

    public void PauseGame(bool pause)
    {
        PauseTime(pause);
        if (pause)
        {
            EnableOnlyCanvas("PauseCanvas");
        }
        else
        {
            DisableCanvas("PauseCanvas");
            EnableOnlyCanvas(m_basicUi);
        }
    }
    public void CloseGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void RestartScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SwitchScene(sceneIndex);
    }

    public void EnableCanvas(string canvasTagToActive)
    {
        foreach (CanvasSwitcher canvas in m_canvasSwitcher)
        {
            if (canvas.CompareTag(canvasTagToActive))
            {
                canvas.EnableCanvas();
            }
        }
    }
    public void EnableCanvas(List<string> canvasTagToActive)
    {
        foreach (CanvasSwitcher canvas in m_canvasSwitcher)
        {
            if (canvas.CompareTag(canvasTagToActive))
            {
                canvas.EnableCanvas();
            }
        }
    }
    public void DisableAllCanvas()
    {
        foreach (CanvasSwitcher canvas in m_canvasSwitcher)
        {
            canvas.DisableCanvas();
        }
    }
    public void DisableCanvas(string canvasTagToActive)
    {
        foreach (CanvasSwitcher canvas in m_canvasSwitcher)
        {
            if (canvas.CompareTag(canvasTagToActive))
            {
                canvas.DisableCanvas();
            }
        }
    }
    public void DisableCanvas(List<string> canvasTagToActive)
    {
        foreach (CanvasSwitcher canvas in m_canvasSwitcher)
        {
            if (canvas.CompareTag(canvasTagToActive))
            {
                canvas.DisableCanvas();
            }
        }
    }
    public void EnableOnlyCanvas(string canvasTagToActive)
    {
        foreach (CanvasSwitcher canvas in m_canvasSwitcher)
        {
            if (canvas.CompareTag(canvasTagToActive))
            {
                canvas.EnableCanvas();
            }
            else
            {
                canvas.DisableCanvas();
            }
        }
    }
    public void EnableOnlyCanvas(List<string> canvasTagToActive)
    {
        foreach (CanvasSwitcher canvas in m_canvasSwitcher)
        {
            if (canvas.CompareTag(canvasTagToActive))
            {
                canvas.EnableCanvas();
            }
            else
            {
                canvas.DisableCanvas();
            }
        }
    }
    public bool IsCanvasActive(string canvasTag)
    {
        CanvasSwitcher canvas = m_canvasSwitcher.Find(x => x.CompareTag(canvasTag));
        if (canvas == null)
        {
            return false;
        }
        return canvas.m_Active;
    }
}


[System.Serializable]
public class CanvasSwitcher
{
    public bool m_Active
    {
        get
        {
            return m_canvas.activeSelf;
        }
    }
    [SerializeField] bool m_startActive;
    [SerializeField] string m_canvasTag;
    [SerializeField] GameObject m_canvas;
    [SerializeField] UnityEvent m_OnCanvasEnabled;
    [SerializeField] UnityEvent m_OnCanvasDisabled;
    public bool m_StartActive => m_startActive;
    public void EnableCanvas(bool active)
    {
        if (active)
        {
            EnableCanvas();
        }
        else
        {
            DisableCanvas();
        }
    }

    public void EnableCanvas()
    {
        m_canvas.SetActive(true);
        m_OnCanvasEnabled?.Invoke();
    }

    public void DisableCanvas()
    {
        m_canvas.SetActive(false);
        m_OnCanvasDisabled?.Invoke();
    }

    public bool CanvasActive()
    {
        return m_canvas.activeSelf;
    }

    public bool CompareTag(string tag)
    {
        return m_canvasTag.Equals(tag);
    }

    public bool CompareTag(List<string> tag)
    {
        return tag.Contains(m_canvasTag);
    }
}