using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRoomCreator : MonoBehaviour
{
    public void CreateChatRoom()
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        localPlayer.GetComponent<ChatRoomAssigner>().CmdIncrementRoomID();
        int room = localPlayer.GetComponent<ChatRoomAssigner>().RoomID;
        ChatRoomManager.Instance.CreateChatRoom(room);
    }
}
