using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatRoomManager : NetworkBehaviour
{
    [SerializeField] GameObject m_roomTemplate = null;

    Player m_localPlayer;
    List<ChatRoom> m_chatRooms;
    Transform m_roomLocation = null;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        m_chatRooms = new List<ChatRoom>();
        m_localPlayer = GetComponent<Player>();
        m_roomLocation = GameObject.Find("Chat Rooms").transform;
        CreateChatRoom(-1);
    }

    public void CreateChatRoom(int roomID)
    {
        GameObject obj = Instantiate(m_roomTemplate, Vector3.zero, Quaternion.identity, m_roomLocation);
        ChatRoom chatRoom = obj.GetComponentInChildren<ChatRoom>();
        m_chatRooms.Add(chatRoom);
        chatRoom.Initialize(m_localPlayer, roomID);
    }

    public void SendMessage(int roomID, string message)
    {
        string fullMessage = m_localPlayer.UserName + ": " + message;
        RpcSendMessage(roomID, fullMessage);
    }

    [ClientRpc]
    void RpcSendMessage(int roomID, string message)
    {
        foreach (ChatRoom room in m_chatRooms)
        {
            if (room.ID == roomID)
            {
                room.AddMessage(message);
            }
        }
    }
}
