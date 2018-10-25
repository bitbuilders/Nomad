﻿using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.Networking;

public class ChatRoom : MonoBehaviour
{
    [SerializeField] RectTransform m_containerBounds = null;
    [SerializeField] RectTransform m_roomViewTransform = null;
    [SerializeField] TextMeshProUGUI m_chatLog = null;
    [SerializeField] TextMeshProUGUI m_roomNameText = null;
    [SerializeField] GameObject m_nameChange = null;
    [SerializeField] TextMeshProUGUI m_namePlaceholder = null;
    [SerializeField] TMP_InputField m_nameInputField = null;
    [SerializeField] GameObject m_buttons = null;
    [SerializeField] GameObject m_chatRoom = null;
    [SerializeField] GameObject m_inputField = null;
    //[SerializeField] [Range(0.0f, 10.0f)] float m_slideSpeed = 1.0f;

    public int ID { get; private set; }
    public string Name { get; private set; }
    
    StringBuilder m_text;
    float m_targetHeight;
    float m_currentHeight;
    float m_time;

    void Start()
    {
        UpdateRoomName();
        m_text = new StringBuilder(m_chatLog.text);
        string welcomeMessage = Colors.ConvertToColor("Welcome to the chat room!", Colors.ColorType.WHITE);
        AddMessage(welcomeMessage);
    }

    public void Initialize(int roomID, string name = "Chat Room")
    {
        Name = name;
        m_buttons.gameObject.SetActive(false);
        m_chatRoom.gameObject.SetActive(false);
        m_inputField.gameObject.SetActive(false);
        ChangeRoomName();

        if (roomID < 0)
            ID = 0;
        else
            ID = roomID;
    }

    public void SetRoomName(string roomName)
    {
        Name = roomName;
    }

    public void SetRoomName(TMP_InputField roomName)
    {
        string name = roomName.text;
        if (!string.IsNullOrEmpty(name.Trim()))
        {
            Name = name;
        }
    }

    public void ChangeRoomName()
    {
        m_nameChange.SetActive(true);
        m_nameInputField.ActivateInputField();
    }

    public void UpdateRoomName()
    {
        m_roomNameText.text = Name;
        m_namePlaceholder.text = Name;
        m_nameInputField.text = "";
    }

    void Update()
    {
        //if (m_time <= 1.0f)
        //{
        //    m_time += Time.deltaTime * m_slideSpeed;
        //    float t = Interpolation.BounceOut(m_time);
        //    float size = Mathf.Lerp(m_currentHeight, m_targetHeight, t);
        //    m_containerBounds.sizeDelta = new Vector2(m_containerBounds.sizeDelta.x, size);
        //}
    }

    public void AddMessage(string text)
    {
        if (m_text.Length > 0)
        {
            m_text.Append("\n");
            m_currentHeight = m_containerBounds.sizeDelta.y;
        }
        else
        {
            m_currentHeight = 0.0f;
        }

        m_text.Append(text);
        m_chatLog.text = m_text.ToString();

        Vector2 bounds = m_roomViewTransform.sizeDelta;
        float padding = m_chatLog.margin.x * 2.0f;
        Vector2 size = m_chatLog.GetPreferredValues(m_chatLog.text, bounds.x - padding, bounds.y - padding);
        m_containerBounds.sizeDelta = new Vector2(m_containerBounds.sizeDelta.x, size.y);
        m_targetHeight = size.y;
        m_time = 0.0f;
    }

    public void DestroyChatRoom()
    {
        ChatRoomManager.Instance.RemoveChatRoom(gameObject);
    }
}
