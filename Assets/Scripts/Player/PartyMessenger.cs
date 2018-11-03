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
        if (localPlayer.UserName == leader)
        {
            PartyManager.Instance.SetupParty(leader);
        }
        else
        {
            if (invited == localPlayer.UserName)
            {
                NotificationManager.Instance.CreateNotification(Notification.NotificationType.PARTY_INVITE, leader, "", 0);
            }
        }
    }

    public void AddPlayerToParty(string player, string addedPlayer)
    {
        CmdAddPlayer(player, addedPlayer);
    }

    [Command]
    void CmdAddPlayer(string player, string addedPlayer)
    {
        RpcReceivePlayer(player, addedPlayer);
    }

    [ClientRpc]
    void RpcReceivePlayer(string player, string addedPlayer)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (localPlayer.UserName == player)
        {
            PartyManager.Instance.AddPlayerToParty(addedPlayer);
        }
    }
}
