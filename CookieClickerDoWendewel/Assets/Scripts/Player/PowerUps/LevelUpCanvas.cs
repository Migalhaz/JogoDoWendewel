using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MigalhaSystem.Extensions;

public class LevelUpCanvas : MonoBehaviour
{
    PlayerLifeSystem m_playerLifeSystem;
    PlayerAttackController m_playerAttackController;
    [SerializeField, Min(1)] int m_lifeIncreaseValue;
    [SerializeField] List<PowerUpData> m_availablePowerUps;

    [Header("Buttons")]
    [SerializeField] List<PowerUpButton> m_buttons;
    [SerializeField] Button m_hpUpButton;
    [SerializeField] Button m_confirmButton;

    PowerUpButton m_currentChoice;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI m_description;
    [SerializeField] string m_hpUpDescription;
    public TextMeshProUGUI m_Description => m_description;

    private void Start()
    {
        m_playerAttackController = PlayerManager.Instance.m_PlayerAttackController;
        m_playerLifeSystem = PlayerManager.Instance.m_PlayerLifeSystem;
        m_confirmButton.onClick.AddListener(ApplyEffect);
    }

    public void SetupCanvas()
    {
        CheckPowerUpsAvailable();
        List<PowerUpData> availablePowerUps = new(m_availablePowerUps);
        m_currentChoice = null;

        switch (availablePowerUps.Count)
        {
            case 0:
                SetupButtons(availablePowerUps, false, false, false);
                break;
            case 1:
                SetupButtons(availablePowerUps, false, true, false);
                break;
            case 2:
                SetupButtons(availablePowerUps, true, false, true);
                break;
            default:
                SetupButtons(availablePowerUps, true, true, true);
                break;
        }

        m_confirmButton.gameObject.SetActive(false);
        m_description.text = "";
    }

    void SetupButtons(List<PowerUpData> availablePowerUps, bool b1Active = true, bool b2Active = true, bool b3Active = true)
    {
        m_buttons[0].m_Button.gameObject.SetActive(b1Active);
        m_buttons[1].m_Button.gameObject.SetActive(b2Active);
        m_buttons[2].m_Button.gameObject.SetActive(b3Active);

        if (b1Active)
        {
            SetButtonByIndex(0);

        }
        if (b2Active)
        {
            SetButtonByIndex(1);
        }
        if (b3Active)
        {
            SetButtonByIndex(2);
        }

        void SetButtonByIndex(int index)
        {
            PowerUpData powerUpData = availablePowerUps.GetRandom();
            availablePowerUps.Remove(powerUpData);
            m_buttons[index].SetupButton(powerUpData, this);
            m_buttons[index].m_Button.onClick.AddListener(ActiveConfirmButton);
        }
    }

    public void CheckPowerUpsAvailable()
    {
        if (MaxPowerUpListAvailable())
        {
            foreach (PowerUps p in m_playerAttackController.m_MaxPowerUps)
            {
                m_availablePowerUps.Remove(p.m_PowerUpData);
            }
        }

        bool MaxPowerUpListAvailable()
        {
            if (m_playerAttackController == null)
            {
                m_playerAttackController = PlayerManager.Instance.m_PlayerAttackController;
            }
            if (m_playerAttackController.m_MaxPowerUps == null) return false;
            if (m_playerAttackController.m_MaxPowerUps.Count <= 0) return false;
            return true;
        }
    }

    public void SetIncreaseHpChoice()
    {
        m_description.text = m_hpUpDescription;
        m_currentChoice = null;
        ActiveConfirmButton();
    }

    public void SetChoice(PowerUpButton powerUpButton)
    {
        m_currentChoice = powerUpButton;
    }

    public void ActiveConfirmButton()
    {
        m_confirmButton.gameObject.SetActive(true);
    }

    public void ApplyEffect()
    {
        if (m_currentChoice != null)
        {
            m_playerAttackController.GetPowerUp(m_currentChoice.m_PowerUpData).LevelUp();
        }
        else
        {
            m_playerLifeSystem.IncreaseLife(m_lifeIncreaseValue);
        }
        CheckPowerUpsAvailable();
        GameManager gameManager = GameManager.Instance;
        gameManager.PauseTime(false);
        gameManager.DisableCanvas("LevelUpCanvas");
        gameManager.EnableCanvas(gameManager.m_basicUi);
    }
}

[System.Serializable]
public class PowerUpButton
{
    [SerializeField] Button m_button;
    [SerializeField] TextMeshProUGUI m_text;
    [SerializeField] RawImage m_icon;
    PowerUpData m_powerUpData;
    LevelUpCanvas m_levelUpCanvasScript;
    public Button m_Button => m_button;
    public TextMeshProUGUI m_Text => m_text;
    public RawImage m_Icon => m_icon;
    public PowerUpData m_PowerUpData => m_powerUpData;

    public void SetupButton(PowerUpData newPowerUpData, LevelUpCanvas levelUpCanvas)
    {
        m_powerUpData = newPowerUpData;
        m_levelUpCanvasScript = levelUpCanvas;

        m_text.text = m_powerUpData.m_PowerUpName;
        if (m_powerUpData.m_PowerUpIcon != null)
        {
            m_icon.texture = m_powerUpData.m_PowerUpIcon.texture;
        }
        m_button.onClick.AddListener(Action);
    }

    public void Action()
    {
        m_levelUpCanvasScript.m_Description.text = m_powerUpData.m_PowerUpDescription;
        m_levelUpCanvasScript.SetChoice(this);
    }
}