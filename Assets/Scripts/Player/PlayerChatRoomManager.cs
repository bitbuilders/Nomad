using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerChatRoomManager : NetworkBehaviour
{
    NetworkData m_networkData;

    private void Start()
    {
        m_networkData = GameObject.Find("Network Data").GetComponent<NetworkData>();
    }
    
    [Command]
    public void CmdCreateChatRoom()
    {
        m_networkData.CmdIncrementRoomID();
        int room = m_networkData.CurrentRoomID;
        RpcCreate(room);
    }
    
    [ClientRpc]
    void RpcCreate(int room)
    {
        print(room);
        if (isLocalPlayer)
            ChatRoomManager.Instance.CreateChatRoom(room);
    }
}
