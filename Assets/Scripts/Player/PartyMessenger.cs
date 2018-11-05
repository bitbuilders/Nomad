using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PartyMessenger : NetworkBehaviour
{
    public void SendPlayerInvites(string leader, string invited)
    {
        CmdSendInvites(leader, invited);
    }

    [Command]
    void CmdSendInvites(string leader, string invited)
    {
        RpcReceiveInvites(leader, invited);
    }

    [ClientRpc]
    void RpcReceiveInvites(string leader, string invited)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (invited == localPlayer.UserName)
        {
            NotificationManager.Instance.CreateNotification(Notification.NotificationType.PARTY_INVITE, leader, "", 0);
        }
    }

    public void AddPlayerToParty(string player, string addedPlayer)
    {
        CmdAddPlayer(player, addedPlayer);
    }

    [Command]
    void CmdAddPlayer(string player, string addedPlayer)
    {
        RpcReceivePlayerAdd(player, addedPlayer);
    }

    [ClientRpc]
    void RpcReceivePlayerAdd(string player, string addedPlayer)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (localPlayer.UserName == player)
        {
            PartyManager.Instance.AddPlayerToParty(addedPlayer);
        }
    }

    public void RemovePlayerFromParty(string player, string removedPlayer)
    {
        CmdSendRemovePlayer(player, removedPlayer);
    }

    [Command]
    void CmdSendRemovePlayer(string player, string removedPlayer)
    {
        RpcReceivePlayerRemove(player, removedPlayer);
    }

    [ClientRpc]
    void RpcReceivePlayerRemove(string player, string removedPlayer)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (localPlayer.UserName == player)
        {
            PartyManager.Instance.RemovePlayerFromParty(removedPlayer);
        }
    }

    public void NewLeader(string player, string leader)
    {
        CmdSendNewLeader(player, leader);
    }

    [Command]
    void CmdSendNewLeader(string player, string leader)
    {
        RpcReceiveNewLeader(player, leader);
    }

    [ClientRpc]
    void RpcReceiveNewLeader(string player, string leader)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (localPlayer.UserName == player)
        {
            PartyManager.Instance.MakeLeader(leader);
        }
    }

    public void SendChatMessage(string player, string message)
    {
        CmdSendMessage(player, message);
    }

    [Command]
    void CmdSendMessage(string player, string message)
    {
        RpcReceiveMessage(player, message);
    }

    [ClientRpc]
    void RpcReceiveMessage(string player, string message)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (localPlayer.UserName == player)
        {
            PartyManager.Instance.ReceiveChatMessage(message);
        }
    }
}
