using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using TMPro;
using UnityStandardAssets.Characters.ThirdPerson;

public class InputField : MonoBehaviour
{
    [SerializeField] ChatRoom m_chatRoom = null;

    TMP_InputField m_inputField = null;

    void Start()
    {
        m_inputField = GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SendMessages();
        }
    }

    private void SendMessages()
    {
        if (m_inputField.text.Replace(" ", "").Length <= 0)
        {
            m_inputField.text = "";
            m_inputField.ActivateInputField();
            return;
        }

        string message = m_inputField.text;
        LocalPlayerData.Instance.LocalPlayer.GetComponent<ChatRoomMessenger>().SendMessageToRoom(message, m_chatRoom.ID);
        //m_chatRoom.LocalOwner.GetComponent<ChatRoomMessenger>().SendMessageToRoom(message, m_chatRoom.ID);

        m_inputField.text = "";
        m_inputField.ActivateInputField();
    }

    public void Select()
    {

    }

    public void DeSelect()
    {

    }

    public void Submit()
    {
    }
}
