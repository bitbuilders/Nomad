using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using TMPro;

public class InputField : MonoBehaviour
{
    [System.Serializable]
    public enum OutputType
    {
        CHAT_ROOM,
        PARTY
    }

    [SerializeField] ChatRoom m_chatRoom = null;
    [SerializeField] OutputType m_outputType = OutputType.CHAT_ROOM;

    TMP_InputField m_inputField = null;

    void Start()
    {
        m_inputField = GetComponent<TMP_InputField>();
    }

    private void SendMessages()
    {
        if (m_inputField.text.Trim().Length <= 0)
        {
            m_inputField.text = "";
            m_inputField.ActivateInputField();
            return;
        }

        string message = m_inputField.text;
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        string username = localPlayer.Color + localPlayer.UserName + ": " + Colors.ColorSuffix;
        string fullMessage = username + message;
        
        switch (m_outputType)
        {
            case OutputType.CHAT_ROOM:
                LocalPlayerData.Instance.LocalPlayer.GetComponent<ChatRoomMessenger>().SendMessageToRoom(fullMessage, m_chatRoom.ID);
                break;
            case OutputType.PARTY:
                PartyManager.Instance.SendChatMessage(fullMessage);
                break;
        }

        m_inputField.text = "";
        m_inputField.ActivateInputField();
    }

    public void OnValueChange()
    {
        if (m_inputField.text.Contains("\n") && !string.IsNullOrEmpty(m_inputField.text.Trim()))
        {
            m_inputField.text = m_inputField.text.Replace("\n", "").Trim();
            SendMessages();
        }
        else if (string.IsNullOrEmpty(m_inputField.text.Trim()))
        {
            m_inputField.text = "";
        }
    }
}
