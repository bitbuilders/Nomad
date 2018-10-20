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

    public void CreateNewConversation(string playerName)
    {
        bool playerExists = LocalPlayerData.Instance.PlayerExists(playerName);
        if (playerExists)
        {
            CmdSendNewConversation(playerName);
        }
    }
    
    [Command]
    void CmdSendNewConversation(string playerName)
    {
        RpcReceiveConversation(playerName);
    }

    [ClientRpc]
    void RpcReceiveConversation(string playerName)
    {
        DirectMessageManager.Instance.CreateMessageRoom(playerName);
    }
}
