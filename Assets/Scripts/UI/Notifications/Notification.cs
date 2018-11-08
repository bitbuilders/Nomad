using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    public enum NotificationType
    {
        ROOM_INVITE,
        PARTY_INVITE
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

        switch (Type)
        {
            case NotificationType.ROOM_INVITE:
                ChatRoomManager.Instance.CreateChatRoom(m_roomID, true, m_roomNameText.text);
                string joinMessage = Colors.ConvertToColor(localPlayer.UserName + " has joined the room!", Colors.ColorType.WHITE);
                localPlayer.GetComponent<ChatRoomMessenger>().SendMessageToRoom(joinMessage, m_roomID);
                break;
            case NotificationType.PARTY_INVITE:
                PartyManager.Instance.SetupParty(m_senderNameText.text);
                break;
        }

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
                m_roomNameText.gameObject.SetActive(true);
                break;
            case NotificationType.PARTY_INVITE:
                m_notificationTypeText.text = "Party Invite";
                m_roomNameText.gameObject.SetActive(false);
                break;
        }
    }
}
