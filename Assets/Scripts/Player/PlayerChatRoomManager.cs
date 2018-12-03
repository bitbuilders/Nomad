using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerChatRoomManager : NetworkBehaviour
{
    NetworkData m_networkData;
    
    [Command]
    public void CmdCreateChatRoom()
    {
        if (m_networkData == null)
            m_networkData = GameObject.Find("Network Data").GetComponent<NetworkData>();

        m_networkData.IncrementRoomID();
        int room = m_networkData.CurrentRoomID;
        RpcCreate(room);
    }
    
    [ClientRpc]
    void RpcCreate(int room)
    {
        if (isLocalPlayer)
        {
            ChatRoomManager.Instance.CreateChatRoom(room, false);
            //Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
            //string creationMessage = Colors.ConvertToColor(localPlayer.UserName + " created the room", Colors.ColorType.WHITE);
            //localPlayer.GetComponent<ChatRoomMessenger>().SendMessageToRoom(creationMessage, room);
        }
    }
}
