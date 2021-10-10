using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] TextMeshProUGUI pingText;
    void Awake()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
    private void LateUpdate()
    {
        UpdatePing();
    }

    public void UpdatePing()
    {
        if (PhotonNetwork.InRoom)
        {
            pingText.gameObject.SetActive(true);
            pingText.text = PhotonNetwork.GetPing().ToString();
        }
        else pingText.gameObject.SetActive(false);
    }
}
