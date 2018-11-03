using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatRoomMessenger : NetworkBehaviour
{
    public void SendMessageToRoom(string message, int roomID)
    {
        CmdSendMessageToServer(message, roomID);
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

    public void SendInviteToRoom(string sender, string playerName, string roomName, int roomID)
    {
        CmdSendInvite(sender, playerName, roomName, roomID);
    }

    [Command]
    void CmdSendInvite(string sender, string playerName, string roomName, int roomID)
    {
        RpcRecieveInvite(sender, playerName, roomName, roomID);
    }

    [ClientRpc]
    void RpcRecieveInvite(string sender, string playerName, string roomName, int roomID)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (localPlayer.UserName == playerName)
        {
            NotificationManager.Instance.CreateNotification(Notification.NotificationType.ROOM_INVITE, sender, roomName, roomID);
        }
    }
}
