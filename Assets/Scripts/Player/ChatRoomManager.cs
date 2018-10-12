using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatRoomManager : NetworkBehaviour
{
    [SerializeField] GameObject m_roomTemplate = null;
    [SerializeField] Transform m_roomLocation = null;

    List<ChatRoom> m_chatRooms;

    private void Start()
    {
        m_chatRooms = new List<ChatRoom>();
        //CreateChatRoom();
    }

    public void CreateChatRoom()
    {
        GameObject obj = Instantiate(m_roomTemplate, Vector3.zero, Quaternion.identity, m_roomLocation);
        m_chatRooms.Add(obj.GetComponent<ChatRoom>());
    }

    public void SendMessage(int roomID, string message)
    {
        RpcAddToChatRoom(roomID, message);
    }

    [ClientRpc]
    void RpcAddToChatRoom(int roomID, string message)
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
