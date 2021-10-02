using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PanelMessageColor
{
    headerFailTextColor,
    regularFailTextColor,
    regularSuccessTextColor,
    neutralColor
}
public class PanelMessagesManager : MonoBehaviour
{
    public static PanelMessagesManager master;

    [SerializeField] GameObject panelMessagePrefab;
    [SerializeField] Transform messagePanelContent;

    [SerializeField] Color headerFailTextColor = Color.white;
    [SerializeField] Color regularFailTextColor = Color.white;
    [SerializeField] Color regularSuccessTextColor = Color.white;
    [SerializeField] Color neutralTextColor = Color.white;
    private void Awake()
    {
        if (master != null) Destroy(this);
        master = this;
    }

    /// <summary>
    /// Instantiate a message in the message panel
    /// </summary>
    /// <param name="message">The message to send</param>
    /// <param name="messageColor">The text-color of the message</param>
    public void InstantiateMessage(string message, PanelMessageColor color)
    {
        GameObject newMessage = Instantiate(panelMessagePrefab, messagePanelContent);
        newMessage.transform.GetComponent<PanelMessage>().SetText(message);

        Color textColor = Color.white;
        switch (color)
        {
            case PanelMessageColor.headerFailTextColor:
                textColor = headerFailTextColor;
                break;
            case PanelMessageColor.regularFailTextColor:
                textColor = regularFailTextColor;
                break;
            case PanelMessageColor.regularSuccessTextColor:
                textColor = regularSuccessTextColor;
                break;
            case PanelMessageColor.neutralColor:
                textColor = neutralTextColor;
                break;
            default:
                textColor = Color.white;
                break;
        }

        newMessage.transform.GetComponent<TextMeshProUGUI>().color = textColor;
        if (AudioManager.master != null) AudioManager.master.MessageReceivedSound();
    }
}
