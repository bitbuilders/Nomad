using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatRoomManager : Singleton<ChatRoomManager>
{
    [SerializeField] GameObject m_roomTemplate = null;
    
    List<ChatRoom> m_chatRooms;
    Transform m_roomLocation = null;
    int m_openRooms = 0;

    private void Start()
    {
        m_chatRooms = new List<ChatRoom>();
        m_roomLocation = GameObject.Find("Chat Rooms").transform;
    }
    
    public void CreateChatRoom(int roomID, string roomName = "Chat Room")
    {
        GameObject obj = Instantiate(m_roomTemplate, Vector3.zero, Quaternion.identity, m_roomLocation);
        obj.transform.SetAsFirstSibling();
        //obj.transform.SetSiblingIndex(m_roomLocation.childCount - 2);
        ChatRoom chatRoom = obj.GetComponentInChildren<ChatRoom>();
        m_chatRooms.Add(chatRoom);
        chatRoom.Initialize(roomID, roomName);
        print("Chat room ID: " + roomID + " created!");
    }

    public void RemoveChatRoom(GameObject room)
    {
        m_chatRooms.Remove(room.GetComponent<ChatRoom>());
        Destroy(room);
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

    public void OpenChatRoom()
    {
        PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        playerMove.AddState(PlayerMovement.PlayerState.CHAT_ROOM);
        m_openRooms++;
    }

    public void HideChatRoom()
    {
        m_openRooms--;

        if (m_openRooms == 0)
        {
            PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
            playerMove.RemoveState(PlayerMovement.PlayerState.CHAT_ROOM);
        }
    }
}
