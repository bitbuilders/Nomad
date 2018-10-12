﻿using System.Collections;
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

        m_localPlayer = GetComponent<Player>();
        m_roomLocation = GameObject.Find("Chat Rooms").transform;
        CreateChatRoom(1);
    }

    [ServerCallback]
    private void OnEnable()
    {
        m_chatRooms = new List<ChatRoom>();
    }

    public void CreateChatRoom(int roomID)
    {
        GameObject obj = Instantiate(m_roomTemplate, Vector3.zero, Quaternion.identity, m_roomLocation);
        ChatRoom chatRoom = obj.GetComponentInChildren<ChatRoom>();
        m_chatRooms.Add(chatRoom);
        chatRoom.Initialize(m_localPlayer, roomID);
    }

    [Command]
    public void CmdSendMessage(int roomID, string message)
    {
        RpcSendMessage(roomID, message);
    }

    [Server]
    void ServerSendMessage(int roomID, string message)
    {
        RpcSendMessage(roomID, message);
    }

    [ClientRpc]
    public void RpcSendMessage(int roomID, string message)
    {
        AddMessageToChatRoom(roomID, message);
    }

    void AddMessageToChatRoom(int roomID, string message)
    {
        print(m_chatRooms == null);
        foreach (ChatRoom room in m_chatRooms)
        {
            if (room.ID == roomID)
            {
                room.AddMessage(message);
            }
        }
    }
}
