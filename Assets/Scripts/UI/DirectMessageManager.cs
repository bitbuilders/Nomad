using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DirectMessageManager : Singleton<DirectMessageManager>
{
    [SerializeField] GameObject m_messageRoom = null;
    [SerializeField] TextMeshProUGUI m_messageRoomText = null;
    [SerializeField] TextMeshProUGUI m_messageRoomTitleText = null;
    [SerializeField] ChatRoom m_messageChatRoom = null;

    List<PlayerMessageRoom> m_messageRooms;
    PlayerMessageRoom m_currentRoom;
    Transform m_roomLocation;

    private void Start()
    {
        m_roomLocation = GameObject.Find("Player Rooms").GetComponent<Transform>();
        m_messageRooms = new List<PlayerMessageRoom>();
    }

    public void CreateMessageRoom(string playerName)
    {
        GameObject go = Instantiate(m_messageRoom, Vector3.zero, Quaternion.identity, m_roomLocation);
        TextMeshProUGUI pName = go.GetComponentInChildren<TextMeshProUGUI>();
        pName.text = playerName;
        PlayerMessageRoom pmr = go.GetComponent<PlayerMessageRoom>();
        pmr.Name = playerName;
        m_messageRooms.Add(pmr);
    }

    public void SetCurrentMessageRoom(PlayerMessageRoom room)
    {
        m_currentRoom = room;
        m_messageRoomText.text = room.Messages;
        m_messageRoomTitleText.text = room.Name;
    }

    public void AddMessageToRoom(string message)
    {

    }
}
