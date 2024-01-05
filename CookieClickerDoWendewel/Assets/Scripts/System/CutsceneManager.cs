using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.SwitchScene("Menu");
    }
}
