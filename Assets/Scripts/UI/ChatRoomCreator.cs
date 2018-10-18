using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRoomCreator : MonoBehaviour
{
    public void CreateChatRoom()
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        localPlayer.GetComponent<NetworkData>().CmdIncrementRoomID();
        int room = localPlayer.GetComponent<NetworkData>().CurrentRoomID;
        ChatRoomManager.Instance.CreateChatRoom(room);
    }
}
