using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatRoomManager : Singleton<ChatRoomManager>
{
    [SerializeField] GameObject m_roomTemplate = null;
    
    List<ChatRoom> m_chatRooms;
    Transform m_roomLocation = null;

    private void Start()
    {
        m_chatRooms = new List<ChatRoom>();
        m_roomLocation = GameObject.Find("Chat Rooms").transform;
    }
    
    public void CreateChatRoom(Player localPlayer, int roomID)
    {
        GameObject obj = Instantiate(m_roomTemplate, Vector3.zero, Quaternion.identity, m_roomLocation);
        ChatRoom chatRoom = obj.GetComponentInChildren<ChatRoom>();
        m_chatRooms.Add(chatRoom);
        chatRoom.Initialize(localPlayer, roomID);
    }

    public void AddMessageToChatRoom(int roomID, string message)
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
