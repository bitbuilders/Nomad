using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DirectMessageInterface : MonoBehaviour
{
    [SerializeField] TMP_InputField m_conversationNameInput = null;
    [SerializeField] TextMeshProUGUI m_notificationText = null;

    public void CreateNewConversation()
    {
        string playerName = m_conversationNameInput.text;
        if (LocalPlayerData.Instance.PlayerExists(playerName))
        {
            DirectMessageManager.Instance.CreateMessageRoom(playerName);
        }
        else
        {
            m_notificationText.gameObject.SetActive(true);
        }
    }

    public void InitializePlayerNameField()
    {
        m_conversationNameInput.text = "";
        m_conversationNameInput.ActivateInputField();
    }
}
