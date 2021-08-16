using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartGameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject cancelButton;
    public GameObject messageBox;

    public bool timerOn;
    private void Awake()
    {
        IdleMenu();
    }
    public void LookForPlayers()
    {
        messageBox.SetActive(true);
        playButton.SetActive(false);
        cancelButton.SetActive(true);
        timerOn = true;
    }
    public void IdleMenu()
    {
        messageBox.SetActive(false);
        playButton.SetActive(true);
        cancelButton.SetActive(false);
        timerOn = false;
    }
}
