using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuSetup : MonoBehaviour
{
    [SerializeField] Button settingsButton;
    [SerializeField] Canvas mainMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        Settings settings = SaveAcrossScenes.master.GetComponentInChildren<Settings>();
        settingsButton.onClick.AddListener(settings.OpenSettingsPanel);
        Button[] allMainMenuButtons = mainMenuCanvas.transform.GetComponentsInChildren<Button>();
        for (int i = 0; i < allMainMenuButtons.Length; i++)
        {
            allMainMenuButtons[i].onClick.AddListener(AudioManager.master.ClickButtonSound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
