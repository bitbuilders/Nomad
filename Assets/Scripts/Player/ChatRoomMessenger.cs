using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatRoomMessenger : NetworkBehaviour
{
    Player m_localPlayer;

    private void Start()
    {
        m_localPlayer = GetComponent<Player>();
    }

    public void SendMessageToRoom(string message, int roomID)
    {
        string fullMessage = m_localPlayer.UserName + ": " + message;
        CmdSendMessageToServer(fullMessage, roomID);
    }

    [Command]
    void CmdSendMessageToServer(string message, int roomID)
    {
        RpcReceiveMessage(message, roomID);
    }

    // This will be received on the client that sent it, but on every instance connected to the server
    // So if Client 2 sends it on Machine 2, Machine 1 will recieve it on Client 2
    [ClientRpc]
    void RpcReceiveMessage(string message, int roomID)
    {
        ChatRoomManager.Instance.AddMessageToChatRoom(roomID, message);
    }
}
