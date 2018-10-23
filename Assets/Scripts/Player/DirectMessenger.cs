using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DirectMessenger : NetworkBehaviour
{
    Player m_localPlayer;

    private void Start()
    {
        m_localPlayer = GetComponent<Player>();
    }
    
    public void SendDirectMessage(string message, string playerName)
    {
        CmdServerMessage(message, m_localPlayer.UserName, playerName);
    }

    [Command]
    void CmdServerMessage(string message, string sender, string recipient)
    {
        RpcReceiveDirectMessage(message, sender, recipient);
    }

    [ClientRpc]
    void RpcReceiveDirectMessage(string message, string sender, string recipient)
    {
        string localUsername = LocalPlayerData.Instance.LocalPlayer.UserName;
        if (recipient != localUsername && sender != localUsername)
            return;
        
        DirectMessageManager.Instance.AddMessageToRoom(message, sender, recipient);
    }
}
