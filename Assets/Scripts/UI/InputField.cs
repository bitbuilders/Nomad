using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using TMPro;

public class InputField : MonoBehaviour
{
    [SerializeField] TMP_InputField m_inputField = null;
    [SerializeField] List<ChatRoom> m_pairedRooms = null;

    NetworkClient m_myClient;

    bool m_focused;

    void Start()
    {
        m_inputField = GetComponent<TMP_InputField>();
        m_focused = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && m_focused)
        {
            SendMessages();
        }
    }

    public void Init()
    {
        m_myClient = new NetworkClient();
        m_myClient.Connect("localhost", 7777);
        m_myClient.RegisterHandler(MsgType.Connect, OnConnected);
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
        var msg = new StringMessage("It worked!");
        m_myClient.Send(1002, msg);
        foreach (ChatRoom room in m_pairedRooms)
        {
            //room.AddMessage(message);
        }

        m_inputField.text = "";
        m_inputField.ActivateInputField();
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }

    public void Select()
    {
        m_focused = true;
    }

    public void DeSelect()
    {
        m_focused = false;
    }

    public void Submit()
    {

    }
}
