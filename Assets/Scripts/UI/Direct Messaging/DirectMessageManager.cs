using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DirectMessageManager : Singleton<DirectMessageManager>
{
    [SerializeField] GameObject m_messageRoom = null;
    [SerializeField] RectTransform m_messageRoomContainer = null;
    [SerializeField] RectTransform m_messageRoomParent = null;
    [SerializeField] TextMeshProUGUI m_messageRoomText = null;
    [SerializeField] TextMeshProUGUI m_messageRoomTitleText = null;
    [SerializeField] Transform m_roomLocation = null;
    [SerializeField] NotificationImageAlert m_notificationAlert;

    List<PlayerMessageRoom> m_messageRooms;
    public PlayerMessageRoom CurrentRoom;

    private void Start()
    {
        m_messageRooms = new List<PlayerMessageRoom>();
    }

    public PlayerMessageRoom CreateMessageRoom(string playerName, string startMessage = "")
    {
        if (RoomExists(playerName))
        {
            return m_messageRooms.Where(room => room.Name == playerName).ToList()[0];
        }

        GameObject go = Instantiate(m_messageRoom, Vector3.zero, Quaternion.identity, m_roomLocation);
        TextMeshProUGUI pName = go.GetComponentInChildren<TextMeshProUGUI>();
        pName.text = playerName;
        PlayerMessageRoom pmr = go.GetComponent<PlayerMessageRoom>();
        pmr.StartMessage = startMessage;
        pmr.Initialize(m_notificationAlert);
        pmr.Name = playerName;
        m_messageRooms.Add(pmr);

        return pmr;
    }

    public void SetCurrentMessageRoom(PlayerMessageRoom room)
    {
        CurrentRoom = room;
        m_messageRoomText.text = room.Messages;
        m_messageRoomTitleText.text = room.Name;

        Vector2 parentX = m_messageRoomParent.sizeDelta;
        Vector2 sizeY = m_messageRoomContainer.sizeDelta;
        float padding = m_messageRoomText.margin.x * 2.0f;
        Vector2 size = m_messageRoomText.GetPreferredValues(m_messageRoomText.text, parentX.x - padding, sizeY.y - padding);
        m_messageRoomContainer.sizeDelta = new Vector2(m_messageRoomContainer.sizeDelta.x, size.y);
    }

    public void AddMessageToRoom(string message, string sender, string recipient)
    {
        bool hasRoom = false;
        int ignore = 1;
        foreach (PlayerMessageRoom room in m_messageRooms)
        {
            if (room.Name == sender || room.Name == recipient)
            {
                room.AddMessage(message);
                hasRoom = true;

                if (room == CurrentRoom)
                {
                    SetCurrentMessageRoom(room);

                    if (GameLobby.Instance.DMOpen)
                        ignore = 0;
                }
            }
        }

        if (!hasRoom)
        {
            PlayerMessageRoom pmr = CreateMessageRoom(sender, message);
            m_notificationAlert.AddChild(pmr.NotificationAlert);
        }

        m_notificationAlert.UpdateStatus(ignore);
    }

    bool RoomExists(string playerName)
    {
        bool exists = false;

        foreach (PlayerMessageRoom room in m_messageRooms)
        {
            if (room.Name == playerName)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }
}
