using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputField : MonoBehaviour
{
    [SerializeField] TMP_InputField m_inputField = null;
    [SerializeField] List<ChatRoom> m_pairedRooms = null;

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

    private void SendMessages()
    {
        if (m_inputField.text.Length <= 0)
            return;

        string message = m_inputField.text;
        foreach (ChatRoom room in m_pairedRooms)
        {
            room.AddMessage(message);
        }
        
        m_inputField.text = "";
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
