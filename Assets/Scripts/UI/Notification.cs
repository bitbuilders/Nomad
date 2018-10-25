using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    public enum NotificationType
    {
        ROOM_INVITE
    }

    [SerializeField] TextMeshProUGUI m_notificationTypeText = null;
    [SerializeField] TextMeshProUGUI m_senderNameText = null;
    [SerializeField] TextMeshProUGUI m_roomNameText = null;

    public NotificationType Type { get; private set; }
    int m_roomID;

    public void Initialize(NotificationType type, string senderName, string roomName, int roomID)
    {
        Type = type;
        m_roomID = roomID;
        UpdateText(senderName, roomName);
    }

    public void AcceptInvitation()
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        ChatRoomManager.Instance.CreateChatRoom(m_roomID, m_roomNameText.text);
        string joinMessage = Colors.ConvertToColor(localPlayer.UserName + " has joined the room!", Colors.ColorType.WHITE);
        localPlayer.GetComponent<ChatRoomMessenger>().SendMessageToRoom(joinMessage, m_roomID);
        Destroy(gameObject);
    }

    public void DeclineInvitation()
    {
        Destroy(gameObject);
    }

    public void AcknowledgeNotification()
    {

    }

    void UpdateText(string sender, string room)
    {
        m_senderNameText.text = sender;

        switch (Type)
        {
            case NotificationType.ROOM_INVITE:
                m_notificationTypeText.text = "Room Invite";
                m_roomNameText.text = room;
                break;
        }
    }
}
